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
using System.IO;
using NUnit.Framework;

namespace DJ.Util.Template
{
    [TestFixture]
    public class TestTemplateEngine
    {
        // TODO Write test for DateTimeVariable with C# pattern string.

        [Test]
        public void TestDateTimeVariableSubstitution()
        {
            var variable = new DateTimeVariable();
            var template = variable.ToString(null);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var str = engine.ToString();
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));
            
            DateTime.Parse(str);
        }

        [Test]
        public void TestEnvironmentVariableSubstitution()
        {
            #region SetUp
            // Create environment variable.
            var envVarName = "TMP_ENV_VAR_NAME";
            var envVarValue = "dummy-value";
            Environment.SetEnvironmentVariable(envVarName, envVarValue);
            #endregion
            
            var variable = new EnvironmentVariable();
            var template = variable.ToString(envVarName);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var str = engine.ToString();
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));

            #region CleanUp
            // Delete environment variable.
            Environment.SetEnvironmentVariable(envVarName, null);
            #endregion
            
            Assert.AreEqual(envVarValue, str);
        }

        [Test]
        public void TestGuidVariableSubstitution()
        {
            var variable = new GuidVariable();
            var template = variable.ToString(null);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var str = engine.ToString();
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));

            new Guid(str);
        }

        [Test]
        public void TestLineFeedVariableSubstitution()
        {
            var variable = new LineFeedVariable();
            var template = variable.ToString(null);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var str = engine.ToString();
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));

            Assert.AreEqual("\n", str);
        }

        [Test]
        public void TestLineFeedVariableWithArgSubstitution()
        {
            var variable = new LineFeedVariable();
            int count = 3;
            var template = variable.ToString(count.ToString());
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var str = engine.ToString();
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));

            Assert.AreEqual(new string('\n', count), str);
        }

        [Test]
        public void TestSpaceVariableSubstitution()
        {
            var variable = new SpaceVariable();
            var template = variable.ToString(null);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var str = engine.ToString();
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));

            Assert.AreEqual(" ", str);
        }

        [Test]
        public void TestSpaceVariableWithArgSubstitution()
        {
            var variable = new SpaceVariable();
            int count = 3;
            var template = variable.ToString(count.ToString());
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var str = engine.ToString();
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));

            Assert.AreEqual(new string(' ', count), str);
        }

        [Test]
        public void TestSeparatorVariableArgDirectorySubstitution()
        {
            var variable = new SeparatorVariable();
            var template = variable.ToString(SeparatorVariable.ArgDirectory);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var str = engine.ToString();
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));

            Assert.AreEqual(Path.DirectorySeparatorChar.ToString(), str);
        }

        [Test]
        public void TestSeparatorVariableArgPathSubstitution()
        {
            var variable = new SeparatorVariable();
            var template = variable.ToString(SeparatorVariable.ArgPath);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var str = engine.ToString();
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));

            Assert.AreEqual(Path.PathSeparator.ToString(), str);
        }

        [Test]
        public void TestSeparatorVariableArgVolumeSubstitution()
        {
            var variable = new SeparatorVariable();
            var template = variable.ToString(SeparatorVariable.ArgVolume);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var str = engine.ToString();
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));

            Assert.AreEqual(Path.VolumeSeparatorChar.ToString(), str);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSeparatorVariableArgUnknownSubstitution()
        {
            var variable = new SeparatorVariable();
            var template = variable.ToString("UNKNOWN");
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            engine.ToString();
        }
    }
}
