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

namespace DJ.App.MarkEmptyDirs
{
    [TestFixture]
    public class TestMain
    {
        [Test]
        public void TestCreateConfiguration()
        {
            var config = MainClass.CreateConfiguration();
            Assert.IsNotNull(config.Exclude);
            Assert.Contains(".hg", config.Exclude);
            Assert.IsNotEmpty(config.PlaceHolderName);
            Assert.IsNotNull(config.PlaceHolderTemplate);
        }
    }
}
