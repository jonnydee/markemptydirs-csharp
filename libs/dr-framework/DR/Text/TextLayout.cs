//  Copyright (C) 2010 by Copyright (c) 2009 by Johann Duscher (alias Jonny Dee)
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
using System.Text;

namespace DR.Text
{
    public class TextLayout
    {
        private static readonly char[] WhiteSpaces = { ' ', '\t' };
        
        public int MinParagraphWidth { get; set; }
        public int MaxColumns { get; set; }
        public string LeftPrefixFirstLine { get; set; }
        public int RightIndentFirstLine { get; set; }
        public string LeftPrefixParagraph { get; set; }
        public int RightIndentParagraph { get; set; }
        public int LinesBeforeParagraph { get; set; }
        public int LinesAfterParagraph { get; set; }
        
        public TextLayout()
        {
            MinParagraphWidth = 10;
            MaxColumns = int.MaxValue;
            LeftPrefixFirstLine = string.Empty;
            RightIndentFirstLine = 0;
            LeftPrefixParagraph = string.Empty;
            RightIndentParagraph = 0;
            LinesBeforeParagraph = 0;
            LinesAfterParagraph = 0;
        }
        
        public StringBuilder Layout(string text)
        {
            return Layout(text, null);
        }
        
        public StringBuilder Layout(string text, StringBuilder builder)
        {
            if (null == builder)
                builder = new StringBuilder();
            
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            
            for (var i = 0; i < LinesBeforeParagraph; i++)
                builder.Append(Environment.NewLine);
            
            foreach (var line in lines)
                LayoutLine(builder, line, MinParagraphWidth, MaxColumns, LeftPrefixFirstLine, RightIndentFirstLine, LeftPrefixParagraph, RightIndentParagraph);
            
            for (var i = 0; i < LinesAfterParagraph; i++)
                builder.Append(Environment.NewLine);
            
            return builder;
        }
        
        private static string LayoutLine(StringBuilder builder, string line, int minParagraphWidth, int maxColumns, string leftPrefixFirstLine, int rightIndentFirstLine, string leftPrefixParagraph, int rightIndentParagraph)
        {
            line = line.Trim(WhiteSpaces);
            var leftPrefix = leftPrefixFirstLine;
            var rightIndent = rightIndentFirstLine;
            while (true)
            {
                var length = maxColumns - leftPrefix.Length - rightIndent;
                // Do not wrap words to new lines if 'minColumns' width is reached,
                // or if the line's length fits within the calculated 'length'.
                if (length < minParagraphWidth || line.Length <= length)
                    break;
                
                // Search for  a whitespace to the left.
                var breakIndex = line.LastIndexOfAny(WhiteSpaces, length);
                if (-1 != breakIndex)
                {
                    length = breakIndex + 1;
                }
                
                var subLine = line.Substring(0, length).Trim(WhiteSpaces);
                builder.Append(leftPrefix).AppendLine(subLine);
                
                line = line.Substring(length).Trim(WhiteSpaces);
                leftPrefix = leftPrefixParagraph;
                rightIndent = rightIndentParagraph;
            }
            builder.Append(leftPrefix).AppendLine(line);
            
            return builder.ToString();
        }       
    }
}
