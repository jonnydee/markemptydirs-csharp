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


namespace DJ.Util.Template
{
    public class SeparatorVariable : TemplateVariable
    {   
        public const string Id = "separator";

        public const string ModeDirectory = "dir";
        public const string ModePath = "path";
        public const string ModeVolume = "vol";

        
        public SeparatorVariable() : base(Id)
        {
        }

        public override string EvaluateValueFor(TemplateEngine ctx, string arg)
        {
            switch (arg.ToLower())
            {
                case ModeDirectory:
                    return Path.DirectorySeparatorChar.ToString();
                case ModePath:
                    return Path.PathSeparator.ToString();
                case ModeVolume:
                    return Path.VolumeSeparatorChar.ToString();
                default:
                    throw new ArgumentOutOfRangeException(Id, arg, "Unknown argument");
            }
        }

        public override string Description
        {
            get { return "get platform specific directory, path, and volume separators"; }
        }

        public override bool CanHaveArgument
        {
            get { return true; }
        }

        public override string ArgumentDescription
        {
            get { return string.Format("'{0}' directory level separator, '{1}' path separator, '{2}' volume separator", ModeDirectory, ModePath, ModeVolume); }
        }

        public override string ArgumentIdentifier
        {
            get { return string.Format("{0}|{1}|{2}", ModeDirectory, ModePath, ModeVolume); }
        }

        public override bool ArgumentMandatory
        {
            get { return true; }
        }
    }
}
