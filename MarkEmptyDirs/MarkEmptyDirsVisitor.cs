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
using System.Collections.Generic;
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
            Exclude = new List<string> {".bzr", ".cvs", ".hg", ".svn"};
		}
		
		public string PlaceHolderName { set; get; }
		
		public string PlaceHolderText { set; get; }
		
		public bool RemoveOnly { set; get; }

		public bool Verbose { set; get; }

        public bool DryRun { set; get; }

        public List<string> Exclude { set; get; }

		public bool PreVisit(DirectoryInfo dirInfo)
		{
			return !Exclude.Contains(dirInfo.Name);
		}
		
		public bool PostVisit(DirectoryInfo dirInfo)
		{
			if (RemoveOnly)
				return true;
			
			var createPlaceHolder = dirInfo.GetFileSystemInfos().Length == 0;
			if (createPlaceHolder)
			{
				var placeHolderFile = new FileInfo(Path.Combine(dirInfo.FullName, PlaceHolderName));

				try
				{
					if (Verbose)
						Console.Out.Write("CREATING PLACEHOLDER: '" + placeHolderFile.FullName + "'...");
                    if (!DryRun)
                    {
                        using (var fileStream = placeHolderFile.Create())
                        {
                            var byteData = Encoding.ASCII.GetBytes(PlaceHolderText);
                            fileStream.Write(byteData, 0, byteData.Length);
                        }
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
				var deletePlaceHolder = RemoveOnly || fileInfo.Directory.GetFileSystemInfos().Length > 1;
				if (deletePlaceHolder)
				{
					try
					{
						if (Verbose)
							Console.Out.Write("DELETING PLACEHOLDER: '" + fileInfo.FullName + "'...");
                        if (!DryRun)
						    fileInfo.Delete();
						if (Verbose)
							Console.Out.WriteLine("FINISHED!");
					}
					catch (Exception ex)
					{
						if (Verbose)
							Console.Out.WriteLine("FAILED! (" + ex.Message + ")");
						else
							Console.Error.WriteLine("DELETING PLACEHOLDER '" + fileInfo.FullName + "' FAILED! (" + ex.Message + ")");
					}
				}
			}
			
			return true;
		}
	}
}