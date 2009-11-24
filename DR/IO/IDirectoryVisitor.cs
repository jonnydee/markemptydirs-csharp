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

namespace DR.IO
{
    public interface IDirectoryVisitor
    {
        /// <summary>
        /// Is called before a directory is walked recursively.
        /// </summary>
        /// <param name="dirInfo">
        /// A <see cref="DirectoryInfo"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/> which indicates if a directory should be walked recursively (<code>true</code>)
        /// or not (<code>false</code>).
        /// </returns>
        bool PreVisit(DirectoryInfo dirInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirInfo">
        /// A <see cref="DirectoryInfo"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        bool PostVisit(DirectoryInfo dirInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo">
        /// A <see cref="FileInfo"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        bool Visit(FileInfo fileInfo);

    }
}
