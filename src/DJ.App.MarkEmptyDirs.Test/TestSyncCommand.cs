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
using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

using DR.IO;

namespace DJ.App.MarkEmptyDirs
{
    [TestFixture]
    public class TestSyncCommand
    {
        public const string TmpDirPath = "tmp";
        public const string TmpDirPath2 = "tmp2";
        public const string TmpDirPath3 = "tmp3";
        public const string TmpDirPath4 = "tmp4";
        public const string TmpDirPath5 = "tmp5";
        public const string TmpDirPath6 = "tmp6";
        
        private DirectoryInfo _tmpDirInfo;
        private DirectoryInfo _tmpDirInfo2;
        private DirectoryInfo _tmpDirInfo3;
        private DirectoryInfo _tmpDirInfo4;
        private DirectoryInfo _tmpDirInfo5;
        private DirectoryInfo _tmpDirInfo6;
        
        [SetUp]
        public void SetUp()
        {
            _tmpDirInfo = new DirectoryInfo(TmpDirPath);
            _tmpDirInfo.Create();
            _tmpDirInfo2 = new DirectoryInfo(TmpDirPath2);
            _tmpDirInfo2.Create();
            _tmpDirInfo3 = new DirectoryInfo(TmpDirPath3);
            _tmpDirInfo3.Create();
            _tmpDirInfo4 = new DirectoryInfo(TmpDirPath4);
            _tmpDirInfo4.Create();
            _tmpDirInfo5 = new DirectoryInfo(TmpDirPath5);
            _tmpDirInfo5.Create();
            _tmpDirInfo6 = new DirectoryInfo(TmpDirPath6);
            _tmpDirInfo6.Create();

            _tmpDirInfo.CreateSubdirectory(PathUtil.Combine("a", "b", "c")).Create();
            _tmpDirInfo.CreateSubdirectory(PathUtil.Combine("a", "d", ".git", "store")).Create();
            _tmpDirInfo.CreateSubdirectory(PathUtil.Combine("a", "e")).Create();
            _tmpDirInfo4.CreateSubdirectory(PathUtil.Combine("dir1", "dir2")).Create();
            _tmpDirInfo5.CreateSubdirectory(PathUtil.Combine("dir3", "dir4")).Create();
			
            SymbolicLinkHelper.CreateSymbolicLink(new DirectoryInfo(PathUtil.Combine("..", "..", "..", TmpDirPath2)), new FileInfo(PathUtil.Combine(_tmpDirInfo.ToString(), "a", "e", "link-to-dir")));
            // These links create cycles to make sure they are handled correctly.
            SymbolicLinkHelper.CreateSymbolicLink(new DirectoryInfo(PathUtil.Combine("..", "..")), new FileInfo(PathUtil.Combine(_tmpDirInfo.ToString(), "a", "e", "link-to-parent-of-parent")));
            SymbolicLinkHelper.CreateSymbolicLink(new DirectoryInfo(PathUtil.Combine("..", TmpDirPath + Path.DirectorySeparatorChar)), new FileInfo(PathUtil.Combine(_tmpDirInfo.ToString(), "link-to-self")));
            SymbolicLinkHelper.CreateSymbolicLink(new DirectoryInfo(PathUtil.Combine("..", TmpDirPath3)), new FileInfo(PathUtil.Combine(_tmpDirInfo3.ToString(), "link-to-self")));
            SymbolicLinkHelper.CreateSymbolicLink(new DirectoryInfo(PathUtil.Combine("..", "..", "..", TmpDirPath5)), new FileInfo(PathUtil.Combine(_tmpDirInfo4.ToString(), "dir1", "dir2", "link-to-other")));
        }

        [TearDown]
        public void TearDown()
        {
            DeleteRecursively.Delete(_tmpDirInfo);
            DeleteRecursively.Delete(_tmpDirInfo2);
            DeleteRecursively.Delete(_tmpDirInfo3);
            DeleteRecursively.Delete(_tmpDirInfo4);
            DeleteRecursively.Delete(_tmpDirInfo5);
            DeleteRecursively.Delete(_tmpDirInfo6);
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
            Assert.AreEqual(2, new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "e")).GetFileSystemInfos().Length);
            Assert.IsEmpty(_tmpDirInfo.GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a")).GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b")).GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".git")).GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".git", "store")).GetFiles());
            Assert.IsEmpty(_tmpDirInfo2.GetFiles());
        }
        
        [Test]
        public void TestCreatePlaceHolders2()
        {
            var symlinkTestDir = _tmpDirInfo6;
            symlinkTestDir.Create();
            symlinkTestDir.CreateSubdirectory(PathUtil.Combine("foo", "empty"));
            SymbolicLinkHelper.CreateSymbolicLink(new DirectoryInfo(PathUtil.Combine("foo", "empty")), new FileInfo(PathUtil.Combine(TmpDirPath6, "empty")));
            
            var config = MainClass.CreateConfiguration();
            config.FollowSymbolicLinks = false;
            config.Directory = symlinkTestDir;

            var cmd = new SyncCommand();
            cmd.Execute(config);
            
            Assert.AreEqual(1, new DirectoryInfo(PathUtil.Combine(TmpDirPath6, "foo", "empty")).GetFileSystemInfos().Length);
        }

        [Test]
        public void TestCreatePlaceHoldersWithFollowSymbolicLinks()
        {
            var config = MainClass.CreateConfiguration();
            config.FollowSymbolicLinks = true;
            config.Directory = _tmpDirInfo;

            var cmd = new SyncCommand();
            cmd.Execute(config);

            Assert.IsTrue(new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b", "c", ".emptydir")).Exists);
            Assert.AreEqual(1, new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b", "c")).GetFiles().Length);
            Assert.IsTrue(new FileInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".emptydir")).Exists);
            Assert.AreEqual(1, new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d")).GetFiles().Length);
            Assert.AreEqual(2, new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "e")).GetFileSystemInfos().Length);
            Assert.IsEmpty(_tmpDirInfo.GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a")).GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "b")).GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".git")).GetFiles());
            Assert.IsEmpty(new DirectoryInfo(PathUtil.Combine(_tmpDirInfo.FullName, "a", "d", ".git", "store")).GetFiles());
            Assert.AreEqual(1, _tmpDirInfo2.GetFiles().Length);
        }

        [Test]
        public void TestCreatePlaceHoldersWithFollowSymbolicLinks2()
        {
            var config = MainClass.CreateConfiguration();
            config.FollowSymbolicLinks = true;
            config.Directory = _tmpDirInfo3;

            var cmd = new SyncCommand();
            cmd.Execute(config);

            Assert.AreEqual(1, _tmpDirInfo3.GetFileSystemInfos().Length);
        }

        [Test]
        public void TestCreatePlaceHoldersWithFollowSymbolicLinks3()
        {
            var config = MainClass.CreateConfiguration();
            config.FollowSymbolicLinks = true;
            config.Directory = _tmpDirInfo4;

            var cmd = new SyncCommand();
            cmd.Execute(config);

            Assert.AreEqual(1, _tmpDirInfo5.GetFileSystemInfos().Length);
            Assert.IsTrue(new FileInfo(PathUtil.Combine(_tmpDirInfo5.FullName, "dir3", "dir4", ".emptydir")).Exists);
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
