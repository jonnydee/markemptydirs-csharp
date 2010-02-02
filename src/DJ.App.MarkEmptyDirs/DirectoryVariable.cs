//  Copyright (C) 2009-2010 by Johann Duscher (alias Jonny Dee)
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

using DR.Template;

namespace DJ.App.MarkEmptyDirs
{
    public class DirectoryVariable : TemplateVariable
    {   
        public const string Id = "dir";

        public const string ArgBase = "base";
        public const string ArgCurrentAbsolute = "cur.abs";
        public const string ArgCurrentRelative = "cur.rel";
        public const string DefaultArg = ArgBase;

        public const string ContextBaseDir = Id + "." + ArgBase;
        public const string ContextCurrentDir = Id + "." + ArgCurrentAbsolute;

        
        public DirectoryVariable() : base(Id)
        {
        }

        public override string EvaluateValueFor(TemplateEngine ctx, string arg)
        {
            if (null == ctx.DynamicContext)
                throw new ArgumentNullException("DynamicContext");

            if (string.IsNullOrEmpty(arg))
                arg = DefaultArg;

            string dir = null;
            switch (arg.ToLower())
            {
                case ArgBase:
                    {
                        var dirInfo = (DirectoryInfo)ctx.DynamicContext[ContextBaseDir];
                        dir = dirInfo.FullName;
                    }
                    break;
                case ArgCurrentAbsolute:
                    {
                        var dirInfo = (DirectoryInfo)ctx.DynamicContext[ContextCurrentDir];
                        dir = dirInfo.FullName;
                    }
                    break;
                case ArgCurrentRelative:
                    {
                        var baseDirInfo = (DirectoryInfo)ctx.DynamicContext[ContextBaseDir];
                        var currentDirInfo = (DirectoryInfo)ctx.DynamicContext[ContextCurrentDir];
                        dir = currentDirInfo.FullName.Substring(baseDirInfo.FullName.Length);
                        if (dir[0] == Path.DirectorySeparatorChar)
                            dir = dir.Substring(1);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(Id, arg, "Unknown argument");
            }

            return dir;
        }

        public override string Description
        {
            get { return string.Format("get the base directory or current directory (default is '{0}')", DefaultArg); }
        }

        public override bool CanHaveArgument
        {
            get { return true; }
        }

        public override string ArgumentDescription
        {
            get { return string.Format("{0} : base directory\n{1} : absolute path of current directory\n{2} : relative path to current directory", ArgBase, ArgCurrentAbsolute, ArgCurrentRelative); }
        }

        public override string ArgumentIdentifier
        {
            get { return string.Format("{0}|{1}|{2}", ArgBase, ArgCurrentAbsolute, ArgCurrentRelative); }
        }

        public override bool ArgumentMandatory
        {
            get { return false; }
        }
    }
}
