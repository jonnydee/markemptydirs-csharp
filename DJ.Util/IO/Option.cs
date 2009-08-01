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

namespace DJ.Util.IO
{
    
    public enum OptionType
    {
        NoOption,
        Short,
        Long
    }
    
    public class Option
    {
        public static Option FindFirstByDescriptor(OptionDescriptor descr, List<Option> options)
        {
            return FindFirstByDescriptor(descr, options, 0);
        }
        
        public static Option FindFirstByDescriptor(OptionDescriptor descr, List<Option> options, int startIndex)
        {
            var foundOpt = FindByDescriptor(descr, options, startIndex);
            return 0 != foundOpt.Count ? foundOpt[0] : null;
        }
        
        public static List<Option> FindByDescriptor(OptionDescriptor descr, List<Option> options)
        {
            return FindByDescriptor(descr, options, 0);
        }
        
        public static List<Option> FindByDescriptor(OptionDescriptor descr, List<Option> options, int startIndex)
        {
            var foundOpts = new List<Option>();
            for (int i = startIndex; i < options.Count; i++)
                if (options[i].Descriptor == descr)
                    foundOpts.Add(options[i]);
            return foundOpts;
        }

        
        public OptionDescriptor Descriptor { set; get; }
        public OptionType OptionType { set; get; }
        public string Name { set; get; }
        public string Value { set; get; }

        public string ToLongFromString()
        {
            var buf = new StringBuilder("--");
            buf.Append(buf);
            if (null != Value)
                buf.Append('=').Append(Value);
            return buf.ToString();
        }
        
        public string ToShortFromString()
        {
            var buf = new StringBuilder("-");
            buf.Append(buf);
            if (null != Value)
                buf.Append(' ').Append(Value);
            return buf.ToString();
        }
        
        public override string ToString ()
        {
            return string.Format("[Option: OptionType={0}, Name={1}, Value={2}, Descriptor={3}]", OptionType, Name, Value, Descriptor);
        }
    }
    
}
