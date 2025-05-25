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
    }
}
