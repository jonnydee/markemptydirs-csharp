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
using System.Text;

using DR;
using DR.IO;

namespace DJ.App.MarkEmptyDirs
{
    
    public class SyncCommand : IDirectoryVisitor, ICommand
    {
        private readonly List<FileSystemInfo> _existingFiles;
        private Configuration _configuration;
        

        public SyncCommand()
        {
            _existingFiles = new List<FileSystemInfo>();            
        }


        public void Execute(Configuration config)
        {
            _configuration = config;

            if (null == _configuration.Directory)
            {
                throw new Exception("No directory specified!");
            }

            if (!_configuration.Directory.Exists)
            {
                throw new Exception(string.Format("Not a directory: '{0}'", _configuration.Directory.FullName));
            }
            
            var walker = DirectoryWalker.Create(this);
            walker.Walk(_configuration.Directory);
        }
        
        public bool PreVisit(DirectoryInfo dirInfo)
        {
            if (SymbolicLinkHelper.IsSymbolicLink(dirInfo))
            {
                if (!_configuration.FollowSymbolicLinks)
                {
                    _existingFiles.Add(dirInfo);
                    return false;
                }

                // Get the symlink's targetPath and
                // if it is relative make it absolute based on dirInfo.
                var targetPath = SymbolicLinkHelper.GetSymbolicLinkTarget(dirInfo);
                if (!Path.IsPathRooted(targetPath))
                {
                    var parentDir = PathUtil.GetParent(dirInfo);
                    targetPath = Path.Combine(parentDir.FullName, targetPath);
                    // Normalize path.
                    targetPath = Path.GetFullPath(targetPath);
                }

                if (IsAlreadyVisited(targetPath))
                {
                    return false;
                }
            }

            if (_configuration.Exclude.Contains(dirInfo.Name))
                return false;

            _existingFiles.Add(dirInfo);
            return true;
        }

        private bool IsAlreadyVisited(string path)
        {
            foreach (var visitedFileInfo in _existingFiles)
            {
                if (path == visitedFileInfo.FullName)
                    return true;
            }
            return false;
        }
        
        private bool IsPlaceHolderNeeded(DirectoryInfo dirInfo)
        {
            var dirName = dirInfo.FullName;
            foreach (var visitedFileInfo in _existingFiles)
            {
                if (visitedFileInfo.FullName.Length <= dirName.Length || !visitedFileInfo.FullName.StartsWith(dirName))
                    continue;

                // At this point there is either a file in a sub-directory,
                // or in the current directory (dirInfo).

                // If the already visited file is in a sub-directory,
                // then no placeholder is needed for the current directory.
                if (PathUtil.GetParent(visitedFileInfo).FullName.Length > dirName.Length)
                    return false;

                // The already visited file is in the current directory.
                // So if this file is not a placeholder file we do not need
                // a placeholder.
                if (visitedFileInfo.Name != _configuration.PlaceHolderName)
                    return false;
            }
            return true;
        }

        public bool PostVisit(DirectoryInfo dirInfo)
        {
            if (IsPlaceHolderNeeded(dirInfo))
            {
                var placeHolderFile = CommandHelper.CreatePlaceHolder(dirInfo, _configuration);
                if (null != placeHolderFile)
                    _existingFiles.Add(placeHolderFile);
            }
            else
            {
                var placeHolderFile = CommandHelper.DeletePlaceHolder(dirInfo, _configuration);
                if (null != placeHolderFile)
                    _existingFiles.Remove(placeHolderFile);
            }
            return true;
        }

        public bool Visit(FileInfo fileInfo)
        {
            _existingFiles.Add(fileInfo);
            return true;
        }
    }

}
