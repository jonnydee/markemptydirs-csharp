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

using DJ.Util;

namespace DJ.Util.Template
{
    
    public class EnvironmentVariable : TemplateVariable
    {
        public EnvironmentVariable() : base("env")
        {
        }

        public override string EvaluateValueFor(TemplateEngine ctx, string arg)
        {
            try
            {
                var envName = arg.Split(new[] { ' ' }, 1)[0].Trim();
                return Environment.GetEnvironmentVariable(envName);
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.LogType.Warn, ex);
                return arg;
            }
        }

        public override string Description
        {
            get { return "get the value from an environment variable"; }
        }

        public override bool CanHaveArgument
        {
            get { return true; }
        }

        public override string ArgumentDescription
        {
            get { return "the environment variable's name"; }
        }

        public override string ArgumentIdentifier
        {
            get { return "env-var-name"; }
        }

        public override bool ArgumentMandatory
        {
            get { return true; }
        }
    }
    
}
