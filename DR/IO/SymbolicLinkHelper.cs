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
using System.Runtime.InteropServices;

namespace DR.IO
{
    namespace Windows
    {
    	public static class SymbolicLinkHelper
        {
            private const UInt32 SymbolicLinkFlagFile = 0;
            private const UInt32 SymbolicLinkFlagDirectory = 1;
    
            [DllImport("kernel32.dll",SetLastError = true)]
            [return: MarshalAs(UnmanagedType.I1)]
            private static extern bool CreateSymbolicLink(string symlinkFileName, string targetFileName, UInt32 flags);
    
            public static bool CreateSymbolicLink(DirectoryInfo targetDirInfo, FileInfo symlinkFileInfo)
            {
                return CreateSymbolicLink(symlinkFileInfo.ToString(), targetDirInfo.ToString(), SymbolicLinkFlagDirectory);
            }
    
            public static bool CreateSymbolicLink(FileInfo targetFileInfo, FileInfo symlinkFileInfo)
            {
                return CreateSymbolicLink(symlinkFileInfo.ToString(), targetFileInfo.ToString(), SymbolicLinkFlagFile);
            }

            public static bool IsSymbolicLink(FileSystemInfo symlinkFileSystemInfo)
            {
                // TODO Implement method.
                throw new NotImplementedException("bool IsSymbolicLink(FileSystemInfo symlinkFileSystemInfo)");
            }
    
            public static Uri GetSymbolicLinkTarget(FileSystemInfo symlinkFileSystemInfo)
            {
                // TODO Implement method.
                throw new NotImplementedException("Uri GetSymbolicLinkTarget(FileSystemInfo symlinkFileSystemInfo)");
            }
        }
    }

    namespace Unix
    {
        public static class SymbolicLinkHelper
        {
            private const string MonoPosixAssemblyName = "Mono.Posix, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756";
            private const string UnixSymbolicLinkInfoTypeName = "Mono.Unix.UnixSymbolicLinkInfo";
            private const string CreateSymbolicLinkToMethodName = "CreateSymbolicLinkTo";
            private const string GetContentsMethodName = "GetContents";
            private const string IsSymbolicLinkPropertyName = "IsSymbolicLink";
            private const string FullNamePropertyName = "FullName";

            private static Type UnixSymbolicLinkInfoType;
            private static MethodInfo CreateSymbolicLinkToMethod;
            private static MethodInfo GetContentsMethod;
            private static PropertyInfo IsSymbolicLinkProperty;
            private static PropertyInfo FullNameProperty;
            
        	private static bool _initialized = false;

            private static void Init()
            {
				if (_initialized)
					return;
                
                var assemblyRef = new AssemblyName { Name = MonoPosixAssemblyName };
                var assembly = Assembly.Load(assemblyRef);
                
                UnixSymbolicLinkInfoType = assembly.GetType(UnixSymbolicLinkInfoTypeName);
                
                CreateSymbolicLinkToMethod = UnixSymbolicLinkInfoType.GetMethod(CreateSymbolicLinkToMethodName, new[] { typeof(string) });
                GetContentsMethod = UnixSymbolicLinkInfoType.GetMethod(GetContentsMethodName);
                IsSymbolicLinkProperty = UnixSymbolicLinkInfoType.GetProperty(IsSymbolicLinkPropertyName);
                FullNameProperty = UnixSymbolicLinkInfoType.GetProperty(FullNamePropertyName);
                
                _initialized = true;
            }
            
            private static void CreateSymbolicLink(string targetPath, string symlinkPath)
            {
                var usli = Activator.CreateInstance(UnixSymbolicLinkInfoType, symlinkPath);
                CreateSymbolicLinkToMethod.Invoke(usli, new[] { targetPath });
            }
            
            public static bool CreateSymbolicLink(DirectoryInfo targetDirInfo, FileInfo symlinkFileInfo)
            {
                try
                {
                    Init();
                    CreateSymbolicLink(targetDirInfo.ToString(), symlinkFileInfo.ToString());
                    return true;
                }
                catch (Exception ex)
                {
					System.Diagnostics.Debug.WriteLine(ex);
                    return false;
                }
            }
    
            public static bool CreateSymbolicLink(FileInfo targetFileInfo, FileInfo symlinkFileInfo)
            {
                try
                {
                    Init();
                    CreateSymbolicLink(targetFileInfo.ToString(), symlinkFileInfo.ToString());
                    return true;
                }
                catch (Exception ex)
                {
					System.Diagnostics.Debug.WriteLine(ex);
                    return false;
                }
            }

            public static bool IsSymbolicLink(FileSystemInfo symlinkFileSystemInfo)
            {
                Init();
                var usli = Activator.CreateInstance(UnixSymbolicLinkInfoType, symlinkFileSystemInfo.ToString());
                return (bool)IsSymbolicLinkProperty.GetValue(usli, new object[0]);
            }
    
            public static Uri GetSymbolicLinkTarget(FileSystemInfo symlinkFileSystemInfo)
            {
                Init();
                var usli = Activator.CreateInstance(UnixSymbolicLinkInfoType, symlinkFileSystemInfo.ToString());
                var ufsi = GetContentsMethod.Invoke(usli, new object[0]);
                return new Uri((string)FullNameProperty.GetValue(ufsi, new object[0]));
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
            catch (Exception ex)
            {
				System.Diagnostics.Debug.WriteLine(ex);
            }

			try
			{
				return Unix.SymbolicLinkHelper.CreateSymbolicLink(targetFileInfo, symlinkFileInfo);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}

        	return false;
        }

        public static bool CreateSymbolicLink(DirectoryInfo targetDirInfo, FileInfo symlinkFileInfo)
        {
            try
            {
                return Windows.SymbolicLinkHelper.CreateSymbolicLink(targetDirInfo, symlinkFileInfo);
            }
            catch (Exception ex)
            {
				System.Diagnostics.Debug.WriteLine(ex);
			}

			try
			{
				return Unix.SymbolicLinkHelper.CreateSymbolicLink(targetDirInfo, symlinkFileInfo);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}

			return false;
		}

        public static bool IsSymbolicLink(FileSystemInfo symlinkFileSystemInfo)
        {
            try
            {
                return Windows.SymbolicLinkHelper.IsSymbolicLink(symlinkFileSystemInfo);
            }
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}

			try
			{
				return Unix.SymbolicLinkHelper.IsSymbolicLink(symlinkFileSystemInfo);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}

			return false;
		}

        public static Uri GetSymbolicLinkTarget(FileSystemInfo symlinkFileSystemInfo)
        {
            try
            {
                return Windows.SymbolicLinkHelper.GetSymbolicLinkTarget(symlinkFileSystemInfo);
            }
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}

			try
			{
				return Unix.SymbolicLinkHelper.GetSymbolicLinkTarget(symlinkFileSystemInfo);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}

			return null;
		}
    }
}
