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

using DR;
using DR.IO;

namespace DJ.App.MarkEmptyDirs
{
    
    public class PurgeCommand : IDirectoryVisitor, ICommand
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
            walker.VisitFiles = false;
            walker.TrackVisitedDirectories = true;
            walker.Walk(_configuration.Directory);
            
            return 0;
        }
        
        public bool PreVisit(IDirectoryWalkerContext context, DirectoryInfo dirInfo)
        {
            if (_configuration.Exclude.Contains(dirInfo.Name))
                return false;

            var placeHolderFile = new FileInfo(Path.Combine(dirInfo.FullName, _configuration.PlaceHolderName));
            if (!placeHolderFile.Exists)
                return true;
            
            foreach (var subDirInfo in dirInfo.GetDirectories())
            {
                try
                {
                    if (!_configuration.DryRun)
                        subDirInfo.Delete(true);
                    if (_configuration.Short)
                        Logger.Log(Logger.LogType.Info, subDirInfo.FullName, true);
                    else if (_configuration.Verbose)
                        Logger.Log(Logger.LogType.Info, string.Format("Deleted directory: '{0}'", subDirInfo.FullName));
                }
                catch (Exception ex)
                {
                    Logger.Log(Logger.LogType.Warn, string.Format("Deletion of directory '{0}' failed: {1}", subDirInfo.FullName, ex.Message));
                }
            }
            
            foreach (var fileInfo in dirInfo.GetFiles())
            {
                try
                {
                    if (fileInfo.FullName == placeHolderFile.FullName)
                    {
                        if (_configuration.Verbose)
                            Logger.Log(Logger.LogType.Info, string.Format("Skipped placeholder: '{0}'", fileInfo.FullName));
                        
                        continue;
                    }
                    
                    if (!_configuration.DryRun)
                        fileInfo.Delete();
                    if (_configuration.Short)
                        Logger.Log(Logger.LogType.Info, fileInfo.FullName, true);
                    else if (_configuration.Verbose)
                        Logger.Log(Logger.LogType.Info, string.Format("Deleted file: '{0}'", fileInfo.FullName));
                }
                catch (Exception ex)
                {
                    Logger.Log(Logger.LogType.Warn, string.Format("Deletion of file '{0}' failed: {1}", fileInfo.FullName, ex.Message));
                }
            }
            
            return false;
        }
                
        public bool PostVisit(IDirectoryWalkerContext context, DirectoryInfo dirInfo)
        {
            return true;
        }

        public bool Visit(IDirectoryWalkerContext context, FileInfo fileInfo)
        {
            return true;
        }
    }
}
