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
using System.Collections;
using System.IO;

using DR;
using DR.IO;
using DR.Template;

namespace DJ.App.MarkEmptyDirs
{

    public class MainClass
    {
        public const string StandardCmdName = "MarkEmptyDirs";
        public const string StandardCmdExtension = "exe";
        public const string Version = "V1.6dev";
        public const string Copyright = "Copyright (c) 2009 by Johann Duscher (alias Jonny Dee)";
        public const string ProjectUrl = "http://code.google.com/p/markemptydirs";
        public const string StandardPlaceHolderName = ".emptydir";
        public static readonly string[] StandardExcludedDirs = new[] { ".bzr", "CVS", ".git", ".hg", ".svn" };
        public const bool StandardVariableSubstitution = true;
        public const bool StandardFollowSymbolicLinks = false;
        public const string SettingsEnvironmentVariable = "MarkEmptyDirsOpts";


        private static string[] ParseArgs(string argsStr)
        {
            // TODO Improve parsing in order to correctly handle string args of the form "...".
            return argsStr.Split(' ');
        }

        public static string GetSettingsInEnvironmentVariable()
        {
            try
            {
                var env = Environment.GetEnvironmentVariables();
                foreach (var key in env.Keys)
                {
                    if (key.ToString().ToUpper() == SettingsEnvironmentVariable.ToUpper())
                        return env[key].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                // Ignore any exception here as there is nothing critical to handle.
                Logger.Log(Logger.LogType.Warn, ex);
            }
            return null;
        }

        private static string[] GetDecoratedArgs(string[] args)
        {
            var envVarValue = GetSettingsInEnvironmentVariable();
            if (null != envVarValue)
            {
                var defaultArgs = ParseArgs(envVarValue);
                var newArgs = new string[defaultArgs.Length + args.Length];
                defaultArgs.CopyTo(newArgs, 0);
                args.CopyTo(newArgs, defaultArgs.Length);
                return newArgs;
            }
            return args;
        }

        public static TemplateEngine CreateTemplateEngine(string template)
        {
            var engine = new TemplateEngine(template);
            
            engine.AddVariable(new DateTimeVariable());
            engine.AddVariable(new DirectoryVariable());
            engine.AddVariable(new EnvironmentVariable());
            engine.AddVariable(new GuidVariable());
            engine.AddVariable(new LineFeedVariable());
            engine.AddVariable(new SeparatorVariable());
            engine.AddVariable(new SpaceVariable());
            engine.AddVariable(new PlaceHolderVariable());
            
            return engine;
        }

        public static Configuration CreateConfiguration()
        {
            return new Configuration
            {
                PlaceHolderName = StandardPlaceHolderName,
                PlaceHolderTemplate = CreateTemplateEngine(string.Empty),
                VariableSubstitution = StandardVariableSubstitution,
                FollowSymbolicLinks = StandardFollowSymbolicLinks,
                Verbose = false,
                Short = false,
                DryRun = false,
                CleanUp = false,
                Exclude = new List<string>(StandardExcludedDirs),
            };
        }
        
        public static Configuration ParseConfiguration(string[] args)
        {
            var optionParser = new OptionParser(OptionDescriptorDefinitions.OptionDescriptors);
            var options = optionParser.ParseOptions(args);
            
            var config = CreateConfiguration();

            string directory = null;
            
            Option opt;
            while (options.Count > 0)
            {
                opt = options[0];
                options.RemoveAt(0);
                
                if (OptionDescriptorDefinitions.PlaceHolderOptionDescriptor == opt.Descriptor)
                {
                    if (!string.IsNullOrEmpty(opt.Value))
                        config.PlaceHolderName = opt.Value;
                    else
                        throw new Exception(string.Format("No value provided for option: '{0}'", opt.Name));
                    continue;
                }
                
                if (OptionDescriptorDefinitions.TextOptionDescriptor == opt.Descriptor)
                {
                    if (!string.IsNullOrEmpty(opt.Value))
                        config.PlaceHolderTemplate = CreateTemplateEngine(opt.Value);
                    continue;
                }
                
                if (OptionDescriptorDefinitions.PlaceHolderFileOptionDescriptor == opt.Descriptor)
                {
                    if (!string.IsNullOrEmpty(opt.Value))
                    {
                        try
                        {
                            var filename = opt.Value;
                            var placeHolderTemplateText = File.ReadAllText(filename);
                            config.PlaceHolderTemplate = CreateTemplateEngine(placeHolderTemplateText);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex);
                        }
                    }
                    else
                        throw new Exception(string.Format("No value provided for option: '{0}'", opt.Name));
                    continue;
                }
                
                if (OptionDescriptorDefinitions.VariableSubstitutionOptionDescriptor == opt.Descriptor)
                {
                    if (!string.IsNullOrEmpty(opt.Value))
                    {
                        try
                        {
                            config.VariableSubstitution = Convert.ToBoolean(opt.Value);
                        }
                        catch
                        {
                            throw new Exception(string.Format("No boolean value provided for option: '{0}'", opt.Name));
                        }
                    }
                    else
                        throw new Exception(string.Format("No value provided for option: '{0}'", opt.Name));
                    continue;
                }
                
                if (OptionDescriptorDefinitions.FollowSymbolicLinksOptionDescriptor == opt.Descriptor)
                {
                    if (!string.IsNullOrEmpty(opt.Value))
                    {
                        try
                        {
                            config.FollowSymbolicLinks = Convert.ToBoolean(opt.Value);
                        }
                        catch
                        {
                            throw new Exception(string.Format("No boolean value provided for option: '{0}'", opt.Name));
                        }
                    }
                    else
                        throw new Exception(string.Format("No value provided for option: '{0}'", opt.Name));
                    continue;
                }
                
                if (OptionDescriptorDefinitions.VerboseOptionDescriptor == opt.Descriptor)
                {
                    config.Verbose = true;
                    continue;
                }
                
                if (OptionDescriptorDefinitions.ShortOptionDescriptor == opt.Descriptor)
                {
                    config.Short = true;
                    continue;
                }
                
                if (OptionDescriptorDefinitions.CleanOptionDescriptor == opt.Descriptor)
                {
                    config.CleanUp = true;
                    continue;
                }

                if (OptionDescriptorDefinitions.DryRunOptionDescriptor == opt.Descriptor)
                {
                    config.DryRun = true;
                    continue;
                }

                if (OptionDescriptorDefinitions.ExcludeOptionDescriptor == opt.Descriptor)
                {
                    var dirs = null != opt.Value ? opt.Value.Split(Path.PathSeparator) : new string[0];
                    var dirList = new List<string>(dirs.Length);
                    dirList.AddRange(dirs);
                    config.Exclude = dirList;
                    continue;
                }
                
                if (OptionDescriptorDefinitions.HelpOptionDescriptor == opt.Descriptor)
                {
                    config.Help = true;
                    continue;
                }
                
                if (OptionDescriptorDefinitions.CreateHookOptionDescriptor == opt.Descriptor)
                {
                    if (!string.IsNullOrEmpty(opt.Value))
                        config.CreateHookTemplate = CreateTemplateEngine(opt.Value);
                    else
                        throw new Exception(string.Format("No value provided for option: '{0}'", opt.Name));
                    continue;
                }
                
                if (OptionDescriptorDefinitions.DeleteHookOptionDescriptor == opt.Descriptor)
                {
                    if (!string.IsNullOrEmpty(opt.Value))
                        config.DeleteHookTemplate = CreateTemplateEngine(opt.Value);
                    else
                        throw new Exception(string.Format("No value provided for option: '{0}'", opt.Name));
                    continue;
                }
                
                if (OptionDescriptorDefinitions.ListOptionDescriptor == opt.Descriptor)
                {
                    config.Short = config.DryRun = config.CleanUp = true;
                    continue;
                }

//                if (OptionDescriptorDefinitions.SyncOptionDescriptor == opt.Descriptor)
//                {
//                    CleanUp = false;
//                    continue;
//                }

                // If we have a descriptor-less option assume it is the directory parameter.
                if (null == directory && OptionType.Short != opt.OptionType && null == opt.Descriptor)
                {
                    directory = opt.Value;
                    continue;
                }

                if (OptionType.NoOption == opt.OptionType)
                    throw new Exception(string.Format("Unknown parameter: '{0}'", opt.Value));

                throw new Exception(string.Format("Unknown option: '{0}' (value: '{1}')", opt.Name, opt.Value));
            }

            config.Directory = null != directory ? new DirectoryInfo(directory) : null;

            return config;
        }
        
        public static void Main(string[] args)
        {
            args = GetDecoratedArgs(args);

            try
            {
                var config = ParseConfiguration(args);
    
                ICommand cmd;

                if (config.Help)
                    cmd = new HelpCommand { Writer = Console.Out };
                else if (config.CleanUp)
                    cmd = new CleanCommand();
                else
                    cmd = new SyncCommand();
    
                cmd.Execute(config);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}
