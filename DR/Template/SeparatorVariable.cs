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

namespace DR.Template
{
    public class SeparatorVariable : TemplateVariable
    {   
        public const string Id = "separator";

        public const string ArgDirectory = "dir";
        public const string ArgPath = "path";
        public const string ArgVolume = "vol";

        
        public SeparatorVariable() : base(Id)
        {
        }

        public override string EvaluateValueFor(TemplateEngine ctx, string arg)
        {
            switch (arg.ToLower())
            {
                case ArgDirectory:
                    return Path.DirectorySeparatorChar.ToString();
                case ArgPath:
                    return Path.PathSeparator.ToString();
                case ArgVolume:
                    return Path.VolumeSeparatorChar.ToString();
                default:
                    throw new ArgumentOutOfRangeException(Id, arg, "Unknown argument");
            }
        }

        public override string Description
        {
            get { return "get platform specific directory, path, or volume separator"; }
        }

        public override bool CanHaveArgument
        {
            get { return true; }
        }

        public override string ArgumentDescription
        {
            get { return string.Format("{0} : directory level separator\n{1} : path separator\n{2} : volume separator", ArgDirectory, ArgPath, ArgVolume); }
        }

        public override string ArgumentIdentifier
        {
            get { return string.Format("{0}|{1}|{2}", ArgDirectory, ArgPath, ArgVolume); }
        }

        public override bool ArgumentMandatory
        {
            get { return true; }
        }
    }
}
