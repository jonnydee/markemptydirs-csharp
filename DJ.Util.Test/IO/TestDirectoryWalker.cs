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

namespace DJ.Util.IO
{
    [TestFixture()]
    public class TestDirectoryWalker
    {
        public const string TmpDirPath = "tmp";

        private DirectoryInfo _tmpDirInfo;
        private DirectoryInfo _aDirInfo;
        private DirectoryInfo _abDirInfo;
        private DirectoryInfo _cDirInfo;
        private FileInfo _a1FileInfo;
        private FileInfo _a2FileInfo;
        private FileInfo _ab3FileInfo;
        private FileInfo _ab4FileInfo;
        private FileInfo _c5FileInfo;
        private FileInfo _c6FileInfo;
        
        [SetUp]
        public void SetUp()
        {
            _tmpDirInfo = new DirectoryInfo(TmpDirPath);
            _tmpDirInfo.Create();
            _aDirInfo = _tmpDirInfo.CreateSubdirectory("a");
            _aDirInfo.Create();
            _abDirInfo = _aDirInfo.CreateSubdirectory("b");
            _abDirInfo.Create();
            _cDirInfo = _tmpDirInfo.CreateSubdirectory("c");
            _cDirInfo.Create();

            _a1FileInfo = new FileInfo(Path.Combine(_aDirInfo.FullName, "1"));
            _a1FileInfo.Create();
            _a2FileInfo = new FileInfo(Path.Combine(_aDirInfo.FullName, "2"));
            _a2FileInfo.Create();
            _ab3FileInfo = new FileInfo(Path.Combine(_abDirInfo.FullName, "3"));
            _ab3FileInfo.Create();
            _ab4FileInfo = new FileInfo(Path.Combine(_abDirInfo.FullName, "4"));
            _ab4FileInfo.Create();
            _c5FileInfo = new FileInfo(Path.Combine(_cDirInfo.FullName, "5"));
            _c5FileInfo.Create();
            _c6FileInfo = new FileInfo(Path.Combine(_cDirInfo.FullName, "6"));
            _c6FileInfo.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _c6FileInfo.Delete();
            _c5FileInfo.Delete();
            _ab4FileInfo.Delete();
            _ab3FileInfo.Delete();
            _a2FileInfo.Delete();
            _a1FileInfo.Delete();

            _cDirInfo.Delete();
            _abDirInfo.Delete();
            _aDirInfo.Delete();
            _tmpDirInfo.Delete();
        }
        
        List<string> GetFileSystemInfoFullNames()
        {
            return new List<string>
            {
                _ab3FileInfo.FullName,
                _ab4FileInfo.FullName,
                _abDirInfo.FullName,
                _a1FileInfo.FullName,
                _a2FileInfo.FullName,
                _aDirInfo.FullName,
                _c5FileInfo.FullName,
                _c6FileInfo.FullName,
                _cDirInfo.FullName,
                _tmpDirInfo.FullName,
            };
        }
        
        [Test]
        public void TestDepthFirstVisitAllFileSystemInfos()
        {
            var visitor = new TestDepthFirstVisitAllFileSystemInfosVisitor() { FileSystemInfoFullNames = GetFileSystemInfoFullNames() };
            DirectoryWalker.Walk(_tmpDirInfo, visitor);
            Assert.AreEqual(0, visitor.FileSystemInfoFullNames.Count);
        }

        class TestDepthFirstVisitAllFileSystemInfosVisitor : IDirectoryVisitor
        {
            public List<string> FileSystemInfoFullNames { set; get; }
            
            public bool PreVisit (DirectoryInfo dirInfo)
            {
                Console.WriteLine("PreVisit: " + dirInfo.FullName);
                return true;
            }
            
            public bool PostVisit (DirectoryInfo dirInfo)
            {
                Console.WriteLine("PostVisit: " + dirInfo.FullName);
                bool equalFullName = FileSystemInfoFullNames[0] == dirInfo.FullName;
                if (equalFullName)
                    FileSystemInfoFullNames.RemoveAt(0);
                return equalFullName;
            }
            
            public bool Visit (FileInfo fileInfo)
            {
                Console.WriteLine("Visit: " + fileInfo.FullName);
                bool equalFullName = FileSystemInfoFullNames[0] == fileInfo.FullName;
                if (equalFullName)
                    FileSystemInfoFullNames.RemoveAt(0);
                return equalFullName;
            }
        }

        [Test]
        public void TestPreVisitFalsePrunesDirectory()
        {
            var visitor = new TestPreVisitFalsePrunesDirectoryVisitor() 
            {
                FileSystemInfoFullNames = GetFileSystemInfoFullNames(),
                PruneDirectoryFullName = _abDirInfo.FullName,
            };
            DirectoryWalker.Walk(_tmpDirInfo, visitor);
            
            foreach (var fullName in visitor.FileSystemInfoFullNames)
            {
                Console.WriteLine("Pruned: " + fullName);
                Assert.That(fullName.StartsWith(_abDirInfo.FullName));
            }
        }

