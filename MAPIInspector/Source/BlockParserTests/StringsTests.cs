using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Parser.Tests
{
    [TestClass]
    public class StringsTests
    {
        [TestMethod]
        public void TrimWhitespace_RemovesLeadingAndTrailingWhitespace()
        {
            Assert.AreEqual("abc", strings.TrimWhitespace("  abc  "));
            Assert.AreEqual("abc", strings.TrimWhitespace("\t\nabc\r\n"));
            Assert.AreEqual("abc", strings.TrimWhitespace("\0abc\0"));
        }

        [TestMethod]
        public void TrimWhitespace_EmptyOrWhitespaceOnly_ReturnsEmpty()
        {
            Assert.AreEqual(string.Empty, strings.TrimWhitespace(""));
            Assert.AreEqual(string.Empty, strings.TrimWhitespace("   "));
            Assert.AreEqual(string.Empty, strings.TrimWhitespace("\t\r\n"));
            Assert.AreEqual(string.Empty, strings.TrimWhitespace("\0\0"));
        }

        [TestMethod]
        public void TrimWhitespace_NoWhitespace_ReturnsOriginal()
        {
            Assert.AreEqual("abc", strings.TrimWhitespace("abc"));
        }

        [TestMethod]
        public void TrimWhitespace_Null_ReturnsEmpty()
        {
            Assert.AreEqual(string.Empty, strings.TrimWhitespace(null));
        }

        [TestMethod]
        public void Test_StripCharacter()
        {
            Assert.AreEqual("bcd", strings.StripCharacter("abcad", 'a'));
            Assert.AreEqual("hello", strings.StripCharacter("hello", 'x'));
            Assert.AreEqual("", strings.StripCharacter("", 'a'));
            Assert.AreEqual("", strings.StripCharacter("aaaa", 'a'));
        }

        [TestMethod]
        public void Test_InvalidCharacter()
        {
            Assert.IsTrue(strings.InvalidCharacter(0x81, false));
            Assert.IsFalse(strings.InvalidCharacter(0x20, false));
            Assert.IsFalse(strings.InvalidCharacter(0x09, true)); // Tab allowed in multiline
            Assert.IsTrue(strings.InvalidCharacter(0x09, false)); // Tab not allowed in single line
        }

        [TestMethod]
        public void Test_BinToTextStringW()
        {
            var bin = new List<byte>(System.Text.Encoding.Unicode.GetBytes("abc"));
            Assert.AreEqual("abc", strings.BinToTextStringW(bin, false));

            var binWithInvalid = new List<byte>(System.Text.Encoding.Unicode.GetBytes("a\u0081c"));
            Assert.AreEqual("a.c", strings.BinToTextStringW(binWithInvalid, false));

            var binWithWhiteSpace = new List<byte>(System.Text.Encoding.Unicode.GetBytes("a\r\nb\rc\n\td"));
            Assert.AreEqual("a..b.c..d", strings.BinToTextStringW(binWithWhiteSpace, false));
            Assert.AreEqual("a\r\nb\rc\n\td", strings.BinToTextStringW(binWithWhiteSpace, true));

            Assert.AreEqual("", strings.BinToTextStringW(new List<byte>(), false));

            var mystringW = "mystring";
            var myStringWvector = new List<byte>(System.Text.Encoding.Unicode.GetBytes(mystringW));
            var vector_abcdW = new List<byte> { 0x61, 0, 0x62, 0, 0x63, 0, 0x64, 0 };
            var vector_abNULLdW = new List<byte> { 0x61, 0, 0x62, 0, 0x00, 0, 0x64, 0 };
            var vector_tabcrlfW = new List<byte> { 0x9, 0, 0xa, 0, 0xd, 0 };

            Assert.AreEqual("", strings.BinToTextStringW(null, true));
            Assert.AreEqual("", strings.BinToTextStringW(null, false));
            Assert.AreEqual(mystringW, strings.BinToTextStringW(myStringWvector, false));
            Assert.AreEqual("abcd", strings.BinToTextStringW(vector_abcdW, false));
            Assert.AreEqual("ab.d", strings.BinToTextStringW(vector_abNULLdW, false));
            Assert.AreEqual("\t\n\r", strings.BinToTextStringW(vector_tabcrlfW, true));
            Assert.AreEqual("...", strings.BinToTextStringW(vector_tabcrlfW, false));
        }

        [TestMethod]
        public void Test_BinToTextStringA()
        {
            var bin = new List<byte>(System.Text.Encoding.ASCII.GetBytes("abc"));
            Assert.AreEqual("abc", strings.BinToTextStringA(bin, false));

            var binWithInvalid = new List<byte> { 97, 0x81, 99 }; // a, invalid, c
            Assert.AreEqual("a.c", strings.BinToTextStringA(binWithInvalid, false));

            Assert.AreEqual("", strings.BinToTextStringA(new List<byte>(), false));

            var mystringA = "mystring";
            var myStringAvector = new List<byte>(System.Text.Encoding.ASCII.GetBytes(mystringA));
            var vector_abcdA = new List<byte> { 0x61, 0x62, 0x63, 0x64 };
            var vector_abNULLdA = new List<byte> { 0x61, 0x62, 0x00, 0x64 };
            var vector_tabcrlfA = new List<byte> { 0x9, 0xa, 0xd };

            Assert.AreEqual("", strings.BinToTextStringA(null, true));
            Assert.AreEqual("", strings.BinToTextStringA(null, false));
            Assert.AreEqual(mystringA, strings.BinToTextStringA(myStringAvector, false));
            Assert.AreEqual("abcd", strings.BinToTextStringA(vector_abcdA, false));
            Assert.AreEqual("ab.d", strings.BinToTextStringA(vector_abNULLdA, false));
            Assert.AreEqual("\t\n\r", strings.BinToTextStringA(vector_tabcrlfA, true));
            Assert.AreEqual("...", strings.BinToTextStringA(vector_tabcrlfA, false));
            Assert.AreEqual(mystringA, strings.BinToTextStringA(myStringAvector, true));
        }

        [TestMethod]
        public void Test_BinToHexString()
        {
            var bin = new List<byte> { 0xAB, 0xCD, 0xEF };
            Assert.AreEqual("ABCDEF", strings.BinToHexString(bin));
            Assert.AreEqual("cb: 3 lpb: ABCDEF", strings.BinToHexString(bin, true));
            Assert.AreEqual("NULL", strings.BinToHexString(new List<byte>()));
            Assert.AreEqual("cb: 0 lpb: NULL", strings.BinToHexString(new List<byte>(), true));
        }

        [TestMethod]
        public void Test_RemoveInvalidCharacters()
        {
            Assert.AreEqual("abc", strings.RemoveInvalidCharacters("abc"));
            Assert.AreEqual("a.c", strings.RemoveInvalidCharacters("a\u0081c"));
            Assert.AreEqual("", strings.RemoveInvalidCharacters(""));
            Assert.AreEqual("a.c\0", strings.RemoveInvalidCharacters("a\u0081c\0"));
        }
    }
}
