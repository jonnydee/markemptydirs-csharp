//  Copyright (C) 2009 by Johann Duscher (alias Jonny Dee)
//
//  This file is part of MarkEmptyDirs.
//
//  MarkEmptyDirs is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  MarkEmptyDirs is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with MarkEmptyDirs.  If not, see <http://www.gnu.org/licenses/>.

using System.IO;

namespace DR.Util.IO
{
    public static class DirectoryWalker
    {
        public static bool Walk(FileSystemInfo fileSystemInfo, IDirectoryVisitor visitor)
        {
            return Walk(fileSystemInfo, visitor, true);
        }
        
        public static bool Walk(FileSystemInfo fileSystemInfo, IDirectoryVisitor visitor, bool visitFiles)
        {
            if (!fileSystemInfo.Exists)
                return false;

            // Check if we are walking a directory.
            
            if ((fileSystemInfo.Attributes & FileAttributes.Directory) != 0)
            {
                var dirInfo = (DirectoryInfo)fileSystemInfo;

                if (visitor.PreVisit(dirInfo))
                {
                    bool continueWalking = true;
                    
                    var subDirectories = dirInfo.GetDirectories();
                    foreach (var subDirectory in subDirectories)
                    {
                        continueWalking = Walk(subDirectory, visitor, visitFiles);
                        if (!continueWalking)
                            break;
                    }

                    if (visitFiles && continueWalking)
                    {
                        var files = dirInfo.GetFiles();
                        foreach (var file in files)
                        {
                            continueWalking = Walk(file, visitor, visitFiles);
                            if (!continueWalking)
                                break;
                        }
                    }

                    return visitor.PostVisit(dirInfo) && continueWalking;
                }

                return true;
            }

            // We must be walking a file.
            
            var fileInfo = (FileInfo)fileSystemInfo;
            
            return visitor.Visit(fileInfo);
        }
    }
}
