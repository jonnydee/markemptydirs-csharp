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
using System.IO;

using DJ.Util.IO;

namespace DJ.App.MarkEmptyDirs
{
        
    public static class OptionDescriptorDefinitions
    {
        public static readonly OptionDescriptor DryRunOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "dry-run", "dry" },
            ShortNames = new[] { 'd' },
            ShortDescription = "simulate command execution without any side effects",
        };
        public static readonly OptionDescriptor ShortOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "short" },
            ShortNames = new[] { 's' },
            ShortDescription = "output short verbose messages",
        };
        public static readonly OptionDescriptor VerboseOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "verbose" },
            ShortNames = new[] { 'v' },
            ShortDescription = "output verbose messages",
        };
        
        public static readonly OptionDescriptor CleanOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "clean" },
            ShortNames = new[] { 'c' },
            ShortDescription = "delete all placeholder files",
        };
        public static readonly OptionDescriptor HelpOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "help" },
            ShortNames = new[] { 'h' },
            ShortDescription = "print help information"
        };
        public static readonly OptionDescriptor CreateHookOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "create-hook" },
            ShortNames = new[] { 'a' },
            ShortDescription = string.Format("invoke command after placeholder creation (use §placeholder§ as placeholder name)", new PlaceHolderVariable().ToString()),
            CanHaveValue = true,
            MandatoryValue = true,
            ValueIdentifier = "create-hook-command",
        };
        public static readonly OptionDescriptor DeleteHookOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "delete-hook" },
            ShortNames = new[] { 'r' },
            ShortDescription = string.Format("invoke command before placeholder deletion (use §placeholder§ as placeholder name)", new PlaceHolderVariable().ToString()),
            CanHaveValue = true,
            MandatoryValue = true,
            ValueIdentifier = "delete-hook-command",
        };
        public static readonly OptionDescriptor ListOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "list" },
            ShortNames = new[] { 'l' },
            ShortDescription = "list all placeholder files",
        };
//        public static readonly OptionDescriptor SyncOptionDescriptor = new OptionDescriptor
//        {
//            LongNames = new[] { "sync" },
//            ShortNames = new[] { 'y' },
//            ShortDescription = "create and/or remove placeholder files",
//        };

        public static readonly OptionDescriptor ExcludeOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "exclude" },
            ShortNames = new[] { 'x' },
            ShortDescription = string.Format("skip excluded dirs (default is '{0}')", string.Join(Path.PathSeparator.ToString(), MainClass.StandardExcludedDirs)),
            CanHaveValue = true,
            MandatoryValue = false,
            ValueIdentifier = "list-of-dirnames",
        };
        public static readonly OptionDescriptor PlaceHolderOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "place-holder" },
            ShortNames = new[] { 'p' },
            ShortDescription = string.Format("use another name for placeholder files (default is '{0}')", MainClass.StandardPlaceHolderName),
            CanHaveValue = true,
            MandatoryValue = true,
            ValueIdentifier = "filename",
        };
        public static readonly OptionDescriptor TextOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "text" },
            ShortNames = new[] { 't' },
            ShortDescription = "create placeholder files with the specified text as content",
            CanHaveValue = true,
            MandatoryValue = true,
            ValueIdentifier = "placeholder-text",
        };
        public static readonly OptionDescriptor PlaceHolderFileOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "file" },
            ShortNames = new[] { 'f' },
            ShortDescription = "create placeholder files using the specified template file as content",
            CanHaveValue = true,
            MandatoryValue = true,
            ValueIdentifier = "placeholder-file",
        };
        public static readonly OptionDescriptor VariableSubstitutionOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "subst" },
            ShortNames = new[] { 'b' },
            ShortDescription = string.Format("use variable subsitution (default is '{0}')", MainClass.StandardVariableSubstitution.ToString().ToLower()),
            CanHaveValue = true,
            MandatoryValue = true,
            ValueIdentifier = "true|false",
        };

        public static readonly OptionDescriptor[] OptionDescriptors = new[]
        {
            DryRunOptionDescriptor,
            ShortOptionDescriptor,
            VerboseOptionDescriptor,

            CleanOptionDescriptor,
            HelpOptionDescriptor,
            CreateHookOptionDescriptor,
            DeleteHookOptionDescriptor,
            ListOptionDescriptor,
//            SyncOptionDescriptor,
            
            ExcludeOptionDescriptor,
            PlaceHolderOptionDescriptor,
            TextOptionDescriptor,
            PlaceHolderFileOptionDescriptor,
            VariableSubstitutionOptionDescriptor,
        };

    }
}
