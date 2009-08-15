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
using System.IO;

using NUnit.Framework;

using DJ.Util.IO;
using DJ.Util.Template;

namespace DJ.App.MarkEmptyDirs
{
    [TestFixture]
    public class TestDirectoryVariable
    {
        [Test]
        public void TestDirectoryVariableModeBaseSubstitution()
        {
            var variable = new DirectoryVariable();
            var template = variable.ToString(DirectoryVariable.ModeBase);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var baseDir = new DirectoryInfo(PathUtil.Combine("path", "to", "base"));
            var dynCtx = new Dictionary<string, object>();
            dynCtx[DirectoryVariable.ContextBaseDir] = baseDir;
            
            var str = engine.ToString(dynCtx);
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));
            
            Assert.AreEqual(baseDir.FullName, str);
        }

        [Test]
        public void TestDirectoryVariableModeCurrentSubstitution()
        {
            var variable = new DirectoryVariable();
            var template = variable.ToString(DirectoryVariable.ModeCurrent);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var currentDir = new DirectoryInfo(PathUtil.Combine("path", "to", "base", "down", "to", "current"));
            var dynCtx = new Dictionary<string, object>();
            dynCtx[DirectoryVariable.ContextCurrentDir] = currentDir;
            
            var str = engine.ToString(dynCtx);
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));
            
            Assert.AreEqual(currentDir.FullName, str);
        }

        [Test]
        public void TestDirectoryVariableModeCurrentRelativeSubstitution()
        {
            var variable = new DirectoryVariable();
            var template = variable.ToString(DirectoryVariable.ModeCurrentRelative);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var baseDir = new DirectoryInfo(PathUtil.Combine("path", "to", "base"));
            var currentDir = new DirectoryInfo(PathUtil.Combine("path", "to", "base", "down", "to", "current"));
            var dynCtx = new Dictionary<string, object>();
            dynCtx[DirectoryVariable.ContextBaseDir] = baseDir;
            dynCtx[DirectoryVariable.ContextCurrentDir] = currentDir;
            
            var str = engine.ToString(dynCtx);
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));

            var currentDirRelative = PathUtil.Combine("down", "to", "current");
            Assert.AreEqual(currentDirRelative, str);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDirectoryVariableModeUnknownSubstitution()
        {
            var variable = new DirectoryVariable();
            var template = variable.ToString("UNKNOWN");
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var dynCtx = new Dictionary<string, object>();
            
            engine.ToString(dynCtx);
        }
    }
}
