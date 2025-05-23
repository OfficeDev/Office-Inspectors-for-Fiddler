using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;

namespace BlockParserTests
{
    [TestClass]
    public class BlockStringWTests
    {
        [TestMethod]
        public void Parse_StringData_CreatesBlockWithCorrectData()
        {
            var block = BlockStringW.Parse("hello", 10, 5);
            Assert.AreEqual("hello", block.Data);
            Assert.AreEqual(5, block.Length);
            Assert.IsFalse(block.Empty);
        }

        [TestMethod]
        public void Parse_EmptyString_ReturnsEmptyBlock()
        {
            var block = BlockStringW.Parse("", 0, 0);
            Assert.AreEqual("", block.Data);
            Assert.AreEqual(0, block.Length);
            Assert.IsTrue(block.Empty);
        }

        [TestMethod]
        public void Parse_BinaryParserWithDoubleNullTerminator_ParsesCorrectly()
        {
            // "test" + '\0' + '\0' (UTF-16LE)
            var str = "test";
            var bytes = Encoding.Unicode.GetBytes(str + "\0\0");
            var parser = new BinaryParser(bytes);
            var block = BlockStringW.Parse(parser);
            Assert.AreEqual("test", block.Data);
            Assert.AreEqual(4, block.Length);
        }

        [TestMethod]
        public void Parse_BinaryParserWithCchChar_ParsesCorrectLength()
        {
            var str = "abcde";
            var bytes = Encoding.Unicode.GetBytes(str + "\0");
            var parser = new BinaryParser(bytes);
            var block = BlockStringW.Parse(parser, 5);
            Assert.AreEqual("abcde", block.Data);
            Assert.AreEqual(5, block.Length);
        }

        [TestMethod]
        public void ImplicitOperatorString_ReturnsData()
        {
            var block = BlockStringW.Parse("foo", 3, 0);
            string s = block;
            Assert.AreEqual("foo", s);
        }

        [TestMethod]
        public void EmptySW_ReturnsEmptyBlock()
        {
            var block = BlockStringW.EmptySW();
            Assert.IsTrue(block.Empty);
            Assert.AreEqual(0, block.Length);
        }

        [TestMethod]
        public void RawBinaryData_ParsesCorrectly()
        {
            var rawData = new byte[] { 0x66, 0x00, 0x6F, 0x00, 0x6F, 0x00, 0x00, 0x00 }; // "foo" + null terminator
            var parser = new BinaryParser(rawData);
            var block = BlockStringW.Parse(parser);
            Assert.AreEqual("foo", block.Data);
            Assert.AreEqual(3, block.Length); // Excluding null terminator
            Assert.IsFalse(block.Empty);
        }
    }
}
