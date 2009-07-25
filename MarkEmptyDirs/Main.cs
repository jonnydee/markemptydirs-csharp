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

    class MainClass
    {
        public const string StandardCmdName = "MarkEmptyDirs";
        public const string StandardCmdExtension = "exe";
        public const string Version = "V1.2dev";
        public const string Copyright = "Copyright (c) 2009 by Johann Duscher (alias Jonny Dee)";
        public const string ProjectUrl = "http://code.google.com/p/markemptydirs";


        public static void Main(string[] args)
        {
            var optionParser = new OptionParser(OptionDescriptorDefinitions.OptionDescriptors);
            var options = optionParser.ParseOptions(args);

            ICommand cmd;

            if (null != Option.FindFirstByDescriptor(OptionDescriptorDefinitions.HelpOptionDescriptor, options))
                cmd = new HelpCommand() { Writer = Console.Out };
            else
                cmd = new MarkEmptyDirsVisitor();

            try
            {
                cmd.Execute(options);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}
