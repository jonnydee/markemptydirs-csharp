using System;
using System.IO;
using System.Text;

using DJ.Util.IO;

namespace DJ.App.MarkEmptyDirs
{
	class MarkEmptyDirsVisitor : IDirectoryVisitor
	{
		public MarkEmptyDirsVisitor()
		{
			PlaceHolderName = ".emptydir";
			PlaceHolderText = string.Empty;
			Verbose = false;
		}
		
		public string PlaceHolderName { set; get; }
		
		public string PlaceHolderText { set; get; }
		
		public bool Verbose { set; get; }
		
		public bool PreVisit(DirectoryInfo dirInfo)
		{
			return true;
		}
		
		public bool PostVisit(DirectoryInfo dirInfo)
		{
			var createPlaceHolder = dirInfo.GetFileSystemInfos().Length == 0;
			if (createPlaceHolder)
			{
				var placeHolderFile = new FileInfo(Path.Combine(dirInfo.FullName, PlaceHolderName));

				try
				{
					if (Verbose)
						Console.Out.Write("CREATING PLACEHOLDER: '" + placeHolderFile.FullName + "'...");
					using (var fileStream = placeHolderFile.Create())
					{
						byte[] byteData = Encoding.ASCII.GetBytes(PlaceHolderText);
						fileStream.Write(byteData, 0, byteData.Length);
					}
					if (Verbose)
						Console.Out.WriteLine("FINISHED!");
				}
				catch (Exception ex)
				{
					if (Verbose)
						Console.Out.WriteLine("FAILED! (" + ex.Message + ")");
					else
						Console.Error.WriteLine("CREATING PLACEHOLDER '" + placeHolderFile.FullName + "' FAILED! (" + ex.Message + ")");
				}
			}
			
			return true;
		}

		public bool Visit(FileInfo fileInfo)
		{
			if (fileInfo.Name == PlaceHolderName)
			{
				var dirInfo = fileInfo.Directory;
				
				var deletePlaceHolder = dirInfo.GetFileSystemInfos().Length > 1;
				if (deletePlaceHolder)
					fileInfo.Delete();
			}
			
			return true;
		}
	}
}