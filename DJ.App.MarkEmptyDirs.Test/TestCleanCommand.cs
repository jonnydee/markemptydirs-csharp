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

using System.IO;

using NUnit.Framework;

using DJ.Util.IO;

namespace DJ.App.MarkEmptyDirs
{
    [TestFixture]
    public class TestCleanCommand
    {
        public const string TmpDirPath = "tmp";

        private DirectoryInfo _tmpDirInfo;
        
        [SetUp]
        public void SetUp()
        {
            _tmpDirInfo = new DirectoryInfo(TmpDirPath);
            _tmpDirInfo.Create();
            
            _tmpDirInfo.CreateSubdirectory(PathUtil.Combine("a", "b", "c")).Create();
            _tmpDirInfo.CreateSubdirectory(PathUtil.Combine("a", "d", ".git", "store")).Create();

            new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, ".emptydir")).Create().Close();
            new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", ".emptydir")).Create().Close();
            new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "file1")).Create().Close();
            new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b", "file2")).Create().Close();
            new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b", "c", ".emptydir")).Create().Close();
            new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".emptydir")).Create().Close();
            new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", "file3")).Create().Close();
            new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".git", ".emptydir")).Create().Close();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteRecursively.Delete(_tmpDirInfo);
        }

        [Test]
        public void TestCleanPlaceHolders()
        {
            var config = MainClass.CreateConfiguration();
            config.Directory = _tmpDirInfo;

            var cmd = new CleanCommand();
            cmd.Execute(config);

            Assert.IsTrue(new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "file1")).Exists);
            Assert.IsTrue(new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b", "file2")).Exists);
            Assert.IsTrue(new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", "file3")).Exists);
            Assert.IsTrue(new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".git", ".emptydir")).Exists);

            Assert.IsEmpty(_tmpDirInfo.GetFiles());
            Assert.AreEqual(1, new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a")).GetFiles().Length);
            Assert.AreEqual(1, new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b")).GetFiles().Length);
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b", "c")).GetFiles());
            Assert.AreEqual(1, new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d")).GetFiles().Length);
            Assert.AreEqual(1, new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".git")).GetFiles().Length);
        }
    }
}
