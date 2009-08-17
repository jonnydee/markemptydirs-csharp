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

using DJ.Util.Template;

namespace DJ.App.MarkEmptyDirs
{
    
    public class PlaceHolderVariable : TemplateVariable
    {
        public const string Id = "placeholder";
        
        public const string ArgFullName = "fullname";
        public const string ArgName = "name";
        public const string DefaultArg = ArgName;

        public const string ContextPlaceHolderFile = Id;
        
        
        public PlaceHolderVariable() : base(Id)
        {
        }

        public override string EvaluateValueFor(TemplateEngine ctx, string arg)
        {
            if (null == ctx.DynamicContext)
                throw new ArgumentNullException("DynamicContext");

            if (string.IsNullOrEmpty(arg))
                arg = DefaultArg;
            
            string fileName = null;
            switch (arg.ToLower())
            {
                case ArgName:
                    {
                        var fileInfo = (FileInfo)ctx.DynamicContext[ContextPlaceHolderFile];
                        fileName = fileInfo.Name;
                    }
                    break;
                case ArgFullName:
                    {
                        var fileInfo = (FileInfo)ctx.DynamicContext[ContextPlaceHolderFile];
                        fileName = fileInfo.FullName;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(Id, arg, "Unknown argument");
            }
            return fileName;
        }

        public override string Description
        {
            get { return string.Format("get the placeholder name (default is '{0}')", DefaultArg); }
        }

        public override bool CanHaveArgument
        {
            get { return true; }
        }

        public override string ArgumentDescription
        {
            get { return string.Format("{0} : file name only\n{1} : full file path", ArgName, ArgFullName); }
        }

        public override string ArgumentIdentifier
        {
            get { return string.Format("{0}|{1}", ArgName, ArgFullName); }
        }

        public override bool ArgumentMandatory
        {
            get { return false; }
        }
    }
}
