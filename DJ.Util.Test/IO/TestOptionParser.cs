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
        [Test]
        public void TestParseLongFormOnlyOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.LongFormOnlyOptionDescriptor;
            var args = new[] { "--" + optionDescriptor.LongNames[0] };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(options.Count, 1);
            Assert.AreSame(options[0].Descriptor, optionDescriptor);
            Assert.AreEqual(options[0].Name, optionDescriptor.LongNames[0]);
            Assert.AreEqual(options[0].OptionType, OptionType.Long);
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
            Assert.AreEqual(options[0].Name, optionDescriptor.LongNames[0]);
            Assert.AreEqual(options[0].OptionType, OptionType.Long);
            
            Assert.AreSame(options[1].Descriptor, optionDescriptor);
            Assert.AreEqual(options[1].Name, optionDescriptor.LongNames[1]);
            Assert.AreEqual(options[1].OptionType, OptionType.Long);
        }

        [Test]
        public void TestParseShortFormOnlyOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.ShortFormOnlyOptionDescriptor;
            var args = new[] { "-" + optionDescriptor.ShortNames[0] };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(options.Count, 1);
            
            Assert.AreSame(options[0].Descriptor, optionDescriptor);
            Assert.AreEqual(options[0].Name, optionDescriptor.ShortNames[0].ToString());
            Assert.AreEqual(options[0].OptionType, OptionType.Short);
        }

        [Test]
        public void TestParseShortFormOnlyAliasOptionDescriptor()
        {
            var optionDescriptor = OptionDescriptorDefinitions.ShortFormOnlyAliasOptionDescriptor;
            var args = new[] { "-" + optionDescriptor.ShortNames[0], "-" + optionDescriptor.ShortNames[1] };

            var parser = new OptionParser(optionDescriptor);
            var options = parser.ParseOptions(args);

            Assert.AreEqual(options.Count, 2);

            Assert.AreSame(options[0].Descriptor, optionDescriptor);
            Assert.AreEqual(options[0].Name, optionDescriptor.ShortNames[0].ToString());
            Assert.AreEqual(options[0].OptionType, OptionType.Short);

            Assert.AreSame(options[1].Descriptor, optionDescriptor);
            Assert.AreEqual(options[1].Name, optionDescriptor.ShortNames[1].ToString());
            Assert.AreEqual(options[1].OptionType, OptionType.Short);
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

            Assert.AreEqual(options.Count, 4);

            Assert.AreSame(options[0].Descriptor, optionDescriptor);
            Assert.AreEqual(options[0].Name, optionDescriptor.ShortNames[0].ToString());
            Assert.AreEqual(options[0].OptionType, OptionType.Short);

            Assert.AreSame(options[1].Descriptor, optionDescriptor);
            Assert.AreEqual(options[1].Name, optionDescriptor.LongNames[0]);
            Assert.AreEqual(options[1].OptionType, OptionType.Long);

            Assert.AreSame(options[2].Descriptor, optionDescriptor);
            Assert.AreEqual(options[2].Name, optionDescriptor.ShortNames[1].ToString());
            Assert.AreEqual(options[2].OptionType, OptionType.Short);

            Assert.AreSame(options[3].Descriptor, optionDescriptor);
            Assert.AreEqual(options[3].Name, optionDescriptor.LongNames[1]);
            Assert.AreEqual(options[3].OptionType, OptionType.Long);
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

            Assert.AreEqual(options.Count, 6);
            
            Assert.AreSame(options[0].Descriptor, optionDescriptor);
            Assert.IsNull(options[0].Value);
            Assert.AreEqual(options[0].Name, optionDescriptor.LongNames[0]);
            Assert.AreEqual(options[0].OptionType, OptionType.Long);

            Assert.IsNull(options[1].Descriptor);
            Assert.AreEqual(options[1].Value, optionalValue);
            Assert.IsNull(options[1].Name);
            Assert.AreEqual(options[1].OptionType, OptionType.NoOption);

            Assert.AreSame(options[2].Descriptor, optionDescriptor);
            Assert.IsEmpty(options[2].Value);
            Assert.AreEqual(options[2].Name, optionDescriptor.LongNames[0]);
            Assert.AreEqual(options[2].OptionType, OptionType.Long);

            Assert.IsNull(options[3].Descriptor);
            Assert.AreEqual(options[3].Value, optionalValue);
            Assert.IsNull(options[3].Name);
            Assert.AreEqual(options[3].OptionType, OptionType.NoOption);

            Assert.AreSame(options[4].Descriptor, optionDescriptor);
            Assert.AreEqual(options[4].Value, optionalValue);
            Assert.AreEqual(options[4].Name, optionDescriptor.LongNames[0]);
            Assert.AreEqual(options[4].OptionType, OptionType.Long);

            Assert.IsNull(options[5].Descriptor);
            Assert.AreEqual(options[5].Value, optionalValue);
            Assert.IsNull(options[5].Name);
            Assert.AreEqual(options[5].OptionType, OptionType.NoOption);
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

            Assert.AreEqual(options.Count, 4);
            
            Assert.AreSame(options[0].Descriptor, optionDescriptor);
            Assert.IsNull(options[0].Value);
            Assert.AreEqual(options[0].Name, optionDescriptor.ShortNames[0].ToString());
            Assert.AreEqual(options[0].OptionType, OptionType.Short);
            
            Assert.IsNull(options[1].Descriptor);
            Assert.AreEqual(options[1].Value, optionalValue);
            Assert.IsNull(options[1].Name);
            Assert.AreEqual(options[1].OptionType, OptionType.NoOption);

            Assert.AreSame(options[2].Descriptor, optionDescriptor);
            Assert.AreEqual(options[2].Value, optionalValue);
            Assert.AreEqual(options[2].Name, optionDescriptor.ShortNames[0].ToString());
            Assert.AreEqual(options[2].OptionType, OptionType.Short);

            Assert.IsNull(options[3].Descriptor);
            Assert.AreEqual(options[3].Value, optionalValue);
            Assert.IsNull(options[3].Name);
            Assert.AreEqual(options[3].OptionType, OptionType.NoOption);
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

            Assert.AreEqual(options.Count, 4);
            
            Assert.AreSame(options[0].Descriptor, optionDescriptor);
            Assert.IsEmpty(options[0].Value);
            Assert.AreEqual(options[0].Name, optionDescriptor.LongNames[0]);
            Assert.AreEqual(options[0].OptionType, OptionType.Long);

            Assert.IsNull(options[1].Descriptor);
            Assert.AreEqual(options[1].Value, mandatoryValue);
            Assert.IsNull(options[1].Name);
            Assert.AreEqual(options[1].OptionType, OptionType.NoOption);

            Assert.AreSame(options[2].Descriptor, optionDescriptor);
            Assert.AreEqual(options[2].Value, mandatoryValue);
            Assert.AreEqual(options[2].Name, optionDescriptor.LongNames[0]);
            Assert.AreEqual(options[2].OptionType, OptionType.Long);

            Assert.IsNull(options[3].Descriptor);
            Assert.AreEqual(options[3].Value, mandatoryValue);
            Assert.IsNull(options[3].Name);
            Assert.AreEqual(options[3].OptionType, OptionType.NoOption);
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

            Assert.AreEqual(options.Count, 3);
            
            Assert.AreSame(options[0].Descriptor, optionDescriptor);
            Assert.AreEqual(options[0].Value, mandatoryValue);
            Assert.AreEqual(options[0].Name, optionDescriptor.ShortNames[0].ToString());
            Assert.AreEqual(options[0].OptionType, OptionType.Short);

            Assert.AreSame(options[1].Descriptor, optionDescriptor);
            Assert.AreEqual(options[1].Value, mandatoryValue);
            Assert.AreEqual(options[1].Name, optionDescriptor.ShortNames[0].ToString());
            Assert.AreEqual(options[1].OptionType, OptionType.Short);

            Assert.IsNull(options[2].Descriptor);
            Assert.AreEqual(options[2].Value, mandatoryValue);
            Assert.IsNull(options[2].Name);
            Assert.AreEqual(options[2].OptionType, OptionType.NoOption);
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

            Assert.AreEqual(options.Count, 2);
            
            Assert.IsNull(options[0].Descriptor);
            Assert.AreEqual(options[0].Name, optionName);
            Assert.IsNull(options[0].Value);
            Assert.AreEqual(options[0].OptionType, OptionType.Short);

            Assert.IsNull(options[1].Descriptor);
            Assert.IsNull(options[1].Name);
            Assert.AreEqual(options[1].Value, noOption);
            Assert.AreEqual(options[1].OptionType, OptionType.NoOption);
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

            Assert.AreEqual(options.Count, 6);
            
            Assert.IsNull(options[0].Descriptor);
            Assert.AreEqual(options[0].Name, optionName);
            Assert.IsNull(options[0].Value);
            Assert.AreEqual(options[0].OptionType, OptionType.Long);

            Assert.IsNull(options[1].Descriptor);
            Assert.IsNull(options[1].Name);
            Assert.AreEqual(options[1].Value, noOption);
            Assert.AreEqual(options[1].OptionType, OptionType.NoOption);
            
            Assert.IsNull(options[2].Descriptor);
            Assert.AreEqual(options[2].Name, optionName);
            Assert.IsEmpty(options[2].Value);
            Assert.AreEqual(options[2].OptionType, OptionType.Long);

            Assert.IsNull(options[3].Descriptor);
            Assert.IsNull(options[3].Name);
            Assert.AreEqual(options[3].Value, noOption);
            Assert.AreEqual(options[3].OptionType, OptionType.NoOption);
            
            Assert.IsNull(options[4].Descriptor);
            Assert.AreEqual(options[4].Name, optionName);
            Assert.AreEqual(options[4].Value, noOption);
            Assert.AreEqual(options[4].OptionType, OptionType.Long);

            Assert.IsNull(options[5].Descriptor);
            Assert.IsNull(options[5].Name);
            Assert.AreEqual(options[5].Value, noOption);
            Assert.AreEqual(options[5].OptionType, OptionType.NoOption);
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
