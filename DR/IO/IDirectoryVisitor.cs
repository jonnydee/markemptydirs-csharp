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
    public interface IDirectoryVisitor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">
        /// A <see cref="IDirectoryWalkerContext"/>
        /// </param>
        /// <param name="dirInfo">
        /// A <see cref="DirectoryInfo"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        bool PreVisit(IDirectoryWalkerContext context, DirectoryInfo dirInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">
        /// A <see cref="IDirectoryWalkerContext"/>
        /// </param>
        /// <param name="dirInfo">
        /// A <see cref="DirectoryInfo"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        bool PostVisit(IDirectoryWalkerContext context, DirectoryInfo dirInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">
        /// A <see cref="IDirectoryWalkerContext"/>
        /// </param>
        /// <param name="fileInfo">
        /// A <see cref="FileInfo"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        bool Visit(IDirectoryWalkerContext context, FileInfo fileInfo);

    }
}
