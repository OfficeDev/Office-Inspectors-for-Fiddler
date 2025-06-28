using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockParser;

namespace BlockParserTests
{
    [TestClass]
    public class BlockStringWTests
    {
        [TestMethod]
        public void Parse_BinaryParserWithDoubleNullTerminator_ParsesCorrectly()
        {
            // "test" + '\0' + '\0' (UTF-16LE)
            var str = "test";
            var bytes = Encoding.Unicode.GetBytes(str + "\0\0");
            var parser = new BinaryParser(bytes);
            var block = Block.ParseStringW(parser);
            Assert.AreEqual("test", block);
            Assert.AreEqual(4, block.Length);
        }

        [TestMethod]
        public void Parse_BinaryParserWithCchChar_ParsesCorrectLength()
        {
            var str = "abcde";
            var bytes = Encoding.Unicode.GetBytes(str + "\0");
            var parser = new BinaryParser(bytes);
            var block = Block.ParseStringW(parser, 5);
            Assert.AreEqual("abcde", block);
            Assert.AreEqual(5, block.Length);
        }

        [TestMethod]
        public void NullTerminator_ParsesCorrectly()
        {
            var rawData = new byte[] { 0x00, 0x00, 0x12, 0x34 };
            var parser = new BinaryParser(rawData);
            var block = Block.ParseStringW(parser);
            Assert.AreEqual("", block);
            Assert.AreEqual(0, block.Length); // Excluding null terminator
            Assert.AreEqual(2, block.Size); // Including null terminator
            Assert.IsTrue(block.Empty);
            Assert.IsTrue(block.Parsed);
            Assert.AreEqual(2, parser.Offset);
            Assert.AreEqual(2, parser.RemainingBytes);
        }

        [TestMethod]
        public void NoTerminator_ParsesCorrectly()
        {
            var rawData = new byte[] { 0x66, 0x00, 0x6F, 0x00, 0x6F, 0x00 };
            var parser = new BinaryParser(rawData);
            var block = Block.ParseStringW(parser);
            Assert.AreEqual("foo", block);
            Assert.AreEqual(3, block.Length); // Excluding null terminator
            Assert.AreEqual(6, block.Size); // No null terminator
            Assert.IsFalse(block.Empty);
            Assert.IsTrue(block.Parsed);
            Assert.AreEqual(6, parser.Offset);
            Assert.AreEqual(0, parser.RemainingBytes);
        }

        [TestMethod]
        public void RawBinaryData_ParsesCorrectly()
        {
            var rawData = new byte[] { 0x66, 0x00, 0x6F, 0x00, 0x6F, 0x00, 0x00, 0x00, 0xAA, 0xBB }; // "foo" + null terminator + extra
            var parser = new BinaryParser(rawData);
            var block = Block.ParseStringW(parser);
            Assert.AreEqual("foo", block);
            Assert.AreEqual(3, block.Length); // Excluding null terminator
            Assert.AreEqual(8, block.Size); // Including null terminator
            Assert.IsFalse(block.Empty);
            Assert.IsTrue(block.Parsed);
            Assert.AreEqual(8, parser.Offset);
            Assert.AreEqual(2, parser.RemainingBytes);
        }

        [TestMethod]
        public void RawBinaryData_ParsesCorrectlyLength()
        {
            var rawData = new byte[] { 0x66, 0x00, 0x6F, 0x00, 0x6F, 0x00, 0x00, 0x00, 0xAA, 0xBB }; // "foo" + null terminator + extra
            var parser = new BinaryParser(rawData);
            var block = Block.ParseStringW(parser, 3);
            Assert.AreEqual("foo", block);
            Assert.AreEqual(3, block.Length); // Excluding null terminator
            Assert.IsFalse(block.Empty);
            Assert.AreEqual(6, parser.Offset);
            Assert.AreEqual(4, parser.RemainingBytes);
        }

        [TestMethod]
        public void TruncatedString_ParsesCorrectly()
        {
            var rawData = new byte[] { 0x66, 0x00, 0x6F, 0x00, 0x6F }; // "fo" + naked 0x6F
            var parser = new BinaryParser(rawData);
            var block = Block.ParseStringW(parser); // Make it guess
            Assert.AreEqual("fo", block);
            Assert.AreEqual(2, block.Length); // Excluding null terminator
            Assert.IsFalse(block.Empty);
            Assert.IsTrue(block.Parsed);
            Assert.AreEqual(4, parser.Offset);
            Assert.AreEqual(1, parser.RemainingBytes);
        }

        [TestMethod]
        public void TruncatedStringWithCount_ParsesCorrectly()
        {
            var rawData = new byte[] { 0x66, 0x00, 0x6F, 0x00, 0x6F}; // "fo" + naked 0x6F
            var parser = new BinaryParser(rawData);
            var block = Block.ParseStringW(parser, 2); // Truncate to "fo"
            Assert.AreEqual("fo", block);
            Assert.AreEqual(2, block.Length); // Excluding null terminator
            Assert.IsFalse(block.Empty);
            Assert.IsTrue(block.Parsed);
            Assert.AreEqual(4, parser.Offset);
            Assert.AreEqual(1, parser.RemainingBytes);
        }

        [TestMethod]
        public void TruncatedStringWithBadCount_ParsesCorrectly()
        {
            var rawData = new byte[] { 0x66, 0x00, 0x6F, 0x00, 0x6F }; // "fo" + naked 0x6F
            var parser = new BinaryParser(rawData);
            var block = Block.ParseStringW(parser, 4); // Suppose we got our length wrong
            Assert.AreEqual("fo", block);
            Assert.AreEqual(2, block.Length); // Excluding null terminator
            Assert.IsFalse(block.Empty);
            Assert.IsTrue(block.Parsed);
            Assert.AreEqual(4, parser.Offset);
            Assert.AreEqual(1, parser.RemainingBytes);
        }

        [TestMethod]
        public void VeryTruncatedStringWithBadCount_ParsesCorrectly()
        {
            var rawData = new byte[] { 0x66 }; // naked 0x6g
            var parser = new BinaryParser(rawData);
            var block = Block.ParseStringW(parser, 4); // Suppose we got our length wrong
            Assert.AreEqual("", block);
            Assert.AreEqual(0, block.Length); // Excluding null terminator
            Assert.IsTrue(block.Empty);
            Assert.IsTrue(block.Parsed);
            Assert.AreEqual(0, parser.Offset);
            Assert.AreEqual(1, parser.RemainingBytes);
        }
    }
}
