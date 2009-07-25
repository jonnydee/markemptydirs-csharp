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

using DJ.Util.IO;

namespace DJ.App.MarkEmptyDirs
{
        
    public static class OptionDescriptorDefinitions
    {
        public static readonly OptionDescriptor DryRunOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "dry-run", "dry" },
            ShortNames = new[] { 'd' },
        };
        public static readonly OptionDescriptor ShortOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "short" },
            ShortNames = new[] { 'r' },
        };
        public static readonly OptionDescriptor VerboseOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "verbose" },
            ShortNames = new[] { 'v' },
        };
        
        public static readonly OptionDescriptor CleanOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "clean" },
            ShortNames = new[] { 'c' },
        };
        public static readonly OptionDescriptor HelpOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "help" },
            ShortNames = new[] { 'h' },
        };
        public static readonly OptionDescriptor ListOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "list" },
            ShortNames = new[] { 'l' },
        };
        public static readonly OptionDescriptor SyncOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "sync" },
            ShortNames = new[] { 's' },
        };

        public static readonly OptionDescriptor ExcludeOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "exclude" },
            ShortNames = new[] { 'x' },
            CanHaveValue = true,
            MandatoryValue = true,
        };
        public static readonly OptionDescriptor PlaceHolderOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "place-holder", "keep-file" },
            ShortNames = new[] { 'p', 'k' },
            CanHaveValue = true,
            MandatoryValue = true,
        };
        public static readonly OptionDescriptor TextOptionDescriptor = new OptionDescriptor
        {
            LongNames = new[] { "text", "content" },
            ShortNames = new[] { 't' },
            CanHaveValue = true,
            MandatoryValue = true,
        };

        public static readonly OptionDescriptor[] OptionDescriptors = new[]
        {
            DryRunOptionDescriptor,
            ShortOptionDescriptor,
            VerboseOptionDescriptor,

            CleanOptionDescriptor,
            HelpOptionDescriptor,
            ListOptionDescriptor,
            SyncOptionDescriptor,
            
            ExcludeOptionDescriptor,
            PlaceHolderOptionDescriptor,
            TextOptionDescriptor
        };

    }
}
