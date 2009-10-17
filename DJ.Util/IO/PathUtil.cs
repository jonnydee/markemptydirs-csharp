using System;
using System.Collections.Generic;
using System.IO;

namespace DJ.Util.IO
{
    public static class PathUtil
    {
        public static string Combine(params string[] pathComponents)
        {
            if (null == pathComponents || pathComponents.Length == 0)
                throw new ArgumentException("At least one path component must be provided", "pathComponents");

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
    }
}
