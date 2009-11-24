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

using DR.Util;
using DR.Util.IO;

namespace DJ.App.MarkEmptyDirs
{
    public static class CommandHelper
    {
        public static int RunShellCommand(DirectoryInfo workingDir, string cmdLineString)
        {
            var cmdLine = CmdLineUtil.SplitCmdAndArgs(cmdLineString);
            var cmd = cmdLine[0];
            var args = cmdLine.Length > 1 ? cmdLine[1] : null;
            
            var psi = new System.Diagnostics.ProcessStartInfo(cmd, args);
            psi.WorkingDirectory = workingDir.FullName;
            psi.UseShellExecute = false;
            
            var proc = System.Diagnostics.Process.Start(psi);
            proc.WaitForExit();
            
            return proc.ExitCode;
        }
        
        public static FileInfo CreatePlaceHolder(DirectoryInfo dirInfo, Configuration config)
        {
            if (null == config)
                throw new ArgumentNullException("config");
            if (null == dirInfo)
                throw new ArgumentNullException("dirInfo");

            var placeHolderFile = new FileInfo(Path.Combine(dirInfo.FullName, config.PlaceHolderName));

            if (placeHolderFile.Exists)
                return null;

            var dynCtx = new Dictionary<string, object>();
            dynCtx[PlaceHolderVariable.ContextPlaceHolderFile] = placeHolderFile;
            dynCtx[DirectoryVariable.ContextBaseDir] = config.Directory;
            dynCtx[DirectoryVariable.ContextCurrentDir] = dirInfo;

            try
            {
                if (!config.DryRun)
                {
                    using (var fileStream = placeHolderFile.Create())
                    {
                        var content = config.VariableSubstitution ? config.PlaceHolderTemplate.ToString(dynCtx) : config.PlaceHolderTemplate.Template;
                        var byteData = Encoding.Default.GetBytes(content);
                        fileStream.Write(byteData, 0, byteData.Length);
                    }
                }
                if (config.Short)
                    Logger.Log(Logger.LogType.Info, placeHolderFile.FullName, true);
                else if (config.Verbose)
                    Logger.Log(Logger.LogType.Info, string.Format("Created placeholder: '{0}'", placeHolderFile.FullName));        
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.LogType.Error, string.Format("Creation of placeholder '{0}' failed: {1}", placeHolderFile.FullName, ex.Message));
                return null;
            }

            if (null != config.CreateHookTemplate)
            {
                try
                {
                    var cmdLineString = config.CreateHookTemplate.ToString(dynCtx);
                    if (!config.DryRun)
                        RunShellCommand(placeHolderFile.Directory, cmdLineString);
                    if (config.Short)
                        Logger.Log(Logger.LogType.Info, cmdLineString, true);
                    else if (config.Verbose)
                        Logger.Log(Logger.LogType.Info, string.Format("Executed placeholder creation hook: '{0}'", cmdLineString));
                }
                catch (Exception ex)
                {
                    Logger.Log(Logger.LogType.Warn, string.Format("Execution of placeholder creation hook '{0}' failed: {1}", placeHolderFile.FullName, ex.Message));
                }
            }
            
            return placeHolderFile;
        }
        
        public static FileInfo DeletePlaceHolder(DirectoryInfo dirInfo, Configuration config)
        {
            if (null == dirInfo)
                throw new ArgumentNullException("dirInfo");
            if (null == config)
                throw new ArgumentNullException("config");

            var placeHolderFile = new FileInfo(Path.Combine(dirInfo.FullName, config.PlaceHolderName));
            if (!placeHolderFile.Exists)
                return null;
        
            var dynCtx = new Dictionary<string, object>();
            dynCtx[PlaceHolderVariable.ContextPlaceHolderFile] = placeHolderFile;
            dynCtx[DirectoryVariable.ContextBaseDir] = config.Directory;
            dynCtx[DirectoryVariable.ContextCurrentDir] = dirInfo;

            if (null != config.DeleteHookTemplate)
            {
                try
                {
                    var cmdLineString = config.DeleteHookTemplate.ToString(dynCtx);
                    if (!config.DryRun)
                        RunShellCommand(placeHolderFile.Directory, cmdLineString);
                    if (config.Short)
                        Logger.Log(Logger.LogType.Info, cmdLineString, true);
                    else if (config.Verbose)
                        Logger.Log(Logger.LogType.Info, string.Format("Executed placeholder deletion hook: '{0}'", cmdLineString));
                }
                catch (Exception ex)
                {
                    Logger.Log(Logger.LogType.Warn, string.Format("Execution of placeholder deletion hook '{0}' failed: {1}", placeHolderFile.FullName, ex.Message));
                }
            }

            try
            {
                if (!config.DryRun)
                {
                    if (placeHolderFile.Exists)
                        placeHolderFile.Delete();
                }
                if (config.Short)
                    Logger.Log(Logger.LogType.Info, placeHolderFile.FullName, true);
                else if (config.Verbose)
                    Logger.Log(Logger.LogType.Info, string.Format("Deleted placeholder: '{0}'", placeHolderFile.FullName));
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.LogType.Error, string.Format("Deletion of placeholder '{0}' failed: {1}", placeHolderFile.FullName, ex.Message));
                return null;
            }

            return placeHolderFile;
        }
    }
}
