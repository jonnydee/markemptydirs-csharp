using System;
using System.IO;

namespace DJ.Util.IO
{
	public static class DirectoryWalker
	{
		public static void Walk(FileSystemInfo fileSystemInfo, IDirectoryVisitor visitor)
		{
			if (!fileSystemInfo.Exists)
				return;
			
			if (fileSystemInfo.Attributes == FileAttributes.Directory)
			{
				var dirInfo = (DirectoryInfo)fileSystemInfo;

				if (visitor.PreVisit(dirInfo))
				{
					var subFileSystemInfos = dirInfo.GetFileSystemInfos();
					foreach (var subFileSystemInfo in subFileSystemInfos)
						Walk(subFileSystemInfo, visitor);
				}
				
				visitor.PostVisit(dirInfo);			
			}
			else
			{
				var fileInfo = (FileInfo)fileSystemInfo;
				
				visitor.Visit(fileInfo);
			}
		}
	}	
}
