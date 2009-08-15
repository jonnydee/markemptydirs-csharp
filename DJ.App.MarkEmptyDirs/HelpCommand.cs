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
using DJ.Util.Template;

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

        void PrintUsage()
        {
            var cmdFileName = GetCommandFileName();
            var cmdFullName = cmdFileName[0];
            var cmdName = cmdFileName[1];

            var usage = new StringBuilder();
            
            usage.Append('\n');
            usage.Append("***\n");
            usage.Append("*** ").Append(cmdName).Append(" ").Append(MainClass.Version).Append(" -- ").Append(MainClass.Copyright).Append('\n');
            usage.Append("***\n");
            usage.Append("*** Project Site: ").Append(MainClass.ProjectUrl).Append('\n');
            usage.Append("***\n");
            usage.Append("*** This program is licensed under the GNU General Public License, Version 3.\n");
            usage.Append("***\n");
            usage.Append('\n');
            
            usage.Append("USAGE:  ").Append(cmdFullName).Append(' ');
            foreach (var descr in OptionDescriptorDefinitions.OptionDescriptors)
            {
                string usageForm = GetUsageForm(descr);
                usage.Append(usageForm).Append(' ');
            }
            usage.Append("<directory>\n\n");
            usage.Append("Option descriptions:\n");
            usage.Append(GetDescription(OptionDescriptorDefinitions.OptionDescriptors));
            usage.Append('\n');

            var envValue = MainClass.GetSettingsInEnvironmentVariable();
            if (null != envValue)
            {
                if (string.Empty == envValue)
                    envValue = "<VARIABLE EMPTY>";
            }
            else
                envValue = "<VARIABLE UNDEFINED>";
            usage.AppendFormat("Defaults set in environment variable '{0}':\n  {1}\n\n", MainClass.SettingsEnvironmentVariable, envValue);

            usage.Append("Available template variables for placeholder files:\n");
            usage.Append(GetDescription(MainClass.CreateTemplateEngine(string.Empty).ListTemplateVariables()));
            
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

            var description = new StringBuilder();
            foreach (var descr in optionDescriptions)
            {
                description.AppendFormat("  {0,-" + maxShortNameColumnWidth + "}  {1,-" + maxLongNameColumnWidth + "}  {2}\n", descr);
            }

            return description.ToString();
        }

        public string GetDescription(List<TemplateVariable> variables)
        {
            var variableNames = new List<string>();
            var maxNameColumnWidth = 0;
            foreach (var variable in variables)
            {
                var variableName = variable.ToString();
                variableNames.Add(variableName);
                maxNameColumnWidth = Math.Max(maxNameColumnWidth, variableName.Length);
            }
            
            var description = new StringBuilder();
            for (var i = 0; i < variables.Count; i++)
            {
                description.AppendFormat("  {0,-" + maxNameColumnWidth + "}  {1}\n", variableNames[i], variables[i].Description);
                if (null != variables[i].ArgumentDescription)
                    description.AppendFormat("  {0,-" + maxNameColumnWidth + "}      <{1}> : {2}", " ", variables[i].ArgumentIdentifier, variables[i].ArgumentDescription);
                description.AppendLine();
            }

            return description.ToString();
        }
        
        public void Execute(Configuration config)
        {
            PrintUsage();
        }
    }

}
