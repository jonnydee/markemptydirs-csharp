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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DR.Template
{
    public class TemplateEngine
    {
        private Dictionary<string, TemplateVariable> _variableMap;
        
        public TemplateEngine(String template)
        {
            Template = template;
            _variableMap = new Dictionary<string, TemplateVariable>();
        }

        public string Template { private set; get; }

        public IDictionary<string, object> DynamicContext { private set; get; }
        
        public void AddVariable(TemplateVariable variable)
        {
            _variableMap[variable.Name] = variable;
        }

        public string EvaluateVariableValue(string name, string args)
        {
            if (_variableMap.ContainsKey(name))
                return _variableMap[name].EvaluateValueFor(this, args);
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
            return ToString(null);
        }

        public string ToString(IDictionary<string, object> dynamicContext)
        {
            try
            {
                DynamicContext = dynamicContext;
                return ApplyTemplateVariables(Template);
            }
            finally
            {
                DynamicContext = null;
            }
        }

        protected string ApplyTemplateVariables(string str)
        {
            Match match;
            while ((match = Regex.Match(str, TemplateVariable.Pattern)).Success)
            {
                var variableName = match.Groups["Name"].ToString();
                // TODO Use regexp to spilt args.
                var args = match.Groups["Args"].ToString();
                var variableValue = EvaluateVariableValue(variableName, args);
                str = str.Replace(match.Groups["Variable"].ToString(), variableValue);
            }
            return str;
        }

    }
}
