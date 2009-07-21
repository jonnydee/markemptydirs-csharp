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

            writer.WriteLine("\n" + cmdName + " " + Version + " -- " + Copyright + "\n");
			writer.WriteLine("USAGE: " + cmdFile.Name + " [--verbose] [--dry-run] [--remove-only] [--place-holder=<filename>] [--text=<\"placeholder text\">] <directory>");
		}
		
		public static void Main(string[] args)
		{
			var visitor = new MarkEmptyDirsVisitor();
			string directory = null;
			
			for (int i = 0; i < args.Length; i++)
			{
				if (IsOption(args[i]))
				{
					string[] keyValuePair = ParseOption(args[i]);
					string key = keyValuePair[0];
					string value = keyValuePair.Length > 1 ? keyValuePair[1] : null;
					
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