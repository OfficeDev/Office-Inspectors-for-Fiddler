using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BlockParser.Tests
{
    [TestClass]
    public class StringsTests
    {
        [TestMethod]
        public void TrimWhitespace_RemovesLeadingAndTrailingWhitespace()
        {
            Assert.AreEqual("abc", Strings.TrimWhitespace("  abc  "));
            Assert.AreEqual("abc", Strings.TrimWhitespace("\t\nabc\r\n"));
            Assert.AreEqual("abc", Strings.TrimWhitespace("\0abc\0"));
        }

        [TestMethod]
        public void TrimWhitespace_EmptyOrWhitespaceOnly_ReturnsEmpty()
        {
            Assert.AreEqual(string.Empty, Strings.TrimWhitespace(""));
            Assert.AreEqual(string.Empty, Strings.TrimWhitespace("   "));
            Assert.AreEqual(string.Empty, Strings.TrimWhitespace("\t\r\n"));
            Assert.AreEqual(string.Empty, Strings.TrimWhitespace("\0\0"));
        }

        [TestMethod]
        public void TrimWhitespace_NoWhitespace_ReturnsOriginal()
        {
            Assert.AreEqual("abc", Strings.TrimWhitespace("abc"));
        }

        [TestMethod]
        public void TrimWhitespace_Null_ReturnsEmpty()
        {
            Assert.AreEqual(string.Empty, Strings.TrimWhitespace(null));
        }

        [TestMethod]
        public void Test_StripCharacter()
        {
            Assert.AreEqual("bcd", Strings.StripCharacter("abcad", 'a'));
            Assert.AreEqual("hello", Strings.StripCharacter("hello", 'x'));
            Assert.AreEqual("", Strings.StripCharacter("", 'a'));
            Assert.AreEqual("", Strings.StripCharacter("aaaa", 'a'));
        }

        [TestMethod]
        public void Test_InvalidCharacter()
        {
            Assert.IsTrue(Strings.InvalidCharacter(0x81, false));
            Assert.IsFalse(Strings.InvalidCharacter(0x20, false));
            Assert.IsFalse(Strings.InvalidCharacter(0x09, true)); // Tab allowed in multiline
            Assert.IsTrue(Strings.InvalidCharacter(0x09, false)); // Tab not allowed in single line
        }

        [TestMethod]
        public void Test_BinToTextStringW()
        {
            var bin = new List<byte>(System.Text.Encoding.Unicode.GetBytes("abc"));
            Assert.AreEqual("abc", Strings.BinToTextStringW(bin, false));

            var binWithInvalid = new List<byte>(System.Text.Encoding.Unicode.GetBytes("a\u0081c"));
            Assert.AreEqual("a.c", Strings.BinToTextStringW(binWithInvalid, false));

            var binWithWhiteSpace = new List<byte>(System.Text.Encoding.Unicode.GetBytes("a\r\nb\rc\n\td"));
            Assert.AreEqual("a..b.c..d", Strings.BinToTextStringW(binWithWhiteSpace, false));
            Assert.AreEqual("a\r\nb\rc\n\td", Strings.BinToTextStringW(binWithWhiteSpace, true));

            Assert.AreEqual("", Strings.BinToTextStringW(new List<byte>(), false));

            var mystringW = "mystring";
            var myStringWvector = new List<byte>(System.Text.Encoding.Unicode.GetBytes(mystringW));
            var vector_abcdW = new List<byte> { 0x61, 0, 0x62, 0, 0x63, 0, 0x64, 0 };
            var vector_abNULLdW = new List<byte> { 0x61, 0, 0x62, 0, 0x00, 0, 0x64, 0 };
            var vector_tabcrlfW = new List<byte> { 0x9, 0, 0xa, 0, 0xd, 0 };

            Assert.AreEqual("", Strings.BinToTextStringW(null, true));
            Assert.AreEqual("", Strings.BinToTextStringW(null, false));
            Assert.AreEqual(mystringW, Strings.BinToTextStringW(myStringWvector, false));
            Assert.AreEqual("abcd", Strings.BinToTextStringW(vector_abcdW, false));
            Assert.AreEqual("ab.d", Strings.BinToTextStringW(vector_abNULLdW, false));
            Assert.AreEqual("\t\n\r", Strings.BinToTextStringW(vector_tabcrlfW, true));
            Assert.AreEqual("...", Strings.BinToTextStringW(vector_tabcrlfW, false));
        }

        [TestMethod]
        public void Test_BinToTextStringA()
        {
            var bin = new List<byte>(System.Text.Encoding.ASCII.GetBytes("abc"));
            Assert.AreEqual("abc", Strings.BinToTextStringA(bin, false));

            var binWithInvalid = new List<byte> { 97, 0x81, 99 }; // a, invalid, c
            Assert.AreEqual("a.c", Strings.BinToTextStringA(binWithInvalid, false));

            Assert.AreEqual("", Strings.BinToTextStringA(new List<byte>(), false));

            var mystringA = "mystring";
            var myStringAvector = new List<byte>(System.Text.Encoding.ASCII.GetBytes(mystringA));
            var vector_abcdA = new List<byte> { 0x61, 0x62, 0x63, 0x64 };
            var vector_abNULLdA = new List<byte> { 0x61, 0x62, 0x00, 0x64 };
            var vector_tabcrlfA = new List<byte> { 0x9, 0xa, 0xd };

            Assert.AreEqual("", Strings.BinToTextStringA(null, true));
            Assert.AreEqual("", Strings.BinToTextStringA(null, false));
            Assert.AreEqual(mystringA, Strings.BinToTextStringA(myStringAvector, false));
            Assert.AreEqual("abcd", Strings.BinToTextStringA(vector_abcdA, false));
            Assert.AreEqual("ab.d", Strings.BinToTextStringA(vector_abNULLdA, false));
            Assert.AreEqual("\t\n\r", Strings.BinToTextStringA(vector_tabcrlfA, true));
            Assert.AreEqual("...", Strings.BinToTextStringA(vector_tabcrlfA, false));
            Assert.AreEqual(mystringA, Strings.BinToTextStringA(myStringAvector, true));
        }

        [TestMethod]
        public void Test_BinToHexString()
        {
            var bin = new List<byte> { 0xAB, 0xCD, 0xEF };
            Assert.AreEqual("ABCDEF", Strings.BinToHexString(bin));
            Assert.AreEqual("cb: 3 lpb: ABCDEF", Strings.BinToHexString(bin, true));
            Assert.AreEqual("NULL", Strings.BinToHexString(new List<byte>()));
            Assert.AreEqual("cb: 0 lpb: NULL", Strings.BinToHexString(new List<byte>(), true));
        }

        [TestMethod]
        public void Test_RemoveInvalidCharacters()
        {
            Assert.AreEqual("abc", Strings.RemoveInvalidCharacters("abc"));
            Assert.AreEqual("a.c", Strings.RemoveInvalidCharacters("a\u0081c"));
            Assert.AreEqual("", Strings.RemoveInvalidCharacters(""));
            Assert.AreEqual("a.c\0", Strings.RemoveInvalidCharacters("a\u0081c\0"));
        }
    }
}
