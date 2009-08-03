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

using DJ.Util;
using DJ.Util.IO;

namespace DJ.App.MarkEmptyDirs
{
    
    public class SyncCommand : IDirectoryVisitor, ICommand
    {
        private readonly List<FileInfo> _existingFiles;
        private Configuration _configuration;
        

        public SyncCommand()
        {
            _existingFiles = new List<FileInfo>();            
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
            
            DirectoryWalker.Walk(_configuration.Directory, this);
        }
        
        public bool PreVisit(DirectoryInfo dirInfo)
        {
            return !_configuration.Exclude.Contains(dirInfo.Name);
        }

        private bool IsPlaceHolderNeeded(DirectoryInfo dirInfo)
        {
            var dirName = dirInfo.FullName;
            foreach (var visitedFileInfo in _existingFiles)
            {
                var visitedFile = visitedFileInfo.FullName;
                if (!visitedFile.StartsWith(dirName))
                    continue;

                // At this point there is either a file in a sub-directory,
                // or in the current directory (dirInfo).

                // If the already visited file is in a sub-directory,
                // then no placeholder is needed for the current directory.
                if (visitedFileInfo.Directory.FullName.Length > dirName.Length)
                    return false;

                // The already visited file is in the current directory.
                // So if this file is not a placeholder file we do not need
                // a placeholder.
                var visitedFileName = visitedFileInfo.Name;
                if (visitedFileName != _configuration.PlaceHolderName)
                    return false;
            }
            return true;
        }

        private bool DeletePlaceHolder(DirectoryInfo dirInfo)
        {
            foreach (var file in dirInfo.GetFiles())
            {
                if (file.Name == _configuration.PlaceHolderName)
                {
                    var placeHolderFile = file;
                    try
                    {
                        if (!_configuration.DryRun)
                        {
                            placeHolderFile.Delete();
                        }
                        _existingFiles.Remove(placeHolderFile);
                        if (_configuration.Short)
                            Logger.Log(Logger.LogType.Info, placeHolderFile.FullName, true);
                        else if (_configuration.Verbose)
                            Logger.Log(Logger.LogType.Info, string.Format("Deleted placeholder: '{0}'", placeHolderFile.FullName));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(Logger.LogType.Error, string.Format("Deletion of placeholder '{0}' failed: {1}", placeHolderFile.FullName, ex.Message));
                        return false;
                    }
                }
            }
            return false;
        }

        private bool CreatePlaceHolder(DirectoryInfo dirInfo)
        {
            var placeHolderFile = new FileInfo(Path.Combine(dirInfo.FullName, _configuration.PlaceHolderName));

            try
            {
                if (placeHolderFile.Exists)
                    return false;

                if (!_configuration.DryRun)
                {
                    using (var fileStream = placeHolderFile.Create())
                    {
                        var byteData = Encoding.ASCII.GetBytes(_configuration.PlaceHolderTemplate.ToString());
                        fileStream.Write(byteData, 0, byteData.Length);
                    }
                }
                _existingFiles.Add(placeHolderFile);
                if (_configuration.Short)
                    Logger.Log(Logger.LogType.Info, placeHolderFile.FullName, true);
                else if (_configuration.Verbose)
                    Logger.Log(Logger.LogType.Info, string.Format("Created placeholder: '{0}'", placeHolderFile.FullName));
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.LogType.Error, string.Format("Creation of placeholder '{0}' failed: {1}", placeHolderFile.FullName, ex.Message));
            }

            return false;
        }
        
        public bool PostVisit(DirectoryInfo dirInfo)
        {
            if (IsPlaceHolderNeeded(dirInfo))
                CreatePlaceHolder(dirInfo);
            else
                DeletePlaceHolder(dirInfo);
            
            return true;
        }

        public bool Visit(FileInfo fileInfo)
        {
            _existingFiles.Add(fileInfo);
            return true;
        }
    }

}
