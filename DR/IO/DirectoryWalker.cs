//  
//  Copyright (c) 2009 by Johann Duscher (alias Jonny Dee)
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

namespace DR.IO
{
    public static class DirectoryWalker
    {
        public static DirectoryWalker<TVisitor> Create<TVisitor>(TVisitor visitor) where TVisitor : IDirectoryVisitor
        {
            return new DirectoryWalker<TVisitor>(visitor);
        }
    }
    
    public class DirectoryWalker<TVisitor> where TVisitor : IDirectoryVisitor
    {
		public TVisitor Visitor { get; protected set; }
		
		public bool VisitFiles { get; set; }
		
		
		public DirectoryWalker(TVisitor visitor)
		{
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
                var dirInfo = (DirectoryInfo)fileSystemInfo;

                if (Visitor.PreVisit(dirInfo))
                {
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

                    return Visitor.PostVisit(dirInfo) && continueWalking;
                }

                return true;
            }

            // We must be walking a file.
            
            var fileInfo = (FileInfo)fileSystemInfo;
            
            return Visitor.Visit(fileInfo);
        }
    }
}
