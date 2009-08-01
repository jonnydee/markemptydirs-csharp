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
using System.Text;
using System.Text.RegularExpressions;

namespace DJ.Util.Template
{
    public class TemplateEngine
    {
        private string _source;
        private Dictionary<string, TemplateVariable> _variableMap;
        
        public TemplateEngine(String source)
        {
            _source = source;
            _variableMap = new Dictionary<string, TemplateVariable>();

            AddVariable(new DateTimeVariable());
            AddVariable(new EnvironmentVariable());
            AddVariable(new GuidVariable());
            AddVariable(new LineFeedVariable());
            AddVariable(new SpaceVariable());
        }

        public void AddVariable(TemplateVariable variable)
        {
            _variableMap[variable.Name] = variable;
        }

        public string GetVariableValue(string name, string args)
        {
            if (_variableMap.ContainsKey(name))
                return _variableMap[name].GetValueFor(this, args);
            return string.Empty;
        }

        public List<TemplateVariable> ListTemplateVariables()
        {
            var keys = new List<string>();
            foreach (var key in _variableMap.Keys)
                keys.Add(key);
            keys.Sort();

            var variables = new List<TemplateVariable>();
            foreach (var key in keys)
                variables.Add(_variableMap[key]);
            
            return variables;
        }
        
        override public string ToString()
        {
            return ApplyTemplateVariables(_source);
        }

        protected string ApplyTemplateVariables(string str)
        {
            Match match;
            while ((match = Regex.Match(str, TemplateVariable.Pattern)).Success)
            {
                var variableName = match.Groups["Name"].ToString();
                // TODO Use regexp to spilt args.
                var args = match.Groups["Args"].ToString();
                var variableValue = GetVariableValue(variableName, args);
                str = str.Replace(match.Groups["Variable"].ToString(), variableValue);
            }
            return str;
        }

    }
}
