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
            var rawData = new byte[] { 0x66, 0x00, 0x6F, 0x00, 0x6F }; // "fo" + naked 0x6F
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

        [TestMethod]
        public void MultipleLines_ParsesCorrectly()
        {
            var str = "line1\r\nline2\r\nline 3"; // Three lines
            /*
             6C0069006E00650031000D000A00 14 bytes
             6C0069006E00650032000D000A00 14 bytes
             6C0069006E00650020003300 12 bytes
             */
            var bytes = Encoding.Unicode.GetBytes(str);
            var parser = new BinaryParser(bytes);
            var block1 = Block.ParseStringLineW(parser);
            var block2 = Block.ParseStringLineW(parser); // Should parse the next line
            var block3 = Block.ParseStringLineW(parser); // Should parse the next line
            Assert.AreEqual("line1", block1); // First line
            Assert.AreEqual(0, block1.Offset);
            Assert.AreEqual(5, block1.Length);
            Assert.AreEqual(14, block1.Size);

            Assert.AreEqual("line2", block2); // Second line
            Assert.AreEqual(14, block2.Offset);
            Assert.AreEqual(5, block2.Length);
            Assert.AreEqual(14, block2.Size);

            Assert.AreEqual("line 3", block3);
            Assert.AreEqual(28, block3.Offset);
            Assert.AreEqual(6, block3.Length);
            Assert.AreEqual(12, block3.Size);

            Assert.AreEqual(40, parser.Offset); // Total bytes read
        }

        [TestMethod]
        public void MultipleLinesNullTerminated_ParsesCorrectly()
        {
            var bytes = new byte[]
            {
                // "line1\r\n"
                0x6C, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x65, 0x00, 0x31, 0x00, 0x0D, 0x00, 0x0A, 0x00, // 14 bytes
                // "line2\n"
                0x6C, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x65, 0x00, 0x32, 0x00, 0x0A, 0x00, // 12 bytes
                // "line 3\0"
                0x6C, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x65, 0x00, 0x20, 0x00, 0x33, 0x00, 0x00, 0x00, // 14 bytes
                // trailing bytes
                0x12, 0x34, 0x56, 0x78
            };
            var parser = new BinaryParser(bytes);
            var block1 = Block.ParseStringLineW(parser);
            var block2 = Block.ParseStringLineW(parser);
            var block3 = Block.ParseStringLineW(parser);

            Assert.AreEqual("line1", block1);
            Assert.AreEqual(0, block1.Offset);
            Assert.AreEqual(5, block1.Length);
            Assert.AreEqual(14, block1.Size);

            Assert.AreEqual("line2", block2);
            Assert.AreEqual(14, block2.Offset);
            Assert.AreEqual(5, block2.Length);
            Assert.AreEqual(12, block2.Size);

            Assert.AreEqual("line 3", block3);
            Assert.AreEqual(26, block3.Offset);
            Assert.AreEqual(6, block3.Length);
            Assert.AreEqual(14, block3.Size);

            Assert.AreEqual(40, parser.Offset);
            Assert.AreEqual(4, parser.RemainingBytes);
            var int1 = Block.ParseT<int>(parser);

            Assert.AreEqual(0x78563412, int1);
        }
    }
}
