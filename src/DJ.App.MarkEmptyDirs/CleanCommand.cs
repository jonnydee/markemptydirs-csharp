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

using DR;
using DR.IO;

namespace DJ.App.MarkEmptyDirs
{
    
    public class CleanCommand : IDirectoryVisitor, ICommand
    {
        private Configuration _configuration;

        
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
            walker.VisitFiles = false;
            walker.TrackVisitedDirectories = true;
            walker.Walk(_configuration.Directory);
        }
        
        public bool PreVisit(IDirectoryWalkerContext context, DirectoryInfo dirInfo)
        {
            if (SymbolicLinkHelper.IsSymbolicLink(dirInfo))
            {
                context.VisitedDirectories.Add(dirInfo);

                if (!_configuration.FollowSymbolicLinks)
                {
                    return false;
                }

                // Get the symlink's targetPath and
                // if it is relative make it absolute based on dirInfo.
                var targetPath = SymbolicLinkHelper.GetSymbolicLinkTarget(dirInfo);
                if (!Path.IsPathRooted(targetPath))
                {
                    var parentDir = PathUtil.GetParent(dirInfo);
                    targetPath = Path.Combine(parentDir.FullName, targetPath);
                    targetPath = Path.GetFullPath(targetPath);
                }

                if (IsAlreadyVisited(context, targetPath))
                {
                    return false;
                }
            }

            if (_configuration.Exclude.Contains(dirInfo.Name))
                return false;

            return true;
        }
                
        private bool IsAlreadyVisited(IDirectoryWalkerContext context, string path)
        {
            foreach (var visitedFileSystemInfo in context.VisitedFileSystemInfos)
            {
                if (path == visitedFileSystemInfo.FullName)
                    return true;
            }
            return false;
        }
        
        public bool PostVisit(IDirectoryWalkerContext context, DirectoryInfo dirInfo)
        {
            CommandHelper.DeletePlaceHolder(dirInfo, _configuration);
            return true;
        }

        public bool Visit(IDirectoryWalkerContext context, FileInfo fileInfo)
        {
            return true;
        }
    }

}
