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
        
        public char LeftPaddingChar { get; set; }
        public int MaxColumns { get; set; }
        public int LeftIndentFirstLine { get; set; }
        public int RightIndentFirstLine { get; set; }
        public int LeftIndentParagraph { get; set; }
        public int RightIndentParagraph { get; set; }
        public int LinesBeforeParagraph { get; set; }
        public int LinesAfterParagraph { get; set; }
        
        public TextLayout()
        {
            LeftPaddingChar = ' ';
            MaxColumns = int.MaxValue;
            LeftIndentFirstLine = 0;
            RightIndentFirstLine = 0;
            LeftIndentParagraph = 0;
            RightIndentParagraph = 0;
            LinesBeforeParagraph = 0;
            LinesAfterParagraph = 0;
        }
        
        public string Layout(string text)
        {
            string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            
            var builder = new StringBuilder();
            
            for (var i = 0; i < LinesBeforeParagraph; i++)
                builder.Append(Environment.NewLine);
            
            foreach (var line in lines)
                LayoutLine(builder, line, MaxColumns, LeftIndentFirstLine, RightIndentFirstLine, LeftIndentParagraph, RightIndentParagraph, LeftPaddingChar);
            
            for (var i = 0; i < LinesAfterParagraph; i++)
                builder.Append(Environment.NewLine);
            
            return builder.ToString();
        }
        
        private static string LayoutLine(StringBuilder builder, string line, int maxColumns, int leftIndentFirstLine, int rightIndentFirstLine, int leftIndentParagraph, int rightIndentParagraph, char leftPaddingChar)
        {
            line = line.Trim(WhiteSpaces);
            var length = 0;
            var leftIndent = leftIndentFirstLine;
            var rightIndent = rightIndentFirstLine;
            while ((length = Math.Min(line.Length, maxColumns - leftIndent - rightIndent)) < line.Length)
            {
                // Search for  a whitespace to the left.
                int breakIndex = breakIndex = line.LastIndexOfAny(WhiteSpaces, length);
                if (-1 != breakIndex)
                {
                    length = breakIndex + 1;
                }
                
                var subLine = line.Substring(0, length).Trim(WhiteSpaces);
                builder.Append(leftPaddingChar, leftIndent).AppendLine(subLine);
                
                line = line.Substring(length).Trim(WhiteSpaces);
                leftIndent = leftIndentParagraph;
                rightIndent = rightIndentParagraph;
            }
            builder.Append(leftPaddingChar, leftIndent).AppendLine(line);
            
            return builder.ToString();
        }       
    }
}
