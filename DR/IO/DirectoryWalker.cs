//  
//  Copyright (c) 2009-2010 by Johann Duscher (alias Jonny Dee)
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System.IO;
using System.Collections.Generic;

namespace DR.IO
{
    public static class DirectoryWalker
    {
        public static DirectoryWalker<TVisitor> Create<TVisitor>(TVisitor visitor) where TVisitor : IDirectoryVisitor
        {
            return new DirectoryWalker<TVisitor>(visitor);
        }
    }
    
    public interface IDirectoryWalkerContext
    {
        IList<FileInfo> VisitedFiles { get; }
        IList<DirectoryInfo> VisitedDirectories { get; }
        IEnumerable<FileSystemInfo> VisitedFileSystemInfos { get; }
    }
    
    public class DirectoryWalker<TVisitor> : IDirectoryWalkerContext where TVisitor : IDirectoryVisitor
    {
		public TVisitor Visitor { get; protected set; }
		
        public bool FollowSymbolicLinks { get; set; }
		public bool VisitFiles { get; set; }
        public bool TrackVisitedFiles { get; set; }
        public bool TrackVisitedDirectories { get; set; }

		private List<FileInfo> _visitedFiles;
        private List<DirectoryInfo> _visitedDirectories;
        
        public IList<FileInfo> VisitedFiles { get { return _visitedFiles; } }
        
        public IList<DirectoryInfo> VisitedDirectories { get { return _visitedDirectories; } }
        
        public IEnumerable<FileSystemInfo> VisitedFileSystemInfos
        {
            get
            {
                foreach (var dirInfo in VisitedDirectories)
                    yield return dirInfo;
                foreach (var fileInfo in VisitedFiles)
                    yield return fileInfo;
            }
        }
        
		public DirectoryWalker(TVisitor visitor)
		{
            _visitedFiles = new List<FileInfo>();
            _visitedDirectories = new List<DirectoryInfo>();
            
            Visitor = visitor;
            VisitFiles = true;
		}
		
        public bool Walk(FileSystemInfo fileSystemInfo)
        {
            if (!fileSystemInfo.Exists)
                return false;

            // Check if we are walking a directory.
            
            if ((fileSystemInfo.Attributes & FileAttributes.Directory) != 0)
            {
                return Walk((DirectoryInfo)fileSystemInfo);
            }
            
            // We must be walking a file.
            
            return Walk((FileInfo)fileSystemInfo);
        }
        
        protected bool Walk(DirectoryInfo dirInfo)
        {
            if (SymbolicLinkHelper.IsSymbolicLink(dirInfo))
            {
                if (TrackVisitedDirectories)
                    _visitedDirectories.Add(dirInfo);
                
                if (!FollowSymbolicLinks)
                {
                    return false;
                }

                // Get the symlink's targetPath and
                // if it is relative make it absolute based on dirInfo.
                var targetPath = SymbolicLinkHelper.GetSymbolicLinkTarget(dirInfo);
                if (!Path.IsPathRooted(targetPath))
                {
                    var parentDir = PathUtil.GetParent(dirInfo);
                    targetPath = Path.Combine(parentDir.FullName, targetPath);
                    // Normalize path.
                    targetPath = Path.GetFullPath(targetPath);
                }

                if (IsVisited(targetPath))
                {
                    return false;
                }
                
                if (TrackVisitedDirectories)
                    _visitedDirectories.Add(new DirectoryInfo(targetPath));
            }
            
            if (Visitor.PreVisit(this, dirInfo))
            {
	            if (TrackVisitedDirectories)
	                _visitedDirectories.Add(dirInfo);
                
                bool continueWalking = true;
                
                var subDirectories = dirInfo.GetDirectories();
                foreach (var subDirectory in subDirectories)
                {
                    continueWalking = Walk(subDirectory);
                    if (!continueWalking)
                        break;
                }

                if (VisitFiles && continueWalking)
                {
                    var files = dirInfo.GetFiles();
                    foreach (var file in files)
                    {
                        continueWalking = Walk(file);
                        if (!continueWalking)
                            break;
                    }
                }

                return Visitor.PostVisit(this, dirInfo) && continueWalking;
            }

            return true;
		}
        
        protected bool Walk(FileInfo fileInfo)
        {
            bool continueWalking = Visitor.Visit(this, fileInfo);
            
            if (TrackVisitedFiles)
                _visitedFiles.Add(fileInfo);
            
            return continueWalking;
        }
        
        protected bool IsVisited(string path)
        {
            foreach (var visitedFileSystemInfo in VisitedFileSystemInfos)
            {
                if (path == visitedFileSystemInfo.FullName)
                    return true;
            }
            return false;
        }
    }
}
