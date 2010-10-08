//  Copyright (C) 2009-2010 by Markus Raufer
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

using NUnit.Framework;

namespace DR.Text
{
    [TestFixture]
    public class TestTextLayout
    {
        private TextLayout _layout;
        
        [SetUp]
        public void SetUp()
        {
            _layout = new TextLayout
            {
                MinParagraphWidth = 5,
                MaxColumns = 15,
                LeftPrefixFirstLine = "#+",
                RightIndentFirstLine = 1,
                LeftPrefixParagraph = "*+#-",
                RightIndentParagraph = 3,
                LinesBeforeParagraph = 1,
                LinesAfterParagraph = 2,
            };
        }
        
        [TearDown]
        public void TearDown()
        {
            _layout = null;
        }

        private void Check(string text, params string[] lines)
        {
            Console.WriteLine(string.Format("TEXT: '{0}'", text));
            
            var builder = new StringBuilder();
            for (var i = 0; i < _layout.LinesBeforeParagraph; i++)
                builder.Append(Environment.NewLine);
            builder.AppendLine(string.Join(Environment.NewLine, lines));
            for (var i = 0; i < _layout.LinesAfterParagraph; i++)
                builder.Append(Environment.NewLine);
            var expectedResult = builder.ToString();
            Console.WriteLine(string.Format("EXPECTED:{1}{0}", expectedResult, Environment.NewLine));
            
            var result = _layout.Layout(text).ToString();
            Console.WriteLine(string.Format("RESULT:{1}{0}", result, Environment.NewLine));
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        public void TestOneLine1()
        {
            Check("12345678901234567890123456789012",
                "#+123456789012",
                "*+#-34567890",
                "*+#-12345678",
                "*+#-9012"
            );
        }

        [Test]
        public void TestOneLine2()
        {
            Check("1234567 9012 45678901 3 56 890 2",
                "#+1234567 9012",
                "*+#-45678901",
                "*+#-3 56 890",
                "*+#-2"
            );
        }

        [Test]
        public void TestOneLine3()
        {
            Check(" 234567   123  678901 3 56 890 2              ",
                "#+234567   123",
                "*+#-678901 3",
                "*+#-56 890 2"
            );
        }

        [Test]
        public void TestOneLine4()
        {
            Check(" 234567  0123  678901 3 56 890 2              ",
                "#+234567  0123",
                "*+#-678901 3",
                "*+#-56 890 2"
            );
        }

        [Test]
        public void TestOneLine5()
        {
            Check(" 234567  01234  78901 3 56 890 2              ",
                "#+234567",
                "*+#-01234",
                "*+#-78901 3",
                "*+#-56 890 2"
            );
        }

        [Test]
        public void TestMultipleLines1()
        {
            Check("123456789012345678901234567890" + Environment.NewLine
                + " 234567  01234  78901 3 56 890 2              ",
                "#+123456789012",
                "*+#-34567890",
                "*+#-12345678",
                "*+#-90",
                "#+234567",
                "*+#-01234",
                "*+#-78901 3",
                "*+#-56 890 2"
            );
        }
    }
}
