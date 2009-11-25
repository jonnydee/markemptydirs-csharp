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

namespace DR.Template
{
    
    public class GuidVariable : TemplateVariable
    {
        public const string Id = "guid";


        public GuidVariable() : base(Id)
        {
        }
        
        public override string EvaluateValueFor(TemplateEngine ctx, string arg)
        {
            return Guid.NewGuid().ToString();
        }

        public override string Description
        {
            get { return "get a new globally unique identifier"; }
        }

        public override bool CanHaveArgument
        {
            get { return false; }
        }
        
        public override string ArgumentDescription
        {
            get { return null; }
        }

        public override string ArgumentIdentifier
        {
            get { return null; }
        }

        public override bool ArgumentMandatory
        {
            get { return false; }
        }
    }
}
