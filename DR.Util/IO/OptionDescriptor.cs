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

namespace DR.Util.IO
{
    
    public class OptionDescriptor
    {
        public string[] LongNames { set; get; }
        public char[] ShortNames { set; get; }
        public string ShortDescription { set; get; }
        public string LongDescription { set; get; }
        public bool CanHaveValue { set; get; }
        public bool MandatoryValue {set; get; }
        public string ValueIdentifier { set; get; }
        public bool MandatoryOption { set; get; }

        public override string ToString ()
        {
            return string.Format("[OptionDescriptor: LongNames={0}, ShortNames={1}, ShortDescription={2}, LongDescription={3}, CanHaveValue={4}, MandatoryValue={5}, ValueIdentifier={6}, MandatoryOption={7}]", LongNames, ShortNames, ShortDescription, LongDescription, CanHaveValue, MandatoryValue, ValueIdentifier, MandatoryOption);
        }
    }

}
