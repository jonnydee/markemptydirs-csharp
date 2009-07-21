//  Copyright (C) 2009 by Johann Duscher (alias Jonny Dee)
//
//  This file is part of MarkEmptyDirs.
//
//  MarkEmptyDirs is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  MarkEmptyDirs is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with MarkEmptyDirs.  If not, see <http://www.gnu.org/licenses/>.

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
		    DryRun = false;
			CleanUp = false;
		}
		
		public string PlaceHolderName { set; get; }
		
		public string PlaceHolderText { set; get; }
		
		public bool CleanUp { set; get; }

		public bool Verbose { set; get; }

        public bool DryRun { set; get; }

		public bool PreVisit(DirectoryInfo dirInfo)
		{
			return true;
		}
		
		public bool PostVisit(DirectoryInfo dirInfo)
		{
			if (CleanUp)
				return true;
			
			var createPlaceHolder = dirInfo.GetFileSystemInfos().Length == 0;
			if (createPlaceHolder)
			{
				var placeHolderFile = new FileInfo(Path.Combine(dirInfo.FullName, PlaceHolderName));

				try
				{
                    if (!DryRun)
                    {
                        using (var fileStream = placeHolderFile.Create())
                        {
                            var byteData = Encoding.ASCII.GetBytes(PlaceHolderText);
                            fileStream.Write(byteData, 0, byteData.Length);
                        }
                    }
				    if (Verbose)
						Console.Out.WriteLine("Created placeholder: '" + placeHolderFile.FullName + "'");
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine("ERROR: Creation of placeholder '" + placeHolderFile.FullName + "' failed! (" + ex.Message + ")");
				}
			}
			
			return true;
		}

		public bool Visit(FileInfo fileInfo)
		{
			if (fileInfo.Name == PlaceHolderName)
			{
				var deletePlaceHolder = CleanUp || fileInfo.Directory.GetFileSystemInfos().Length > 1;
				if (deletePlaceHolder)
				{
					try
					{
                        if (!DryRun)
						    fileInfo.Delete();
						if (Verbose)
							Console.Out.WriteLine("Deleted placeholder: '" + fileInfo.FullName + "'");
					}
					catch (Exception ex)
					{
						Console.Error.WriteLine("ERROR: Deletion of placeholder '" + fileInfo.FullName + "' failed! (" + ex.Message + ")");
					}
				}
			}
			
			return true;
		}
	}
}