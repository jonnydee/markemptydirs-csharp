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

namespace DJ.Util.IO
{
    public static class Logger
    {
        public enum LogType
        {
            Debug,
            Info,
            Error,
            Warn
        }

        public static void Log(Exception ex)
        {
            Log(LogType.Error, ex.Message);
        }
        
        public static void Log(LogType type, string message)
        {
            Log(type, message, false);
        }
        
        public static void Log(LogType type, string message, bool shortMessage)
        {
            switch (type)
            {
                case LogType.Debug:
                    Console.Out.WriteLine((shortMessage ? "" : "DEBUG: ") + message);
                    break;
                case LogType.Info:
                    Console.Out.WriteLine((shortMessage ? "" : "INFO: ") + message);
                    break;
                case LogType.Error:
                    Console.Error.WriteLine((shortMessage ? "" : "ERROR: ") + message);
                    break;
                case LogType.Warn:
                    Console.Error.WriteLine((shortMessage ? "" : "WARNING: ") + message);
                    break;
            }
            
        }        
    }
}
