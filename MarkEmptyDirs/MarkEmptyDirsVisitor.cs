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

using DJ.Util.IO;

namespace DJ.App.MarkEmptyDirs
{
    class MarkEmptyDirsVisitor : ICommand, IDirectoryVisitor
    {
        public MarkEmptyDirsVisitor()
        {
            PlaceHolderName = ".emptydir";
            PlaceHolderText = string.Empty;
            Verbose = false;
            Short = false;
            DryRun = false;
            CleanUp = false;
            Exclude = new List<string> { ".bzr", ".cvs", ".git", ".hg", ".svn" };
        }

        public string PlaceHolderName { set; get; }

        public string PlaceHolderText { set; get; }

        public bool CleanUp { set; get; }

        public bool Verbose { set; get; }

        public bool Short { set; get; }

        public bool DryRun { set; get; }

        public List<string> Exclude { set; get; }


        public void Execute(List<Option> options)
        {
            string directory = null;

            Option opt;
            while (options.Count > 0)
            {
                opt = options[0];
                options.RemoveAt(0);
                
                if (OptionDescriptorDefinitions.PlaceHolderOptionDescriptor == opt.Descriptor)
                {
                    if (!string.IsNullOrEmpty(opt.Value))
                        PlaceHolderName = opt.Value;
                    continue;
                }
                
                if (OptionDescriptorDefinitions.TextOptionDescriptor == opt.Descriptor)
                {
                    if (!string.IsNullOrEmpty(opt.Value))
                        PlaceHolderText = opt.Value + '\n';
                    continue;
                }
                
                if (OptionDescriptorDefinitions.VerboseOptionDescriptor == opt.Descriptor)
                {
                    Verbose = true;
                    continue;
                }
                
                if (OptionDescriptorDefinitions.ShortOptionDescriptor == opt.Descriptor)
                {
                    Short = true;
                    continue;
                }
                
                if (OptionDescriptorDefinitions.CleanOptionDescriptor == opt.Descriptor)
                {
                    CleanUp = true;
                    continue;
                }

                if (OptionDescriptorDefinitions.DryRunOptionDescriptor == opt.Descriptor)
                {
                    DryRun = true;
                    continue;
                }

                if (OptionDescriptorDefinitions.ExcludeOptionDescriptor == opt.Descriptor)
                {
                    var dirs = null != opt.Value ? opt.Value.Split(Path.PathSeparator) : new string[0];
                    var dirList = new List<string>(dirs.Length);
                    dirList.AddRange(dirs);
                    Exclude = dirList;
                }
                
                if (OptionDescriptorDefinitions.ListOptionDescriptor == opt.Descriptor)
                {
                    Short = DryRun = CleanUp = true;
                    continue;
                }

                // If we have a descriptor-less option assume it is the directory parameter.
                if (null == directory && OptionType.Short != opt.OptionType && null == opt.Descriptor)
                {
                    directory = opt.Value;
                    continue;
                }

                throw new Exception(string.Format("Unknown option: '{0}'", opt.Name));
            }

            if (null == directory)
            {
                throw new Exception("No directory specified!");
            }

            var dirInfo = new DirectoryInfo(directory);
            if (!dirInfo.Exists)
            {
                throw new Exception(string.Format("Not a directory: '{0}'", dirInfo.FullName));
            }

            DirectoryWalker.Walk(dirInfo, this);
        }
        
        public bool PreVisit(DirectoryInfo dirInfo)
        {
            return !Exclude.Contains(dirInfo.Name);
        }

        public bool PostVisit(DirectoryInfo dirInfo)
        {
            if (CleanUp)
                return true;

            var createPlaceHolder = dirInfo.GetFileSystemInfos().Length == 0;
            if (createPlaceHolder)
            {
                var placeHolderFile = new FileInfo(Path.Combine(dirInfo.FullName, PlaceHolderName));

                try
                {
                    if (!DryRun)
                    {
                        using (var fileStream = placeHolderFile.Create())
                        {
                            var byteData = Encoding.ASCII.GetBytes(PlaceHolderText);
                            fileStream.Write(byteData, 0, byteData.Length);
                        }
                    }
                    if (Short)
                        Logger.Log(Logger.LogType.Info, placeHolderFile.FullName);
                    else if (Verbose)
                        Logger.Log(Logger.LogType.Info, string.Format("Created placeholder: '{0}'", placeHolderFile.FullName));
                }
                catch (Exception ex)
                {
                    Logger.Log(Logger.LogType.Error, string.Format("Creation of placeholder '{0}' failed: {1}", placeHolderFile.FullName, ex.Message));
                }
            }

            return true;
        }

        public bool Visit(FileInfo fileInfo)
        {
            if (fileInfo.Name == PlaceHolderName)
            {
                var deletePlaceHolder = CleanUp || fileInfo.Directory.GetFileSystemInfos().Length > 1;
                if (deletePlaceHolder)
                {
                    try
                    {
                        if (!DryRun)
                            fileInfo.Delete();
                        if (Short)
                            Logger.Log(Logger.LogType.Info, fileInfo.FullName);
                        else if (Verbose)
                            Logger.Log(Logger.LogType.Info, string.Format("Deleted placeholder: '{0}'", fileInfo.FullName));
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(Logger.LogType.Info, string.Format("Deletion of placeholder '{0}' failed: {1}", fileInfo.FullName, ex.Message));
                    }
                }
            }

            return true;
        }
    }
}