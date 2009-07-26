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

namespace DJ.Util.IO
{
    
    public class DateTimeVariable : TemplateVariable
    {
        
        public DateTimeVariable() : base("datetime")
        {
        }

        public override string Description {
            get {
                return "Gets the UTC time (accepts C# DateTime format pattern string as argument).";
            }
        }

        public override string ArgumentDescription {
            get {
                return "Optional C# DateTime format pattern string";
            }
        }
        
        public override string GetValueFor (TemplateEngine ctx, string arg)
        {
            var datetime = DateTime.Now.ToUniversalTime();
            return string.IsNullOrEmpty(arg) ? datetime.ToString() : datetime.ToString(arg);
        }

    }
}
