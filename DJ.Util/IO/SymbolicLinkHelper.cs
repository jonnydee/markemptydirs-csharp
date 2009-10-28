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

using System;
using System.IO;
using System.Reflection;

namespace DJ.Util.IO
{
    namespace Windows
    {
        using System.Runtime.InteropServices;
        
        public static class SymbolicLinkHelper
        {
            private const UInt32 SymbolicLinkFlagFile = 0;
            private const UInt32 SymbolicLinkFlagDirectory = 1;
    
            [DllImport("kernel32.dll",SetLastError = true)]
            [return: MarshalAs(UnmanagedType.I1)]
            private static extern bool CreateSymbolicLink(string symlinkFileName, string targetFileName, UInt32 flags);
    
            public static bool CreateSymbolicLink(DirectoryInfo targetDirInfo, FileInfo symlinkFileInfo)
            {
                return CreateSymbolicLink(symlinkFileInfo.FullName, targetDirInfo.FullName, SymbolicLinkFlagDirectory);
            }
    
            public static bool CreateSymbolicLink(FileInfo targetFileInfo, FileInfo symlinkFileInfo)
            {
                return CreateSymbolicLink(symlinkFileInfo.FullName, targetFileInfo.FullName, SymbolicLinkFlagFile);
            }
        }
    }

    namespace Unix
    {
        public static class SymbolicLinkHelper
        {
            private const string MonoPosixAssemblyName = "Mono.Posix, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756";
            private const string UnixDirectoryInfoTypeName = "Mono.Unix.UnixDirectoryInfo";
            private const string UnixFileInfoTypeName = "Mono.Unix.UnixFileInfo";
            private const string CreateSymbolicLinkMethodName = "CreateSymbolicLink";

            private static readonly Type UnixDirectoryInfoType;
            private static readonly Type UnixFileInfoType;

            static SymbolicLinkHelper()
            {
                var assemblyRef = new AssemblyName { Name = MonoPosixAssemblyName };
                var assembly = Assembly.Load(assemblyRef);
                UnixFileInfoType = assembly.GetType(UnixFileInfoTypeName);
                UnixDirectoryInfoType = assembly.GetType(UnixDirectoryInfoTypeName);
            }
            
            private static void CreateSymbolicLink(Type unixFileSystemInfoType, string targetPath, string symlinkPath)
            {
                var ufi = Activator.CreateInstance(unixFileSystemInfoType, targetPath);
                var method = unixFileSystemInfoType.GetMethod(CreateSymbolicLinkMethodName);
                method.Invoke(ufi, new[] { symlinkPath });
            }
            
            public static bool CreateSymbolicLink(DirectoryInfo targetDirInfo, FileInfo symlinkFileInfo)
            {
                try
                {
                    CreateSymbolicLink(UnixDirectoryInfoType, targetDirInfo.FullName, symlinkFileInfo.FullName);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
    
            public static bool CreateSymbolicLink(FileInfo targetFileInfo, FileInfo symlinkFileInfo)
            {
                try
                {
                    CreateSymbolicLink(UnixFileInfoType, targetFileInfo.FullName, symlinkFileInfo.FullName);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }

    public static class SymbolicLinkHelper
    {
        public static bool CreateSymbolicLink(FileInfo targetFileInfo, FileInfo symlinkFileInfo)
        {
            try
            {
                return Windows.SymbolicLinkHelper.CreateSymbolicLink(targetFileInfo, symlinkFileInfo);
            }
            catch (EntryPointNotFoundException)
            {
                return Unix.SymbolicLinkHelper.CreateSymbolicLink(targetFileInfo, symlinkFileInfo);
            }
        }

        public static bool CreateSymbolicLink(DirectoryInfo targetDirInfo, FileInfo symlinkFileInfo)
        {
            try
            {
                return Windows.SymbolicLinkHelper.CreateSymbolicLink(targetDirInfo, symlinkFileInfo);
            }
            catch (EntryPointNotFoundException)
            {
                return Unix.SymbolicLinkHelper.CreateSymbolicLink(targetDirInfo, symlinkFileInfo);
            }
        }
    }
}
