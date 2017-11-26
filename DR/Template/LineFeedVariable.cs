//  
//  Copyright (c) 2009-2010 by Johann Duscher (alias Jonny Dee)
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

namespace DR.Template
{
    
    
    public class LineFeedVariable : TemplateVariable
    {
        public const string Id = "lf";

        public const string ArgCount = "count";

        
        public LineFeedVariable() : base(Id)
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
            return new string('\n', count);
        }

        public override string Description
        {
            get { return "get a line feed character"; }
        }

        public override bool CanHaveArgument
        {
            get { return true; }
        }

        public override string ArgumentDescription
        {
            get { return string.Format("{0} : integer describing how many linefeeds should be returned", ArgCount); }
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
