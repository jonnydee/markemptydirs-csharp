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

namespace DJ.Util.IO
{

    public class OptionParser
    {
        private readonly ICollection<OptionDescriptor> _optionDescriptors;
        
        public OptionParser(params OptionDescriptor[] optionDescriptors)
            : this((ICollection<OptionDescriptor>)optionDescriptors)
        {
        }

        public OptionParser(ICollection<OptionDescriptor> optionDescriptors)
        {
            _optionDescriptors = optionDescriptors;
        }

        public bool StopWhenNoOptionDescriptor { set; get; }

        public int Index { private set; get; }
        
        Dictionary<string,OptionDescriptor> CreateLongNameDescriptorMap()
        {
            var map = new Dictionary<string,OptionDescriptor>();
            
            foreach (var optionDescr in _optionDescriptors)
            {
                if (null == optionDescr.LongNames)
                    continue;
                
                foreach (var longName in optionDescr.LongNames)
                    map[longName] = optionDescr;
            }

            return map;
        }
        
        Dictionary<char,OptionDescriptor> CreateShortNameDescriptorMap()
        {
            var map = new Dictionary<char,OptionDescriptor>();
            
            foreach (var optionDescr in _optionDescriptors)
            {
                if (null == optionDescr.ShortNames)
                    continue;
                
                foreach (var shortName in optionDescr.ShortNames)
                    map[shortName] = optionDescr;
            }

            return map;
        }

        public List<Option> ParseOptions(string[] args)
        {
            return ParseOptions(args, 0, args.Length);
        }
        
        public List<Option> ParseOptions(string[] args, int startIndex)
        {
            return ParseOptions(args, startIndex, args.Length);
        }
        
        /// <summary>
        /// Starts parsing options starting at argument with index <code>startIndex</code>.
        /// </summary>
        /// <param name="args">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="startIndex">
        /// A <see cref="System.Int32"/>
        /// </param>
        /// <returns>
        /// A <see cref="List"/> containing all options.
        /// </returns>
        public List<Option> ParseOptions(string[] args, int startIndex, int endIndex)
        {
            var longNameDescriptorMap = CreateLongNameDescriptorMap();
            var shortNameDescriptorMap = CreateShortNameDescriptorMap();
            
            var options = new List<Option>();

            for (Index = startIndex; Index < endIndex; Index++)
            {
                string arg = args[Index];

                // If we encounter a standalone '--' return all remaining
                // args as non-interpreted options.
                if (arg == "--")
                {
                    while (++Index < endIndex)
                    {
                        options.Add(new Option { OptionType = OptionType.NoOption, Value = args[Index] });
                    }
                    break;
                }
                    
                if (arg.StartsWith("--"))
                {
                    // Parse long option
                    var keyValueString = arg.Substring(2);
                    var parts = keyValueString.Split(new[] { '=' }, 2);
                    var longName = parts[0];
                    var value = parts.Length > 1 ? parts[1] : null;
                    // If no option with that name is known add a descriptor-less option and continue,
                    // or stop, if we need to.
                    if (!longNameDescriptorMap.ContainsKey(longName))
                    {
                        if (!StopWhenNoOptionDescriptor)
                        {
                            options.Add(new Option { OptionType = OptionType.Long, Name = longName, Value = value });
                            continue;
                        }
                        // We need to stop here and return.
                        return options;
                    }
                    // Get the option descriptor.
                    var optDescr = longNameDescriptorMap[longName];
                    // Make sure a value is only provided vor options that can have values.
                    if (!optDescr.CanHaveValue && parts.Length > 1)
                        throw new Exception(string.Format("Option '{0}' must not have a value", longName));
                    // Make sure a mandatory value is provided.
                    if (optDescr.MandatoryValue && parts.Length == 1)
                    {
                        throw new Exception(string.Format("Missing value for option '{0}'", longName));
                    }
                    // Return option.
                    options.Add(new Option { OptionType = OptionType.Long, Name = longName, Value = value, Descriptor = optDescr });
                    continue;
                }

                if (arg.StartsWith("-"))
                {
                    string opts = arg.Substring(1);
                    // Parse short options
                    for (int j = 0; j < opts.Length; j++)
                    {
                        char shortName = opts[j];
                        // If no option with that name is known add a descriptor-less option and continue,
                        // or stop, if we need to.
                        if (!shortNameDescriptorMap.ContainsKey(shortName))
                        {
                            if (!StopWhenNoOptionDescriptor)
                            {
                                options.Add(new Option { OptionType = OptionType.Short, Name = shortName.ToString() });
                                continue;
                            }
                            // We need to stop here and return.
                            return options;
                        }
                        // Get option descriptor.
                        var optDescr = shortNameDescriptorMap[shortName];
                        // If option cannot have a value we can return that option.
                        if (!optDescr.CanHaveValue)
                        {
                            options.Add(new Option { OptionType = OptionType.Short, Name = shortName.ToString(), Descriptor = optDescr });
                            continue;
                        }
                        // If a value for option is not required the rest of arg (if existent) is
                        // the option's value.
                        if (!optDescr.MandatoryValue)
                        {
                            var value = j < opts.Length - 1 ? opts.Substring(j + 1) : null;
                            options.Add(new Option { OptionType = OptionType.Short, Name = shortName.ToString(), Value = value, Descriptor = optDescr });
                            break;
                        }
                        // The option's value is mandatory.
                        else
                        {
                            // If existent, take the rest of arg as the option's value.
                            if (j < opts.Length - 1)
                            {
                                var value = opts.Substring(j + 1);
                                options.Add(new Option { OptionType = OptionType.Short, Name = shortName.ToString(), Value = value, Descriptor = optDescr });
                                break;
                            }

                            // The mandatory value was not provided in current arg,
                            // so use the next arg as the required value.
                            if (Index < endIndex - 1)
                            {
                                var value = args[++Index];
                                options.Add(new Option { OptionType = OptionType.Short, Name = shortName.ToString(), Value = value, Descriptor = optDescr });
                                break;
                            }

                            // There is no further argument in the args list, so the required
                            // value is not provided on command line.
                            throw new Exception(string.Format("Missing value for option '{0}'", shortName));
                        }
                    }

                    continue;
                }

                // If no option is recognized add a descriptor-less option and continue,
                // or stop, if we need to.
                if (!StopWhenNoOptionDescriptor)
                {
                    options.Add(new Option { OptionType = OptionType.NoOption, Value = arg });
                    continue;
                }
                // We need to stop here.
                return options;
            }

            return options;
        }
        
    }
}
