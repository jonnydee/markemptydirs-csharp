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

using System;
using System.Collections.Generic;
using System.IO;

namespace DR.IO
{
    public static class PathUtil
    {
        public static string Combine(params string[] pathComponents)
        {
            if (null == pathComponents)
                throw new ArgumentNullException("pathComponents");

            if (pathComponents.Length == 0)
                return string.Empty;
            
            var path = pathComponents[0];

            for (var i = 1; i < pathComponents.Length; i++)
                path = Path.Combine(path, pathComponents[i]);

            return path;
        }

        public static string[] Split(string path)
        {
            return path.Split(new[] { Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static FileSystemInfo CreateFileSystemInfo(string path)
        {
            if (Directory.Exists(path))
                return new DirectoryInfo(path);
            
            if (File.Exists(path))
                return new FileInfo(path);
            
            throw new FileNotFoundException("File not existent", path);
        }
        
        public static DirectoryInfo GetParent(FileSystemInfo info)
        {
            if (info is DirectoryInfo)
                return ((DirectoryInfo)info).Parent;
            if (info is FileInfo)
                return ((FileInfo)info).Directory;
            throw new ArgumentException(string.Format("Unknown FileSystemInfo type: {0}", info.GetType().AssemblyQualifiedName), "info"); 
        }

        public static bool TreeContains(DirectoryInfo tree, FileSystemInfo info)
        {
            return info.FullName.StartsWith(tree.FullName);
        }

        public static string[] GetRelativePath(DirectoryInfo root, FileSystemInfo info)
        {
            if (!TreeContains(root, info))
                throw new ArgumentException(string.Format("Path '{0}' not under tree '{1}'", info.FullName, root.FullName), "info");
            
            // Traverse the directory tree upwards until the repository root is reached
            // and collect all intermediate FileSystemInfo path components.
            var pathComponents = new List<string>();
            while (info.FullName != root.FullName)
            {
                pathComponents.Add(info.Name);
                info = GetParent(info);
            }
            pathComponents.Reverse();

            return pathComponents.ToArray();
        }
    }
}
