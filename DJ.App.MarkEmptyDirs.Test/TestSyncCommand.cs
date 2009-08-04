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

namespace DJ.App.MarkEmptyDirs
{
    [TestFixture]
    public class TestSyncCommand
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
        }

        [TearDown]
        public void TearDown()
        {
            DeleteRecursively.Delete(_tmpDirInfo);
        }

        [Test]
        public void TestCreatePlaceHolders()
        {
            var config = MainClass.CreateConfiguration();
            config.Directory = _tmpDirInfo;

            var cmd = new SyncCommand();
            cmd.Execute(config);

            Assert.IsTrue(new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b", "c", ".emptydir")).Exists);
            Assert.AreEqual(1, new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b", "c")).GetFiles().Length);
            Assert.IsTrue(new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".emptydir")).Exists);
            Assert.AreEqual(1, new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d")).GetFiles().Length);
            Assert.IsEmpty(_tmpDirInfo.GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a")).GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b")).GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".git")).GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".git", "store")).GetFiles());
        }

        [Test]
        public void TestSyncPlaceHolders()
        {
            TestCreatePlaceHolders();
            var file1 = new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b", "c", "file1"));
            file1.Create().Close();
            var dir12 = new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", "dir1", "dir2"));
            dir12.Create();
            
            var config = MainClass.CreateConfiguration();
            config.Directory = _tmpDirInfo;

            var cmd = new SyncCommand();
            cmd.Execute(config);

            Assert.IsFalse(new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b", "c", ".emptydir")).Exists);
            Assert.AreEqual(1, new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b", "c")).GetFiles().Length);
            
            Assert.IsTrue(new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", "dir1", "dir2", ".emptydir")).Exists);
            Assert.AreEqual(1, new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", "dir1", "dir2")).GetFiles().Length);
            
            Assert.IsEmpty(_tmpDirInfo.GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a")).GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b")).GetFiles());
            Assert.IsTrue(new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b", "c", "file1")).Exists);
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d")).GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".git")).GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".git", "store")).GetFiles());
        }
    }
}
