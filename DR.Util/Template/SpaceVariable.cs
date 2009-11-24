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

namespace DJ.Util.Template
{
    
    public class SpaceVariable : TemplateVariable
    {
        public const string Id = "sp";

        public const string ArgCount = "count";

                
        public SpaceVariable() : base(Id)
        {
        }

        public override string EvaluateValueFor(TemplateEngine ctx, string arg)
        {
            int count = 1;
            try
            {
                count = Convert.ToInt32(arg);
            }
            catch
            {
            }
            return new string(' ', count);
        }

        public override string Description
        {
            get { return "get a space character"; }
        }

        public override bool CanHaveArgument
        {
            get { return true; }
        }

        public override string ArgumentDescription
        {
            get { return string.Format("{0} : integer describing how many spaces should be returned", ArgCount); }
        }

        public override string ArgumentIdentifier
        {
            get { return ArgCount; }
        }

        public override bool ArgumentMandatory
        {
            get { return false; }
        }
    }
}
