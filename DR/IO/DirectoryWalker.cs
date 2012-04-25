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
        IDictionary<string, FileInfo> VisitedFiles { get; }
        IDictionary<string, DirectoryInfo> VisitedDirectories { get; }
    }
    
    public class DirectoryWalker<TVisitor> : IDirectoryWalkerContext where TVisitor : IDirectoryVisitor
    {
        public TVisitor Visitor { get; protected set; }
        
        public bool FollowSymbolicLinks { get; set; }
        public bool VisitFiles { get; set; }
        public bool TrackVisitedFiles { get; set; }
        public bool TrackVisitedDirectories { get; set; }

        private Dictionary<string, FileInfo> _visitedFiles;
        private Dictionary<string, DirectoryInfo> _visitedDirectories;
        
        public IDictionary<string, FileInfo> VisitedFiles { get { return _visitedFiles; } }

        public IDictionary<string, DirectoryInfo> VisitedDirectories { get { return _visitedDirectories; } }
        
        //public IEnumerable<FileSystemInfo> VisitedFileSystemInfos
        //{
        //    get
        //    {
        //        foreach (FileSystemInfo dirInfo in VisitedDirectories.Values)
        //            yield return dirInfo;
        //        foreach (FileSystemInfo fileInfo in VisitedFiles.Values)
        //            yield return fileInfo;
        //    }
        //}
        
        public DirectoryWalker(TVisitor visitor)
        {
            _visitedFiles = new Dictionary<string, FileInfo>();
            _visitedDirectories = new Dictionary<string, DirectoryInfo>();
            
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
            bool isSymbolicLink = SymbolicLinkHelper.IsSymbolicLink(dirInfo);
            
            // First check if we have already visited dirInfo. If so, we do not have
            // to do anything.
            if (IsVisited(dirInfo))
                return true;
            
            DirectoryInfo targetDirInfo = null;
			
            // If dirInfo is a symlink -- and we must not follow symlinks or we have
            // already visited targetDirInfo, we do not have to do anything except
            // remembering that we have visited dirInfo.
            if (isSymbolicLink)
            {
                if (!FollowSymbolicLinks || IsVisited(targetDirInfo = GetAbsoluteTarget(dirInfo)))
                {
                    AddVisited(dirInfo);
                    return true;
                }
            }
            
            // At this stage dirInfo either is a normal directory,
            // or it is a symlink and we must follow symlinks.
            // In both cases, we need to recurse into dirInfo.
            
            if (Visitor.PreVisit(this, dirInfo))
            {
                // Remember that we have visited dirInfo.
                AddVisited(dirInfo);
                // If dirInfo is a symlink we also remember that we have visited
                // targetDirInfo, because visiting the symlink dirInfo is equivalent.
                if (isSymbolicLink)
                    AddVisited(targetDirInfo);
                
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
            
            AddVisited(fileInfo);
            
            return continueWalking;
        }
        
        protected void AddVisited(DirectoryInfo dirInfo)
        {
            if (null != dirInfo && TrackVisitedDirectories && !IsVisited(dirInfo))
                VisitedDirectories.Add(dirInfo.FullName, dirInfo);
        }
        
        protected void AddVisited(FileInfo fileInfo)
        {
            if (null != fileInfo && TrackVisitedFiles && !IsVisited(fileInfo))
                VisitedFiles.Add(fileInfo.FullName, fileInfo);
        }
        
        public bool IsVisited(FileInfo fileInfo)
        {
            if (TrackVisitedFiles && null != fileInfo)
                return VisitedFiles.ContainsKey(fileInfo.FullName);
            
            return false;
        }
        
        public bool IsVisited(DirectoryInfo dirInfo)
        {
            if (TrackVisitedDirectories && null != dirInfo)
                return VisitedDirectories.ContainsKey(dirInfo.FullName);
            
            return false;
        }
        
        public bool IsVisited(FileSystemInfo fileSystemInfo)
        {
            if (null == fileSystemInfo)
                return false;
            
            if (fileSystemInfo is DirectoryInfo)
                return IsVisited((DirectoryInfo)fileSystemInfo);
            
            return IsVisited((FileInfo)fileSystemInfo);
        }
        
        private DirectoryInfo GetAbsoluteTarget(DirectoryInfo dirInfo)
        {
            // Get the symlink's targetPath and
            // if it is relative make it absolute based on dirInfo.
            var targetPath = SymbolicLinkHelper.GetSymbolicLinkTargetAbsolute(dirInfo);
            return new DirectoryInfo(targetPath);
        }
    }
}
