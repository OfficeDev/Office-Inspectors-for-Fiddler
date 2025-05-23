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
        public void Join_NullValues_ReturnsEmptyString()
        {
            var result = strings.Join(null, ",");
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Join_EmptyList_ReturnsEmptyString()
        {
            var result = strings.Join(new List<string>(), ",");
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Join_SingleElement_ReturnsElement()
        {
            var result = strings.Join(new List<string> { "a" }, ",");
            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void Join_MultipleElements_ReturnsJoinedString()
        {
            var result = strings.Join(new List<string> { "a", "b", "c" }, ",");
            Assert.AreEqual("a,b,c", result);
        }

        [TestMethod]
        public void Join_SeparatorIsEmptyString_JoinsWithoutSeparator()
        {
            var result = strings.Join(new List<string> { "a", "b", "c" }, "");
            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public void Join_ElementsContainNulls_TreatsNullAsEmptyString()
        {
            var result = strings.Join(new List<string> { "a", null, "c" }, ",");
            Assert.AreEqual("a,,c", result);
        }
    }
}
