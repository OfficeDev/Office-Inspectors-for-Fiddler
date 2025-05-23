using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using System;
using System.Text;

namespace BlockParserTests
{
    [TestClass]
    public class BlockStringATests
    {
        [TestMethod]
        public void Parse_BinaryParserWithNullTerminator_ParsesCorrectly()
        {
            // "test" + '\0'
            var str = "test";
            var bytes = Encoding.ASCII.GetBytes(str + "\0");
            var parser = new BinaryParser(bytes);
            var block = BlockStringA.Parse(parser);
            Assert.AreEqual("test", block.Data);
            Assert.AreEqual(4, block.Length);
        }

        [TestMethod]
        public void Parse_BinaryParserWithCchChar_ParsesCorrectLength()
        {
            var str = "abcde";
            var bytes = Encoding.ASCII.GetBytes(str + "\0");
            var parser = new BinaryParser(bytes);
            var block = BlockStringA.Parse(parser, 5);
            Assert.AreEqual("abcde", block.Data);
            Assert.AreEqual(5, block.Length);
        }

        [TestMethod]
        public void ImplicitOperatorString_ReturnsData()
        {
            var block = BlockStringA.Parse(new BinaryParser(Encoding.ASCII.GetBytes("foo\0")), 3);
            string s = block.Data;
            Assert.AreEqual("foo", s);
        }

        [TestMethod]
        public void EmptySA_ReturnsEmptyBlock()
        {
            var block = BlockStringA.EmptySA();
            Assert.IsTrue(block.Empty);
            Assert.AreEqual(0, block.Length);
        }

        [TestMethod]
        public void RawBinaryData_ParsesCorrectly()
        {
            var rawData = new byte[] { 0x66, 0x6F, 0x6F, 0x00 }; // "foo" + null terminator
            var parser = new BinaryParser(rawData);
            var block = BlockStringA.Parse(parser);
            Assert.AreEqual("foo", block.Data);
            Assert.AreEqual(3, block.Length); // Excluding null terminator
            Assert.IsFalse(block.Empty);
        }
    }
}