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

using DR.IO;
using DR.Template;

namespace DJ.App.MarkEmptyDirs
{
    [TestFixture]
    public class TestPlaceHolderVariable
    {
        [Test]
        public void TestPlaceHolderVariableArgFullNameSubstitution()
        {
            var variable = new PlaceHolderVariable();
            var template = variable.ToString(PlaceHolderVariable.ArgFullName);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var placeHolderFile = new FileInfo(PathUtil.Combine("path", "to", "placeholder", ".emptydir"));
            var dynCtx = new Dictionary<string, object>();
            dynCtx[PlaceHolderVariable.ContextPlaceHolderFile] = placeHolderFile;
            
            var str = engine.ToString(dynCtx);
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));
            
            Assert.AreEqual(placeHolderFile.FullName, str);
        }

        [Test]
        public void TestPlaceHolderVariableArgNameSubstitution()
        {
            var variable = new PlaceHolderVariable();
            var template = variable.ToString(PlaceHolderVariable.ArgName);
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var placeHolderFile = new FileInfo(PathUtil.Combine("path", "to", "placeholder", ".emptydir"));
            var dynCtx = new Dictionary<string, object>();
            dynCtx[PlaceHolderVariable.ContextPlaceHolderFile] = placeHolderFile;
            
            var str = engine.ToString(dynCtx);
            Console.WriteLine(string.Format("SUBSTITUTED STRING: '{0}'", str));
            
            Assert.AreEqual(placeHolderFile.Name, str);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestPlaceHolderVariableArgUnknownSubstitution()
        {
            var variable = new PlaceHolderVariable();
            var template = variable.ToString("UNKNOWN");
            
            var engine = new TemplateEngine(template);
            engine.AddVariable(variable);

            var dynCtx = new Dictionary<string, object>();
            
            engine.ToString(dynCtx);
        }
    }
}
