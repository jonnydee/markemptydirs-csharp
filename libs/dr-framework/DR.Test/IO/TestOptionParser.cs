//  Copyright (C) 2009-2010 by Johann Duscher (alias Jonny Dee)
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
using NUnit.Framework;

namespace DR.IO
{
    [TestFixture]
    public class TestOptionParser
    {
        [Test]
        public void TestParseLongFormOnlyOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.LongFormOnlyOptionDescriptor;
            var args = new[] { "--" + optionDescriptor.LongNames[0] };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(1, options.Count);
            
            Assert.AreSame(optionDescriptor, options[0].Descriptor);
            Assert.AreEqual(optionDescriptor.LongNames[0], options[0].Name);
            Assert.AreEqual(OptionType.Long, options[0].OptionType);
        }

        [Test]
        public void TestParseLongFormOnlyAliasOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.LongFormOnlyAliasOptionDescriptor;
            var args = new[] { "--" + optionDescriptor.LongNames[0], "--" + optionDescriptor.LongNames[1] };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(2, options.Count);
            
            Assert.AreSame(optionDescriptor, options[0].Descriptor);
            Assert.AreEqual(optionDescriptor.LongNames[0], options[0].Name);
            Assert.AreEqual(OptionType.Long, options[0].OptionType);
            
