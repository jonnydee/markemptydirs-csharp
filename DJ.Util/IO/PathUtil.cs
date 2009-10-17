using System;
using System.Collections.Generic;
using System.IO;

namespace DJ.Util.IO
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
