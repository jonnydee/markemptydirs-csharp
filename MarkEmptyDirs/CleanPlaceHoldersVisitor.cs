//  
//  Copyright (c) 2009 by Copyright (c) 2009 by Johann Duscher (alias Jonny Dee)
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using System.IO;

using DJ.Util.IO;

namespace DJ.App.MarkEmptyDirs
{
    
    public class CleanPlaceHoldersVisitor : IDirectoryVisitor, ICommand
    {
        private Configuration _configuration;

        
        public void Execute(Configuration config)
        {
            _configuration = config;
            DirectoryWalker.Walk(_configuration.Directory, this);
        }
        
        public bool PreVisit(DirectoryInfo dirInfo)
        {
            return !_configuration.Exclude.Contains(dirInfo.Name);
        }
                
        public bool PostVisit(DirectoryInfo dirInfo)
        {
            return true;
        }

        public bool Visit(FileInfo fileInfo)
        {
            if (!fileInfo.Exists || fileInfo.Name != _configuration.PlaceHolderName)
                return true;

            var placeHolderFile = fileInfo;
            try
            {
                if (!_configuration.DryRun)
                    placeHolderFile.Delete();

                if (_configuration.Short)
                    Logger.Log(Logger.LogType.Info, placeHolderFile.FullName, true);
                else if (_configuration.Verbose)
                    Logger.Log(Logger.LogType.Info, string.Format("Deleted placeholder: '{0}'", placeHolderFile.FullName));
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.LogType.Error, string.Format("Deletion of placeholder '{0}' failed: {1}", placeHolderFile.FullName, ex.Message));
            }
                
            return true;
        }
    }

}
