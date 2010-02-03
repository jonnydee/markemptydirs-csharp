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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DR.IO
{
    internal enum SymbolicLinkSupport
    {
        Undefined,
        Supported,
        NotSupported,
    }
    
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
    
            public static string GetSymbolicLinkTarget(FileSystemInfo symlinkFileSystemInfo)
            {
                // TODO Implement method.
                throw new NotImplementedException("string GetSymbolicLinkTarget(FileSystemInfo symlinkFileSystemInfo)");
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
            private const string IsSymbolicLinkPropertyName = "IsSymbolicLink";
            private const string ContentsPathPropertyName = "ContentsPath";

            private static Type UnixSymbolicLinkInfoType;
            private static MethodInfo CreateSymbolicLinkToMethod;
            private static PropertyInfo IsSymbolicLinkProperty;
            private static PropertyInfo ContentsPathProperty;
            
        	private static bool _initialized = false;

            private static void Init()
            {
                if (_initialized)
                    return;

                try
                {
                    var assemblyRef = new AssemblyName { Name = MonoPosixAssemblyName };
                    var assembly = Assembly.Load(assemblyRef);

                    UnixSymbolicLinkInfoType = assembly.GetType(UnixSymbolicLinkInfoTypeName);

                    CreateSymbolicLinkToMethod = UnixSymbolicLinkInfoType.GetMethod(CreateSymbolicLinkToMethodName, new[] { typeof(string) });
                    IsSymbolicLinkProperty = UnixSymbolicLinkInfoType.GetProperty(IsSymbolicLinkPropertyName);
                    ContentsPathProperty = UnixSymbolicLinkInfoType.GetProperty(ContentsPathPropertyName);

                    _initialized = true;

                }
                catch (Exception ex)
                {
                    throw new NotSupportedException("symbolic links", ex);
                }
            }
            
            private static void CreateSymbolicLink(string targetPath, string symlinkPath)
            {
                var usli = Activator.CreateInstance(UnixSymbolicLinkInfoType, symlinkPath);
                CreateSymbolicLinkToMethod.Invoke(usli, new[] { targetPath });
            }
            
            public static bool CreateSymbolicLink(DirectoryInfo targetDirInfo, FileInfo symlinkFileInfo)
            {
                Init();
                CreateSymbolicLink(targetDirInfo.ToString(), symlinkFileInfo.ToString());
                return true;
            }
    
            public static bool CreateSymbolicLink(FileInfo targetFileInfo, FileInfo symlinkFileInfo)
            {
                Init();
                CreateSymbolicLink(targetFileInfo.ToString(), symlinkFileInfo.ToString());
                return true;
            }

            public static bool IsSymbolicLink(FileSystemInfo symlinkFileSystemInfo)
            {
                Init();
                var usli = Activator.CreateInstance(UnixSymbolicLinkInfoType, symlinkFileSystemInfo.ToString());
                return (bool)IsSymbolicLinkProperty.GetValue(usli, new object[0]);
            }
    
            public static string GetSymbolicLinkTarget(FileSystemInfo symlinkFileSystemInfo)
            {
                Init();
                var usli = Activator.CreateInstance(UnixSymbolicLinkInfoType, symlinkFileSystemInfo.ToString());
                return (string)ContentsPathProperty.GetValue(usli, new object[0]);
            }
        }
    }

    public static class SymbolicLinkHelper
    {
        private static SymbolicLinkSupport _windowsSymbolicLinkSupport = SymbolicLinkSupport.Undefined;
        private static SymbolicLinkSupport _unixSymbolicLinkSupport = SymbolicLinkSupport.Undefined;
        
        public static bool CreateSymbolicLink(FileInfo targetFileInfo, FileInfo symlinkFileInfo)
        {
            if (SymbolicLinkSupport.NotSupported != _windowsSymbolicLinkSupport)
            {
                try
                {
                    return Windows.SymbolicLinkHelper.CreateSymbolicLink(targetFileInfo, symlinkFileInfo);
                }
                catch (EntryPointNotFoundException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    _windowsSymbolicLinkSupport = SymbolicLinkSupport.NotSupported;
                }
                catch (Exception ex)
                {
    				System.Diagnostics.Debug.WriteLine(ex);
                }
            }
            
            if (SymbolicLinkSupport.NotSupported != _unixSymbolicLinkSupport)
            {
    			try
    			{
    				return Unix.SymbolicLinkHelper.CreateSymbolicLink(targetFileInfo, symlinkFileInfo);
    			}
                catch (NullReferenceException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    _unixSymbolicLinkSupport = SymbolicLinkSupport.NotSupported;
                }
    			catch (Exception ex)
    			{
    				System.Diagnostics.Debug.WriteLine(ex);
    			}
            }
            
        	return false;
        }

        public static bool CreateSymbolicLink(DirectoryInfo targetDirInfo, FileInfo symlinkFileInfo)
        {
            if (SymbolicLinkSupport.NotSupported != _windowsSymbolicLinkSupport)
            {
                try
                {
                    return Windows.SymbolicLinkHelper.CreateSymbolicLink(targetDirInfo, symlinkFileInfo);
                }
                catch (EntryPointNotFoundException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    _windowsSymbolicLinkSupport = SymbolicLinkSupport.NotSupported;
                }
                catch (Exception ex)
                {
    				System.Diagnostics.Debug.WriteLine(ex);
    			}
            }
            
            if (SymbolicLinkSupport.NotSupported != _unixSymbolicLinkSupport)
            {
    			try
    			{
    				return Unix.SymbolicLinkHelper.CreateSymbolicLink(targetDirInfo, symlinkFileInfo);
    			}
                catch (NullReferenceException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    _unixSymbolicLinkSupport = SymbolicLinkSupport.NotSupported;
                }
    			catch (Exception ex)
    			{
    				System.Diagnostics.Debug.WriteLine(ex);
    			}
            }
            
			return false;
		}

        public static bool IsSymbolicLink(FileSystemInfo symlinkFileSystemInfo)
        {
            if (SymbolicLinkSupport.NotSupported != _windowsSymbolicLinkSupport)
            {
                try
                {
                    return Windows.SymbolicLinkHelper.IsSymbolicLink(symlinkFileSystemInfo);
                }
                catch (EntryPointNotFoundException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    _windowsSymbolicLinkSupport = SymbolicLinkSupport.NotSupported;
                }
    			catch (Exception ex)
    			{
    				System.Diagnostics.Debug.WriteLine(ex);
    			}
            }
            
            if (SymbolicLinkSupport.NotSupported != _unixSymbolicLinkSupport)
            {
    			try
    			{
    				return Unix.SymbolicLinkHelper.IsSymbolicLink(symlinkFileSystemInfo);
    			}
                catch (NotSupportedException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    _unixSymbolicLinkSupport = SymbolicLinkSupport.NotSupported;
                }
    			catch (Exception ex)
    			{
    				System.Diagnostics.Debug.WriteLine(ex);
                    throw;
    			}
            }
            
			return false;
		}

        public static string GetSymbolicLinkTarget(FileSystemInfo symlinkFileSystemInfo)
        {
            if (SymbolicLinkSupport.NotSupported != _windowsSymbolicLinkSupport)
            {
                try
                {
                    return Windows.SymbolicLinkHelper.GetSymbolicLinkTarget(symlinkFileSystemInfo);
                }
                catch (EntryPointNotFoundException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    _windowsSymbolicLinkSupport = SymbolicLinkSupport.NotSupported;
                }
    			catch (Exception ex)
    			{
    				System.Diagnostics.Debug.WriteLine(ex);
    			}
            }
            
            if (SymbolicLinkSupport.NotSupported != _unixSymbolicLinkSupport)
            {
    			try
    			{
    				return Unix.SymbolicLinkHelper.GetSymbolicLinkTarget(symlinkFileSystemInfo);
    			}
                catch (NullReferenceException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    _unixSymbolicLinkSupport = SymbolicLinkSupport.NotSupported;
                }
    			catch (Exception ex)
    			{
    				System.Diagnostics.Debug.WriteLine(ex);
    			}
            }
            
			return null;
		}
    }
}
