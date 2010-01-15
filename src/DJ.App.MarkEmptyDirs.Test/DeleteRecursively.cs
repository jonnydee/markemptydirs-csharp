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

using System;
using System.IO;

using DR.IO;

namespace DJ.App.MarkEmptyDirs
{
    class DeleteRecursively : IDirectoryVisitor
    {
        public static void Delete(DirectoryInfo dirInfo)
        {
            var walker = DirectoryWalker.Create(new DeleteRecursively());
            walker.Walk(dirInfo);
        }        

        public bool PreVisit(DirectoryInfo dirInfo)
        {
            if (SymbolicLinkHelper.IsSymbolicLink(dirInfo))
            {
                dirInfo.Delete();
                return false;
            }
            return true;
        }
                
        public bool PostVisit(DirectoryInfo dirInfo)
        {
            if (dirInfo.Exists)
                dirInfo.Delete();

            return true;
        }

        public bool Visit(FileInfo fileInfo)
        {
            if (fileInfo.Exists)
                fileInfo.Delete();
                
            return true;
        }
    }
}
