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

using NUnit.Framework;

namespace DJ.Util.IO
{
    [TestFixture]
    public class TestOptionParser
    {
        List<OptionDescriptor> GetOptionDescriptors()
        {
            return new List<OptionDescriptor>
            {
                OptionDescriptorDefinitions.LongFormOnlyOptionDescriptor,
                OptionDescriptorDefinitions.LongFormOnlyAliasOptionDescriptor,
                OptionDescriptorDefinitions.NormalOptionDescriptor,
                OptionDescriptorDefinitions.OptionalValueOptionDescriptor,
                OptionDescriptorDefinitions.MandatoryValueOptionDescriptor,
            };
        }

        string[] AddNoise(params string[] args)
        {
            var prefix = new[] { "prefix-no-option", "--prefix-unknown-option" };
            var postfix = new[] { "postfix-no-option", "--post-unknown-option" };
            var newArgs = new string[prefix.Length + args.Length + postfix.Length];
            prefix.CopyTo(newArgs, 0);
            args.CopyTo(newArgs, prefix.Length);
            postfix.CopyTo(newArgs, prefix.Length + args.Length);
            return newArgs;
        }
        
        [Test]
        public void TestParseLongFormOnlyOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.LongFormOnlyOptionDescriptor;
            var args = new[] { "--" + optionDescriptor.LongNames[0] };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(options.Count, 1);
            Assert.AreSame(options[0].Descriptor, optionDescriptor);
        }

        [Test]
        public void TestParseLongFormOnlyAliasOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.LongFormOnlyAliasOptionDescriptor;
            var args = new[] { "--" + optionDescriptor.LongNames[0], "--" + optionDescriptor.LongNames[1] };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(options.Count, 2);
            Assert.AreSame(options[0].Descriptor, optionDescriptor);
            Assert.AreSame(options[1].Descriptor, optionDescriptor);
        }

        
        public static class OptionDescriptorDefinitions
        {
            public static readonly OptionDescriptor LongFormOnlyOptionDescriptor = new OptionDescriptor
            {
                LongNames = new[] { "long-form-only-option" },
                ShortDescription = "option with only long form and no arg",
            };
            public static readonly OptionDescriptor LongFormOnlyAliasOptionDescriptor = new OptionDescriptor
            {
                LongNames = new[] { "long-form-only-option", "long-form-only-option-alias" },
                ShortDescription = "option with only long form and alias and no arg",
            };
            public static readonly OptionDescriptor ShortFormOnlyOptionDescriptor = new OptionDescriptor
            {
                ShortNames = new[] { 's' },
                ShortDescription = "option with only short form and no arg",
            };
            public static readonly OptionDescriptor ShortFormOnlyAliasOptionDescriptor = new OptionDescriptor
            {
                ShortNames = new[] { 'a', 'A' },
                ShortDescription = "option with only short form and alias and no arg",
            };
            public static readonly OptionDescriptor NormalOptionDescriptor = new OptionDescriptor
            {
                LongNames = new[] { "normal-option", "normal-option-alias" },
                ShortNames = new[] { 'n', 'N' },
                ShortDescription = "normal option with long and short form and aliases and no arg",
            };
            public static readonly OptionDescriptor OptionalValueOptionDescriptor = new OptionDescriptor
            {
                LongNames = new[] { "optional-value-option" },
                ShortNames = new[] { 'o' },
                ShortDescription = "option with optional value",
                CanHaveValue = true,
                ValueIdentifier = "optional-value",
            };
            public static readonly OptionDescriptor MandatoryValueOptionDescriptor = new OptionDescriptor
            {
                LongNames = new[] { "mandatory-value-option" },
                ShortNames = new[] { 'm' },
                ShortDescription = "option with mandatory value",
                CanHaveValue = true,
                MandatoryValue = true,
                ValueIdentifier = "mandatory-value",
            };    
        }        
    }
}
