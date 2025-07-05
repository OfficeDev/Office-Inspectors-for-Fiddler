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

        [TestMethod]
        public void MultipleLines_ParsesCorrectly()
        {
            var str = "line1\r\nline2\r\nline 3"; // Three lines
            /*
             6C696E65310D0A 7 bytes
             6C696E65320D0A 7 bytes
             6C696E652033 6 bytes
             */
            var bytes = Encoding.ASCII.GetBytes(str);
            var parser = new BinaryParser(bytes);
            var block1 = Block.ParseStringLineA(parser);
            var block2 = Block.ParseStringLineA(parser); // Should parse the next line
            var block3 = Block.ParseStringLineA(parser); // Should parse the next line
            Assert.AreEqual("line1", block1); // First line
            Assert.AreEqual(0, block1.Offset);
            Assert.AreEqual(5, block1.Length);
            Assert.AreEqual(7, block1.Size);

            Assert.AreEqual("line2", block2); // Second line
            Assert.AreEqual(7, block2.Offset);
            Assert.AreEqual(5, block2.Length);
            Assert.AreEqual(7, block2.Size);

            Assert.AreEqual("line 3", block3);
            Assert.AreEqual(14, block3.Offset);
            Assert.AreEqual(6, block3.Length);
            Assert.AreEqual(6, block3.Size);

            Assert.AreEqual(20, parser.Offset); // Total bytes read
        }

        [TestMethod]
        public void MultipleLinesNullTerminated_ParsesCorrectly()
        {
            var bytes = new byte[]
            {
                // "line1\r\n"
                0x6C, 0x69, 0x6E, 0x65, 0x31, 0x0D, 0x0A, // 7 bytes
                // "line2\n"
                0x6C, 0x69, 0x6E, 0x65, 0x32, 0x0A, // 6 bytes
                // "line 3\0"
                0x6C, 0x69, 0x6E, 0x65, 0x20, 0x33, 0x00, // 7 bytes
                // trailing bytes
                0x12, 0x34, 0x56, 0x78
            };
            var parser = new BinaryParser(bytes);
            var block1 = Block.ParseStringLineA(parser);
            var block2 = Block.ParseStringLineA(parser);
            var block3 = Block.ParseStringLineA(parser);

            Assert.AreEqual("line1", block1);
            Assert.AreEqual(0, block1.Offset);
            Assert.AreEqual(5, block1.Length);
            Assert.AreEqual(7, block1.Size);

            Assert.AreEqual("line2", block2);
            Assert.AreEqual(7, block2.Offset);
            Assert.AreEqual(5, block2.Length);
            Assert.AreEqual(6, block2.Size);

            Assert.AreEqual("line 3", block3);
            Assert.AreEqual(13, block3.Offset);
            Assert.AreEqual(6, block3.Length);
            Assert.AreEqual(7, block3.Size);

            Assert.AreEqual(20, parser.Offset);
            Assert.AreEqual(4, parser.RemainingBytes);
            var int1 = Block.ParseT<int>(parser);

            Assert.AreEqual(0x78563412, int1);
        }
    }
}