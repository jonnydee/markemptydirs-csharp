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

using DJ.Util.Template;

namespace DJ.App.MarkEmptyDirs
{
    
    public class Configuration
    {        
        public Configuration()
        {
        }

        public bool CleanUp { set; get; }

        public DirectoryInfo Directory { set; get; }

        public bool DryRun { set; get; }

        public List<string> Exclude { set; get; }

        public bool Help { set; get; }

        public string PlaceHolderName { set; get; }

        public TemplateEngine PlaceHolderTemplate { set; get; }

        public bool Short { set; get; }

        public bool Verbose { set; get; }
    }
}