            Assert.AreSame(optionDescriptor, options[1].Descriptor);
            Assert.AreEqual(optionDescriptor.LongNames[1], options[1].Name);
            Assert.AreEqual(OptionType.Long, options[1].OptionType);
        }

        [Test]
        public void TestParseShortFormOnlyOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.ShortFormOnlyOptionDescriptor;
            var args = new[] { "-" + optionDescriptor.ShortNames[0] };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(1, options.Count);
            
            Assert.AreSame(optionDescriptor, options[0].Descriptor);
            Assert.AreEqual(optionDescriptor.ShortNames[0].ToString(), options[0].Name);
            Assert.AreEqual(OptionType.Short, options[0].OptionType);
        }

        [Test]
        public void TestParseShortFormOnlyAliasOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.ShortFormOnlyAliasOptionDescriptor;
            var args = new[] { "-" + optionDescriptor.ShortNames[0], "-" + optionDescriptor.ShortNames[1] };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(2, options.Count);

            Assert.AreSame(optionDescriptor, options[0].Descriptor);
            Assert.AreEqual(optionDescriptor.ShortNames[0].ToString(), options[0].Name);
            Assert.AreEqual(OptionType.Short, options[0].OptionType);

            Assert.AreSame(optionDescriptor, options[1].Descriptor);
            Assert.AreEqual(optionDescriptor.ShortNames[1].ToString(), options[1].Name);
            Assert.AreEqual(OptionType.Short, options[1].OptionType);
        }

        [Test]
        public void TestParseNormalOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.NormalOptionDescriptor;
            var args = new[]
            {
                "-" + optionDescriptor.ShortNames[0],
                "--" + optionDescriptor.LongNames[0],
                "-" + optionDescriptor.ShortNames[1],
                "--" + optionDescriptor.LongNames[1],
            };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(4, options.Count);

            Assert.AreSame(optionDescriptor, options[0].Descriptor);
            Assert.AreEqual(optionDescriptor.ShortNames[0].ToString(), options[0].Name);
            Assert.AreEqual(OptionType.Short, options[0].OptionType);

            Assert.AreSame(optionDescriptor, options[1].Descriptor);
            Assert.AreEqual(optionDescriptor.LongNames[0], options[1].Name);
            Assert.AreEqual(OptionType.Long, options[1].OptionType);

            Assert.AreSame(optionDescriptor, options[2].Descriptor);
            Assert.AreEqual(optionDescriptor.ShortNames[1].ToString(), options[2].Name);
            Assert.AreEqual(OptionType.Short, options[2].OptionType);

            Assert.AreSame(optionDescriptor, options[3].Descriptor);
            Assert.AreEqual(optionDescriptor.LongNames[1], options[3].Name);
            Assert.AreEqual(OptionType.Long, options[3].OptionType);
        }

        [Test]
        public void TestParseOptionalValueLongFormOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.OptionalValueOptionDescriptor;
            var optionalValue = "optionalValue";
            var args = new[]
            {
                "--" + optionDescriptor.LongNames[0],
                optionalValue,
                "--" + optionDescriptor.LongNames[0] + "=",
                optionalValue,
                "--" + optionDescriptor.LongNames[0] + "=" + optionalValue,
                optionalValue,
            };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(6, options.Count);
            
            Assert.AreSame(optionDescriptor, options[0].Descriptor);
            Assert.IsNull(options[0].Value);
            Assert.AreEqual(optionDescriptor.LongNames[0], options[0].Name);
            Assert.AreEqual(OptionType.Long, options[0].OptionType);

            Assert.IsNull(options[1].Descriptor);
            Assert.AreEqual(optionalValue, options[1].Value);
            Assert.IsNull(options[1].Name);
            Assert.AreEqual(OptionType.NoOption, options[1].OptionType);

            Assert.AreSame(optionDescriptor, options[2].Descriptor);
            Assert.IsEmpty(options[2].Value);
            Assert.AreEqual(optionDescriptor.LongNames[0], options[2].Name);
            Assert.AreEqual(OptionType.Long, options[2].OptionType);

            Assert.IsNull(options[3].Descriptor);
            Assert.AreEqual(optionalValue, options[3].Value);
            Assert.IsNull(options[3].Name);
            Assert.AreEqual(OptionType.NoOption, options[3].OptionType);

            Assert.AreSame(optionDescriptor, options[4].Descriptor);
            Assert.AreEqual(optionalValue, options[4].Value);
            Assert.AreEqual(optionDescriptor.LongNames[0], options[4].Name);
            Assert.AreEqual(OptionType.Long, options[4].OptionType);

            Assert.IsNull(options[5].Descriptor);
            Assert.AreEqual(optionalValue, options[5].Value);
            Assert.IsNull(options[5].Name);
            Assert.AreEqual(OptionType.NoOption, options[5].OptionType);
        }

        [Test]
        public void TestParseOptionalValueShortFormOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.OptionalValueOptionDescriptor;
            var optionalValue = "optionalValue";
            var args = new[]
            {
                "-" + optionDescriptor.ShortNames[0],
                optionalValue,
                "-" + optionDescriptor.ShortNames[0] + optionalValue,
                optionalValue,
            };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(4, options.Count);
            
            Assert.AreSame(optionDescriptor, options[0].Descriptor);
            Assert.IsNull(options[0].Value);
            Assert.AreEqual(optionDescriptor.ShortNames[0].ToString(), options[0].Name);
            Assert.AreEqual(OptionType.Short, options[0].OptionType);
            
            Assert.IsNull(options[1].Descriptor);
            Assert.AreEqual(optionalValue, options[1].Value);
            Assert.IsNull(options[1].Name);
            Assert.AreEqual(OptionType.NoOption, options[1].OptionType);

            Assert.AreSame(optionDescriptor, options[2].Descriptor);
            Assert.AreEqual(optionalValue, options[2].Value);
            Assert.AreEqual(optionDescriptor.ShortNames[0].ToString(), options[2].Name);
            Assert.AreEqual(OptionType.Short, options[2].OptionType);

            Assert.IsNull(options[3].Descriptor);
            Assert.AreEqual(optionalValue, options[3].Value);
            Assert.IsNull(options[3].Name);
            Assert.AreEqual(OptionType.NoOption, options[3].OptionType);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestParseMandatoryValueMissingLongFormOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.MandatoryValueOptionDescriptor;
            var mandatoryValue = "mandatoryValue";
            var args = new[]
            {
                "--" + optionDescriptor.LongNames[0],
                mandatoryValue,
            };

            var parser = new OptionParser(optionDescriptor);
            parser.ParseOptions(args);
        }

        [Test]
        public void TestParseMandatoryValueLongFormOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.MandatoryValueOptionDescriptor;
            var mandatoryValue = "mandatoryValue";
            var args = new[]
            {
                "--" + optionDescriptor.LongNames[0] + "=",
                mandatoryValue,
                "--" + optionDescriptor.LongNames[0] + "=" + mandatoryValue,
                mandatoryValue,
            };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(4, options.Count);
            
            Assert.AreSame(optionDescriptor, options[0].Descriptor);
            Assert.IsEmpty(options[0].Value);
            Assert.AreEqual(optionDescriptor.LongNames[0], options[0].Name);
            Assert.AreEqual(OptionType.Long, options[0].OptionType);

            Assert.IsNull(options[1].Descriptor);
            Assert.AreEqual(mandatoryValue, options[1].Value);
            Assert.IsNull(options[1].Name);
            Assert.AreEqual(OptionType.NoOption, options[1].OptionType);

            Assert.AreSame(optionDescriptor, options[2].Descriptor);
            Assert.AreEqual(mandatoryValue, options[2].Value);
            Assert.AreEqual(optionDescriptor.LongNames[0], options[2].Name);
            Assert.AreEqual(OptionType.Long, options[2].OptionType);

            Assert.IsNull(options[3].Descriptor);
            Assert.AreEqual(mandatoryValue, options[3].Value);
            Assert.IsNull(options[3].Name);
            Assert.AreEqual(OptionType.NoOption, options[3].OptionType);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestParseMandatoryValueMissingShortFormOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.MandatoryValueOptionDescriptor;
            var args = new[]
            {
                "-" + optionDescriptor.ShortNames[0],
            };

            var parser = new OptionParser(optionDescriptor);
            parser.ParseOptions(args);
        }

        [Test]
        public void TestParseMandatoryValueShortFormOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.MandatoryValueOptionDescriptor;
            var mandatoryValue = "mandatoryValue";
            var args = new[]
            {
                "-" + optionDescriptor.ShortNames[0], mandatoryValue,
                "-" + optionDescriptor.ShortNames[0] + mandatoryValue,
                mandatoryValue,
            };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(3, options.Count);
            
            Assert.AreSame(optionDescriptor, options[0].Descriptor);
            Assert.AreEqual(mandatoryValue, options[0].Value);
            Assert.AreEqual(optionDescriptor.ShortNames[0].ToString(), options[0].Name);
            Assert.AreEqual(OptionType.Short, options[0].OptionType);

            Assert.AreSame(optionDescriptor, options[1].Descriptor);
            Assert.AreEqual(mandatoryValue, options[1].Value);
            Assert.AreEqual(optionDescriptor.ShortNames[0].ToString(), options[1].Name);
            Assert.AreEqual(OptionType.Short, options[1].OptionType);

            Assert.IsNull(options[2].Descriptor);
            Assert.AreEqual(mandatoryValue, options[2].Value);
            Assert.IsNull(options[2].Name);
            Assert.AreEqual(OptionType.NoOption, options[2].OptionType);
        }

        [Test]
        public void TestParseUnknownShortFormOption()
        {
            var optionName = "u";
            var noOption = "no-option";
            var args = new[]
            {
                "-" + optionName,
                noOption,
            };

            var parser = new OptionParser();
            var options = parser.ParseOptions(args);

            Assert.AreEqual(2, options.Count);
            
            Assert.IsNull(options[0].Descriptor);
            Assert.AreEqual(optionName, options[0].Name);
            Assert.IsNull(options[0].Value);
            Assert.AreEqual(OptionType.Short, options[0].OptionType);

            Assert.IsNull(options[1].Descriptor);
            Assert.IsNull(options[1].Name);
            Assert.AreEqual(noOption, options[1].Value);
            Assert.AreEqual(OptionType.NoOption, options[1].OptionType);
        }

        [Test]
        public void TestParseUnknownLongFormOption()
        {
            var optionName = "unknown-option";
            var noOption = "no-option";
            var args = new[]
            {
                "--" + optionName,
                noOption,
                "--" + optionName + "=",
                noOption,
                "--" + optionName + "=" + noOption,
                noOption,
            };

            var parser = new OptionParser();
            var options = parser.ParseOptions(args);

            Assert.AreEqual(6, options.Count);
            
            Assert.IsNull(options[0].Descriptor);
            Assert.AreEqual(optionName, options[0].Name);
            Assert.IsNull(options[0].Value);
            Assert.AreEqual(OptionType.Long, options[0].OptionType);

            Assert.IsNull(options[1].Descriptor);
            Assert.IsNull(options[1].Name);
            Assert.AreEqual(noOption, options[1].Value);
            Assert.AreEqual(OptionType.NoOption, options[1].OptionType);
            
            Assert.IsNull(options[2].Descriptor);
            Assert.AreEqual(optionName, options[2].Name);
            Assert.IsEmpty(options[2].Value);
            Assert.AreEqual(OptionType.Long, options[2].OptionType);

            Assert.IsNull(options[3].Descriptor);
            Assert.IsNull(options[3].Name);
            Assert.AreEqual(noOption, options[3].Value);
            Assert.AreEqual(OptionType.NoOption, options[3].OptionType);
            
            Assert.IsNull(options[4].Descriptor);
            Assert.AreEqual(optionName, options[4].Name);
            Assert.AreEqual(noOption, options[4].Value);
            Assert.AreEqual(OptionType.Long, options[4].OptionType);

            Assert.IsNull(options[5].Descriptor);
            Assert.IsNull(options[5].Name);
            Assert.AreEqual(noOption, options[5].Value);
            Assert.AreEqual(OptionType.NoOption, options[5].OptionType);
        }

        [Test]
        public void TestParseMultipleShortFormOptions()
        {
            var optionDescriptors = new[]
            {
                OptionDescriptorDefinitions.NormalOptionDescriptor,
                OptionDescriptorDefinitions.OptionalValueOptionDescriptor,
            };
            var optionValue = "value";
            var args = new[]
            {
                "-" + optionDescriptors[0].ShortNames[0]
                    + optionDescriptors[1].ShortNames[0] + optionValue,
                optionValue,
            };

            var parser = new OptionParser(optionDescriptors);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(3, options.Count);
            
            Assert.AreSame(optionDescriptors[0], options[0].Descriptor);
            Assert.IsNull(options[0].Value);
            Assert.AreEqual(optionDescriptors[0].ShortNames[0].ToString(), options[0].Name);
            Assert.AreEqual(OptionType.Short, options[0].OptionType);

            Assert.AreSame(optionDescriptors[1], options[1].Descriptor);
            Assert.AreEqual(optionValue, options[1].Value);
            Assert.AreEqual(optionDescriptors[1].ShortNames[0].ToString(), options[1].Name);
            Assert.AreEqual(OptionType.Short, options[1].OptionType);

            Assert.IsNull(options[2].Descriptor);
            Assert.IsNull(options[2].Name);
            Assert.AreEqual(optionValue, options[2].Value);
            Assert.AreEqual(OptionType.NoOption, options[2].OptionType);
         }

        [Test]
        public void TestParseOptionCutter()
        {
            var optionDescriptor = OptionDescriptorDefinitions.MandatoryValueOptionDescriptor;
            var args = new[]
            {
                "--",
                "--some-option=some-value",
                "-o",
                "no-option",
            };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(3, options.Count);
            
            Assert.IsNull(options[0].Descriptor);
            Assert.IsNull(options[0].Name);
            Assert.AreEqual(args[1], options[0].Value);
            Assert.AreEqual(OptionType.NoOption, options[0].OptionType);

            Assert.IsNull(options[1].Descriptor);
            Assert.IsNull(options[1].Name);
            Assert.AreEqual(args[2], options[1].Value);
            Assert.AreEqual(OptionType.NoOption, options[1].OptionType);

            Assert.IsNull(options[2].Descriptor);
            Assert.IsNull(options[2].Name);
            Assert.AreEqual(args[3], options[2].Value);
            Assert.AreEqual(OptionType.NoOption, options[2].OptionType);
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