        class TestPreVisitFalsePrunesDirectoryVisitor : IDirectoryVisitor
        {
            public List<string> FileSystemInfoFullNames { set; get; }
            public string PruneDirectoryFullName { set; get; }
            
            public bool PreVisit (DirectoryInfo dirInfo)
            {
                Console.WriteLine("PreVisit: " + dirInfo.FullName);
                return PruneDirectoryFullName != dirInfo.FullName;
            }
            
            public bool PostVisit (DirectoryInfo dirInfo)
            {
                Console.WriteLine("PostVisit: " + dirInfo.FullName);
                return FileSystemInfoFullNames.Remove(dirInfo.FullName);
            }
            
            public bool Visit (FileInfo fileInfo)
            {
                Console.WriteLine("Visit: " + fileInfo.FullName);
                return FileSystemInfoFullNames.Remove(fileInfo.FullName);
            }
        }

        [Test]
        public void TestVisitFileReturnsFalseStopsWalking()
        {
            var visitor = new TestVisitFileReturnsFalseStopsWalkingVisitor() 
            {
                FileSystemInfoFullNames = GetFileSystemInfoFullNames(),
                StopWalkingAfterVisitFileFullName = _a1FileInfo.FullName,
            };
            DirectoryWalker.Walk(_tmpDirInfo, visitor);

            var prunedFileSystemInfoFullNames = new List<string>
            {
                _a2FileInfo.FullName,
                _c5FileInfo.FullName,
                _c6FileInfo.FullName,
                _cDirInfo.FullName,
            };
            
            for (int i = 0; i < visitor.FileSystemInfoFullNames.Count; i++)
            {
                Console.WriteLine("Pruned: " + visitor.FileSystemInfoFullNames[i]);
                Assert.That(visitor.FileSystemInfoFullNames[i] == prunedFileSystemInfoFullNames[i]);
            }
        }

        class TestVisitFileReturnsFalseStopsWalkingVisitor : IDirectoryVisitor
        {
            public List<string> FileSystemInfoFullNames { set; get; }
            public string StopWalkingAfterVisitFileFullName { set; get; }
            
            public bool PreVisit (DirectoryInfo dirInfo)
            {
                Console.WriteLine("PreVisit: " + dirInfo.FullName);
                return true;
            }
            
            public bool PostVisit (DirectoryInfo dirInfo)
            {
                Console.WriteLine("PostVisit: " + dirInfo.FullName);
                return FileSystemInfoFullNames.Remove(dirInfo.FullName);
            }
            
            public bool Visit (FileInfo fileInfo)
            {
                Console.WriteLine("Visit: " + fileInfo.FullName);
                return FileSystemInfoFullNames.Remove(fileInfo.FullName) && (StopWalkingAfterVisitFileFullName != fileInfo.FullName);
            }
        }

        [Test]
        public void TestPostVisitDirectoryReturnsFalseStopsWalking()
        {
            var visitor = new TestPostVisitDirectoryReturnsFalseStopsWalkingVisitor() 
            {
                FileSystemInfoFullNames = GetFileSystemInfoFullNames(),
                StopWalkingAfterPostVisitDirectoryFullName = _aDirInfo.FullName,
            };
            DirectoryWalker.Walk(_tmpDirInfo, visitor);

            var prunedFileSystemInfoFullNames = new List<string>
            {
                _c5FileInfo.FullName,
                _c6FileInfo.FullName,
                _cDirInfo.FullName,
            };
            
            for (int i = 0; i < visitor.FileSystemInfoFullNames.Count; i++)
            {
                Console.WriteLine("Pruned: " + visitor.FileSystemInfoFullNames[i]);
                Assert.That(visitor.FileSystemInfoFullNames[i] == prunedFileSystemInfoFullNames[i]);
            }
        }

        class TestPostVisitDirectoryReturnsFalseStopsWalkingVisitor : IDirectoryVisitor
        {
            public List<string> FileSystemInfoFullNames { set; get; }
            public string StopWalkingAfterPostVisitDirectoryFullName { set; get; }
            
            public bool PreVisit (DirectoryInfo dirInfo)
            {
                Console.WriteLine("PreVisit: " + dirInfo.FullName);
                return true;
            }
            
            public bool PostVisit (DirectoryInfo dirInfo)
            {
                Console.WriteLine("PostVisit: " + dirInfo.FullName);
                return FileSystemInfoFullNames.Remove(dirInfo.FullName) && (StopWalkingAfterPostVisitDirectoryFullName != dirInfo.FullName);
            }
            
            public bool Visit (FileInfo fileInfo)
            {
                Console.WriteLine("Visit: " + fileInfo.FullName);
                return FileSystemInfoFullNames.Remove(fileInfo.FullName);
            }
        }
    }
}
