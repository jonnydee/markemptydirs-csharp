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

using DJ.Util.IO;

namespace DJ.App.MarkEmptyDirs
{
    
    class HelpCommand : ICommand
    {
        public HelpCommand()
        {
        }

        public TextWriter Writer { set; get; }
        
        static string[] GetCommandFileName()
        {
            try
            {
                var cmdFile = new FileInfo(Environment.GetCommandLineArgs()[0]);
                var cmdExtension = cmdFile.Extension;
                var cmdName = cmdFile.Name.Substring(0, cmdFile.Name.Length - cmdExtension.Length);
                return new[] { cmdFile.Name, cmdName, cmdExtension };
            }
            catch
            {
                return new string[] { MainClass.StandardCmdName + "." + MainClass.StandardCmdExtension, MainClass.StandardCmdName, MainClass.StandardCmdExtension };
            }
        }

        void PrintUsage()
        {
            var cmdFileName = GetCommandFileName();
            var cmdFullName = cmdFileName[0];
            var cmdName = cmdFileName[1];
            
            Writer.WriteLine();
            Writer.WriteLine("***");
            Writer.WriteLine("*** " + cmdName + " " + MainClass.Version + " -- " + MainClass.Copyright);
            Writer.WriteLine("***");
            Writer.WriteLine("*** Project Site: " + MainClass.ProjectUrl);
            Writer.WriteLine("***");
            Writer.WriteLine("*** This program is licensed under the GNU General Public License, Version 3.");
            Writer.WriteLine("***");
            Writer.WriteLine();
            Writer.WriteLine("USAGE: " + cmdFullName + " [--verbose] [--short] [--dry-run] [--clean] [--list] [--exclude=<list-of-dirnames>] [--place-holder=<filename>] [--text=<placeholder-text>] <directory>\n");
        }

        public void Execute(List<Option> options)
        {
            PrintUsage();
        }
    }

}
