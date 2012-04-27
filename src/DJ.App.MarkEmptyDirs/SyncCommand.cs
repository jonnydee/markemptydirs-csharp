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
using System.Text;

using DR;
using DR.IO;

namespace DJ.App.MarkEmptyDirs
{
    
    public class SyncCommand : IDirectoryVisitor, ICommand
    {
        private Configuration _configuration;
        

        public int Execute(Configuration config)
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
            walker.FollowSymbolicLinks = _configuration.FollowSymbolicLinks;
            walker.TrackVisitedDirectories = true;
            walker.TrackVisitedFiles = true;
            walker.Walk(_configuration.Directory);
            
            return 0;
        }
        
        private bool MustVisit(IDirectoryWalkerContext context, DirectoryInfo dirInfo)
        {
            return !_configuration.Exclude.Contains(dirInfo.Name);
        }
        
        public bool PreVisit(IDirectoryWalkerContext context, DirectoryInfo dirInfo)
        {
            return MustVisit(context, dirInfo);
        }

        private bool IsPlaceHolderNeeded(IDirectoryWalkerContext context, DirectoryInfo dirInfo)
        {
            var files = dirInfo.GetFiles();

            // If there is more than one file we do not need a placeholder.
            if (files.Length > 1)
                return false;

            // If there are subdirectories we check if any of them will be walked, in that case we do not need a placeholder.
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                if (MustVisit(context, dir))
                    return false;
            }

            // Here we know that there is 0 or 1 file and subdirectories that are not going to be walked (special directories, excluded ones)

            if (files.Length == 0)
                // At this point there are no files and no tracked subdirectories, so we need a placeholder.
                return true;
            
            // At this point there is exactly one file. If it is a placeholder we actually need one.
            var placeHolderFile = new FileInfo(Path.Combine(dirInfo.FullName, _configuration.PlaceHolderName));
            return placeHolderFile.Exists;

            /*
            foreach (FileSystemInfo visitedFileSystemInfo in context.VisitedFileSystemInfos)
            {
                string visitedName = visitedFileSystemInfo.FullName;
                if (visitedName.Length <= dirName.Length || !visitedName.StartsWith(dirName))
                    continue;

                // At this point there is either a file in a sub-directory,
                // or in the current directory (dirInfo).

                // If the already visited file is in a sub-directory,
                // then no placeholder is needed for the current directory.
                var parentDir = PathUtil.GetParent(visitedFileSystemInfo);
                if (parentDir.FullName.Length > dirName.Length)
                    return false;

                // The already visited file is in the current directory.
                // So increase number of found candidates.
                ++foundCandidateFiles;
                // If there are more than one file we know we do not need a placeholder.
                if (foundCandidateFiles > 1)
                    return false;
            }
            
            if (1 == foundCandidateFiles)
            {
                // At this point there is exactly one file. If it is a placeholder we actually need one.
                var placeHolderFile = new FileInfo(Path.Combine(dirInfo.FullName, _configuration.PlaceHolderName));
                return placeHolderFile.Exists;
            }
            
            // At this point there is no file or subdirectory, so we need a placeholder.
            return true;
             */
        }

        public bool PostVisit(IDirectoryWalkerContext context, DirectoryInfo dirInfo)
        {
            if (IsPlaceHolderNeeded(context, dirInfo))
            {
                var placeHolderFile = CommandHelper.CreatePlaceHolder(dirInfo, _configuration);
                if (null != placeHolderFile)
                    context.VisitedFiles.Add(placeHolderFile.FullName, placeHolderFile);
            }
            else
            {
                var placeHolderFile = CommandHelper.DeletePlaceHolder(dirInfo, _configuration);
                if (null != placeHolderFile)
                    context.VisitedFiles.Remove(placeHolderFile.FullName);
            }
            return true;
        }

        public bool Visit(IDirectoryWalkerContext context, FileInfo fileInfo)
        {
            return true;
        }
    }

}
