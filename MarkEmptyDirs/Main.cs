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

using DJ.Util.IO;

namespace DJ.App.MarkEmptyDirs
{
	class MainClass
	{
	    public const string Version = "V1.0";
	    public const string Copyright = "Copyright (c) 2009 by Johann Duscher (alias Jonny Dee)";

		static bool IsOption(string arg)
		{
			return arg.StartsWith("--");
		}
		
		static string[] ParseOption(string arg)
		{
			return arg.Substring(2).Split('=');
		}
		
		static void PrintUsage(TextWriter writer)
		{
            var cmdFile = new FileInfo(Environment.GetCommandLineArgs()[0]);
		    var cmdExtension = cmdFile.Extension;
            var cmdName = cmdFile.Name.Substring(0, cmdFile.Name.Length - cmdExtension.Length);

            writer.WriteLine("\n--- This program is licensed under the GNU GPLv3");
            writer.WriteLine("---");
            writer.WriteLine("--- " + cmdName + " " + Version + " -- " + Copyright + "\n");
			writer.WriteLine("USAGE: " + cmdFile.Name + " [--verbose] [--dry-run] [--remove-only] [--place-holder=<filename>] [--text=<\"placeholder text\">] <directory>");
		}
		
		public static void Main(string[] args)
		{
			var visitor = new MarkEmptyDirsVisitor();
			string directory = null;
			
			for (var i = 0; i < args.Length; i++)
			{
				if (IsOption(args[i]))
				{
					var keyValuePair = ParseOption(args[i]);
					var key = keyValuePair[0];
					var value = keyValuePair.Length > 1 ? keyValuePair[1] : null;
					
					switch (key)
					{
					case "place-holder":
						if (!string.IsNullOrEmpty(value))
							visitor.PlaceHolderName = value;
						break;
					case "text":
						if (!string.IsNullOrEmpty(value))
							visitor.PlaceHolderText = value + '\n';
						break;
					case "verbose":
						visitor.Verbose = true;
						break;
					case "remove-only":
						visitor.RemoveOnly = true;
						break;
                    case "dry-run":
					    visitor.DryRun = true;
					    break;
					case "help":
						PrintUsage(Console.Out);
						return;
					default:
						Console.Error.WriteLine("ERROR: Unknown option: " + key + " (value: '" + value + "')!");
						PrintUsage(Console.Error);
						return;
					}
				} else {
					directory = args[i];
				}
			}
			
            if (null == directory)
            {
                Console.Error.WriteLine("ERROR: No directory specified!");
                return;
            }

			var dirInfo = new DirectoryInfo(directory);
			if (!dirInfo.Exists)
			{
				Console.Error.WriteLine("ERROR: Not a directory: '" + dirInfo.FullName + "'!");
				return;
			}
			
			DirectoryWalker.Walk(dirInfo, visitor);
		}
	}
}