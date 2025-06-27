using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockParser;
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
            var block = Block.ParseStringA(parser);
            Assert.AreEqual("test", block);
            Assert.AreEqual(4, block.Length);
        }

        [TestMethod]
        public void Parse_BinaryParserWithCchChar_ParsesCorrectLength()
        {
            var str = "abcde";
            var bytes = Encoding.ASCII.GetBytes(str + "\0");
            var parser = new BinaryParser(bytes);
            var block = Block.ParseStringA(parser, 5);
            Assert.AreEqual("abcde", block);
            Assert.AreEqual(5, block.Length);
        }

        [TestMethod]
        public void ImplicitOperatorString_ReturnsData()
        {
            var block = Block.ParseStringA(new BinaryParser(Encoding.ASCII.GetBytes("foo\0")), 3);
            string s = block;
            Assert.AreEqual("foo", s);
        }

        [TestMethod]
        public void NullTerminator_ParsesCorrectly()
        {
            var rawData = new byte[] { 0x00, 0x00, 0x12, 0x34 };
            var parser = new BinaryParser(rawData);
            var block = Block.ParseStringA(parser);
            Assert.AreEqual("", block);
            Assert.AreEqual(0, block.Length); // Excluding null terminator
            Assert.AreEqual(1, block.Size); // Including null terminator
            Assert.IsTrue(block.Empty);
            Assert.IsTrue(block.Parsed);
            Assert.AreEqual(1, parser.Offset);
            Assert.AreEqual(3, parser.RemainingBytes);
        }

        [TestMethod]
        public void NoTerminator_ParsesCorrectly()
        {
            var rawData = new byte[] { 0x66, 0x6F, 0x6F };
            var parser = new BinaryParser(rawData);
            var block = Block.ParseStringA(parser);
            Assert.AreEqual("foo", block);
            Assert.AreEqual(3, block.Length); // Excluding null terminator
            Assert.AreEqual(3, block.Size); // No null terminator
            Assert.IsFalse(block.Empty);
            Assert.IsTrue(block.Parsed);
            Assert.AreEqual(3, parser.Offset);
            Assert.AreEqual(0, parser.RemainingBytes);
        }

        [TestMethod]
        public void RawBinaryData_ParsesCorrectly()
        {
            var rawData = new byte[] { 0x66, 0x6F, 0x6F, 0x00, 0xAA, 0xBB }; // "foo" + null terminator
            var parser = new BinaryParser(rawData);
            var block = Block.ParseStringA(parser);
            Assert.AreEqual("foo", block);
            Assert.AreEqual(3, block.Length); // Excluding null terminator
            Assert.IsFalse(block.Empty);
            Assert.AreEqual(4, parser.Offset);
            Assert.AreEqual(2, parser.RemainingBytes);
        }

        [TestMethod]
        public void RawBinaryData_ParsesCorrectlyLength()
        {
            var rawData = new byte[] { 0x66, 0x6F, 0x6F, 0x00, 0xAA, 0xBB }; // "foo" + null terminator
            var parser = new BinaryParser(rawData);
            var block = Block.ParseStringA(parser, 3);
            Assert.AreEqual("foo", block);
            Assert.AreEqual(3, block.Length); // Excluding null terminator
            Assert.IsFalse(block.Empty);
            Assert.AreEqual(3, parser.Offset);
            Assert.AreEqual(3, parser.RemainingBytes);
        }
    }
}