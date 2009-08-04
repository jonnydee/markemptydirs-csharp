using System;
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
    }
}
