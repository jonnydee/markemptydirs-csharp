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

namespace DJ.Util.IO
{
    public static class DirectoryWalker
    {
        public static void Walk(FileSystemInfo fileSystemInfo, IDirectoryVisitor visitor)
        {
            if (!fileSystemInfo.Exists)
                return;

            if ((fileSystemInfo.Attributes & FileAttributes.Directory) != 0)
            {
                var dirInfo = (DirectoryInfo)fileSystemInfo;

                if (visitor.PreVisit(dirInfo))
                {
                    var subFileSystemInfos = dirInfo.GetFileSystemInfos();
                    foreach (var subFileSystemInfo in subFileSystemInfos)
                        Walk(subFileSystemInfo, visitor);
                }

                visitor.PostVisit(dirInfo);
            }
            else
            {
                var fileInfo = (FileInfo)fileSystemInfo;

                visitor.Visit(fileInfo);
            }
        }
    }
}
