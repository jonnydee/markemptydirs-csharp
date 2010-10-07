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

using DR.IO;
using DR.Template;
using DR.Text;

namespace DJ.App.MarkEmptyDirs
{
    
    class HelpCommand : ICommand
    {
        public HelpCommand()
        {
        }

        public TextWriter Writer { set; get; }
        
        static string[] GetCommandFileName()
        {
            try
            {
                var cmdFilePath = Environment.GetCommandLineArgs()[0];
                
                var cmdNameWithExtension = Path.GetFileName(cmdFilePath);
                var cmdName = Path.GetFileNameWithoutExtension(cmdFilePath);
                var cmdExtension = Path.GetExtension(cmdFilePath);
                return new[] { cmdNameWithExtension, cmdName, cmdExtension };
            }
            catch
            {
                return new[] { Path.ChangeExtension(MainClass.StandardCmdName, MainClass.StandardCmdExtension), MainClass.StandardCmdName, MainClass.StandardCmdExtension };
            }
        }

        int MaxColumns
        {
            get
            {
                return Console.BufferWidth > 0 ? Console.BufferWidth : Console.LargestWindowWidth;
            }
        }
        
        void PrintUsage()
        {
            var cmdFileName = GetCommandFileName();
            var cmdFullName = cmdFileName[0];
            var cmdName = cmdFileName[1];

            var usage = new StringBuilder();

            #region Print Header
            {
                var header = new StringBuilder();
                header.Append(cmdName).Append(" ").Append(MainClass.Version).Append(" -- ").Append(MainClass.Copyright).AppendLine();
                header.AppendLine();
                header.Append("Project Site: ").Append(MainClass.ProjectUrl).AppendLine();
                header.AppendLine();
                header.Append("This program is licensed under the GNU General Public License, Version 3.").AppendLine();
                
                var headerLayout = new TextLayout()
                {
                    LinesBeforeParagraph = 1,
                    LeftPrefixFirstLine = new string(' ', 4),
                    LeftPrefixParagraph = new string(' ', 4),
                    MaxColumns = MaxColumns,
                    LinesAfterParagraph = 1,
                };
                headerLayout.Layout(header.ToString(), usage);
            }
            #endregion
            
            #region Command line with options
            {
                var cmdLine = new StringBuilder();
                cmdLine.Append("USAGE:  ").Append(cmdFullName).Append(' ');
                foreach (var descr in OptionDescriptorDefinitions.OptionDescriptors)
                {
                    string usageForm = GetUsageForm(descr);
                    cmdLine.Append(usageForm).Append(' ');
                }
                cmdLine.Append("<directory>").AppendLine();
                
                var cmdLineLayout = new TextLayout()
                {
                    LeftPrefixParagraph = new string(' ', 8 + cmdFullName.Length + 1),
                    MaxColumns = MaxColumns,
                    LinesAfterParagraph = 1,
                };
                cmdLineLayout.Layout(cmdLine.ToString(), usage);
            }
            #endregion
            
            #region Option descriptions
            {
                usage.Append("Option descriptions:\n");
                usage.Append(GetDescription(OptionDescriptorDefinitions.OptionDescriptors));
                usage.AppendLine();
            }
            #endregion

            #region Settings environment variable
            {
                var envValue = MainClass.GetSettingsInEnvironmentVariable();
                if (null != envValue)
                {
                    if (string.Empty == envValue)
                        envValue = "<VARIABLE EMPTY>";
                }
                else
                    envValue = "<VARIABLE UNDEFINED>";
                
                var settingsEnvVarLayout = new TextLayout()
                {
                    MaxColumns = MaxColumns,
                };
                
                var caption = string.Format("Defaults set in environment variable '{0}':", MainClass.SettingsEnvironmentVariable);
                settingsEnvVarLayout.Layout(caption, usage);
                
                settingsEnvVarLayout.LeftPrefixFirstLine = settingsEnvVarLayout.LeftPrefixParagraph = new string(' ', 2);
                settingsEnvVarLayout.LinesAfterParagraph = 1;
                settingsEnvVarLayout.Layout(envValue, usage);
            }
            #endregion

            #region Template variables
            {
                var templateVarLayout = new TextLayout()
                {
                    MaxColumns = MaxColumns,
                };
                
                templateVarLayout.Layout("Available template variables for placeholder files and corresponding creation/deletion hooks:", usage);
                
                usage.Append(GetDescription(MainClass.CreateTemplateEngine(string.Empty).ListTemplateVariables()));
            }
            #endregion
            
            Writer.WriteLine(usage.ToString());
        }

        public string GetUsageForm(OptionDescriptor descr)
        {
            string optionName = null;
            if (null != descr.ShortNames && descr.ShortNames.Length > 0)
            {
                optionName = "-" + descr.ShortNames[0].ToString();
                if (descr.CanHaveValue)
                {
                    optionName += " ";
                    if (descr.MandatoryValue)
                        optionName += string.Format("<{0}>", descr.ValueIdentifier);
                    else
                        optionName += string.Format("[{0}]", descr.ValueIdentifier);
                }
            }
            else
            {
                optionName = "--" + descr.LongNames[0];
                if (descr.CanHaveValue)
                {
                    optionName += "=";
                    if (descr.MandatoryValue)
                        optionName += string.Format("[{0}]", descr.ValueIdentifier);
                    else
                        optionName += string.Format("<{0}>", descr.ValueIdentifier);
                }
            }

            if (descr.MandatoryOption)
                optionName = "<" + optionName + ">";
            else
                optionName = "[" + optionName + "]";
            
            return optionName;
        }

        public string[] GetDescriptionColumns(OptionDescriptor descr)
        {
            var shortNameColumn = new StringBuilder();
            if (null != descr.ShortNames && descr.ShortNames.Length > 0)
            {
                shortNameColumn.Append('-').Append(descr.ShortNames[0]);
                for (int i = 1; i < descr.ShortNames.Length; i++)
                    shortNameColumn.Append("|").Append(descr.ShortNames[i]);
            }

            var longNameColumn = new StringBuilder();
            if (null != descr.LongNames && descr.LongNames.Length > 0)
            {
                longNameColumn.Append("--").Append(descr.LongNames[0]);
                for (int i = 1; i < descr.LongNames.Length; i++)
                    longNameColumn.Append("|").Append(descr.LongNames[i]);
            }

            var shortNameColumnString = shortNameColumn.ToString();
            var longNameColumnString = longNameColumn.ToString();
            var shortDescription = descr.ShortDescription ?? string.Empty;

            var columns = new[] { shortNameColumnString, longNameColumnString, shortDescription };

            return columns;
        }

        public string GetDescription(params OptionDescriptor[] descriptors)
        {
            var optionDescriptions = new List<string[]>();
            int maxShortNameColumnWidth = 0;
            int maxLongNameColumnWidth = 0;
            foreach (var descr in descriptors)
            {
                var cols = GetDescriptionColumns(descr);
                maxShortNameColumnWidth = Math.Max(maxShortNameColumnWidth, cols[0].Length);
                maxLongNameColumnWidth = Math.Max(maxLongNameColumnWidth, cols[1].Length);
                optionDescriptions.Add(cols);
            }

            var descriptionLayout = new TextLayout
            {
                LeftPrefixFirstLine = new string(' ', 2),
                LeftPrefixParagraph = new string(' ', 2 + maxShortNameColumnWidth + 2 + maxLongNameColumnWidth + 2),
                MaxColumns = MaxColumns,
            };
            
            var description = new StringBuilder();
            foreach (var descr in optionDescriptions)
            {
                var optionDescription = string.Format("{0,-" + maxShortNameColumnWidth + "}  {1,-" + maxLongNameColumnWidth + "}  {2}", descr);
                descriptionLayout.Layout(optionDescription, description);
            }

            return description.ToString();
        }

        public string GetDescription(List<TemplateVariable> variables)
        {
            var varDescriptionLayout1 = new TextLayout
            {
                LeftPrefixFirstLine = new string(' ', 2),
                LeftPrefixParagraph = new string(' ', 2),
                MaxColumns = MaxColumns,
            };
            
            var varDescriptionLayout2 = new TextLayout
            {
                LeftPrefixFirstLine = new string(' ', 6),
                LeftPrefixParagraph = new string(' ', 6),
                MaxColumns = MaxColumns,
            };
            
            if (variables.Count == 0)
                return varDescriptionLayout1.Layout("<NO TEMPLATE VARIABLES AVAILABLE>").ToString();
            
            var description = new StringBuilder();
            for (var i = 0; i < variables.Count; i++)
            {
                var varDescription = string.Format("{0} --> {1}", variables[i].ToString(), variables[i].Description);
                varDescriptionLayout1.Layout(varDescription, description);
                
                if (null != variables[i].ArgumentDescription)
                    varDescriptionLayout2.Layout(variables[i].ArgumentDescription, description);
                
                if (i < variables.Count - 1)
                    description.AppendLine();
            }

            return description.ToString();
        }
        
        public int Execute(Configuration config)
        {
            PrintUsage();
            
            return 0;
        }
    }

}
