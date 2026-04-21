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
        public void LineModeDoesNotReadNull()
        {
            var bytes = new byte[]
            {
                0x6C, 0x69, 0x0A, 0x0A, 0x00, 0xAA
            };
            var parser = new BinaryParser(bytes);
            var block = Block.ParseStringLineA(parser); // Should read "li" and stop at null
            Assert.AreEqual("li", block); // Should not include null
            Assert.AreEqual(0, block.Offset);
            Assert.AreEqual(2, block.Length); // Length is 2, not 3
            Assert.AreEqual(3, block.Size); // Size is 2, not 3 (no null terminator)

            var block2 = Block.ParseStringLineA(parser);
            Assert.IsTrue(block2.Empty); // Should be empty because we stopped at null
            Assert.IsTrue(block2.Parsed);
            Assert.AreEqual(3, block2.Offset);
            Assert.AreEqual(0, block2.Length); // Length 0 means we're done
            Assert.AreEqual(1, block2.Size);

            Assert.AreEqual(4, parser.Offset); // Should be at 4
            Assert.AreEqual(2, parser.RemainingBytes); // Should have 2 bytes remaining

            var tmpByte = Block.TestParse<byte>(parser); // Should read the next byte
            Assert.AreEqual(0, tmpByte); // Should be 0, the null terminator
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
                0x6C, 0x69, 0x6E, 0x65, 0x20, 0x33, 0x0A, // 7 bytes
                0x0D, 0x0A, // 2 bytes
                // trailing bytes
                0x12, 0x34, 0x56, 0x78
            };
            var parser = new BinaryParser(bytes);

            var block1 = Block.ParseStringLineA(parser);
            Assert.AreEqual("line1", block1);
            Assert.AreEqual(0, block1.Offset);
            Assert.AreEqual(5, block1.Length);
            Assert.AreEqual(7, block1.Size);

            var block2 = Block.ParseStringLineA(parser);
            Assert.AreEqual("line2", block2);
            Assert.AreEqual(7, block2.Offset);
            Assert.AreEqual(5, block2.Length);
            Assert.AreEqual(6, block2.Size);

            var block3 = Block.ParseStringLineA(parser);
            Assert.AreEqual("line 3", block3);
            Assert.AreEqual(13, block3.Offset);
            Assert.AreEqual(6, block3.Length);
            Assert.AreEqual(7, block3.Size);

            var block4 = Block.ParseStringLineA(parser);
            Assert.AreEqual(20, block4.Offset);
            Assert.AreEqual(0, block4.Length); // Length 0 means we're done
            Assert.AreEqual(2, block4.Size);

            Assert.AreEqual(4, parser.RemainingBytes);

            var int1 = Block.ParseT<int>(parser);

            Assert.AreEqual(0x78563412, int1);
        }

        [TestMethod]
        public void BlankLine_SingleLine_BlankLine_Line_EOS()
        {
            var bytes = new byte[]
            {
                // "line1\r\n"
                0x6C, 0x69, 0x6E, 0x65, 0x31, 0x0D, 0x0A, // 7 bytes
                // "\r\n" (blank line)
                0x0D, 0x0A, // 2 bytes
                // "line3"
                0x6C, 0x69, 0x6E, 0x65, 0x33 // 5 bytes (no terminator - EOS)
            };
            var parser = new BinaryParser(bytes);

            var block1 = Block.ParseStringLineA(parser);
            Assert.AreEqual("line1", block1);
            Assert.IsFalse(block1.BlankLine);
            Assert.AreEqual(0, block1.Offset);
            Assert.AreEqual(5, block1.Length);
            Assert.AreEqual(7, block1.Size);

            var block2 = Block.ParseStringLineA(parser);
            Assert.AreEqual("", block2);
            Assert.IsTrue(block2.BlankLine);
            Assert.AreEqual(7, block2.Offset);
            Assert.AreEqual(0, block2.Length);
            Assert.AreEqual(2, block2.Size);

            var block3 = Block.ParseStringLineA(parser);
            Assert.AreEqual("line3", block3);
            Assert.IsFalse(block3.BlankLine);
            Assert.AreEqual(9, block3.Offset);
            Assert.AreEqual(5, block3.Length);
            Assert.AreEqual(5, block3.Size);

            Assert.AreEqual(14, parser.Offset);
            Assert.AreEqual(0, parser.RemainingBytes);
        }

        [TestMethod]
        public void BlankLine_TwoLines_BlankLine_Line_NullTerminator_OtherData()
        {
            var bytes = new byte[]
            {
                // "line1\r\n"
                0x6C, 0x69, 0x6E, 0x65, 0x31, 0x0D, 0x0A, // 7 bytes
                // "line2\n"
                0x6C, 0x69, 0x6E, 0x65, 0x32, 0x0A, // 6 bytes
                // "\r\n" (blank line)
                0x0D, 0x0A, // 2 bytes
                // "line4\0" (null terminated)
                0x6C, 0x69, 0x6E, 0x65, 0x34, 0x00, // 6 bytes
                // Other data
                0xAA, 0xBB, 0xCC // 3 bytes
            };
            var parser = new BinaryParser(bytes);

            var block1 = Block.ParseStringLineA(parser);
            Assert.AreEqual("line1", block1);
            Assert.IsFalse(block1.BlankLine);
            Assert.AreEqual(0, block1.Offset);
            Assert.AreEqual(5, block1.Length);
            Assert.AreEqual(7, block1.Size);

            var block2 = Block.ParseStringLineA(parser);
            Assert.AreEqual("line2", block2);
            Assert.IsFalse(block2.BlankLine);
            Assert.AreEqual(7, block2.Offset);
            Assert.AreEqual(5, block2.Length);
            Assert.AreEqual(6, block2.Size);

            var block3 = Block.ParseStringLineA(parser);
            Assert.AreEqual("", block3);
            Assert.IsTrue(block3.BlankLine);
            Assert.AreEqual(13, block3.Offset);
            Assert.AreEqual(0, block3.Length);
            Assert.AreEqual(2, block3.Size);

            var block4 = Block.ParseStringLineA(parser);
            Assert.AreEqual("line4", block4);
            Assert.IsFalse(block4.BlankLine);
            Assert.AreEqual(15, block4.Offset);
            Assert.AreEqual(5, block4.Length);
            Assert.AreEqual(5, block4.Size);

            Assert.AreEqual(20, parser.Offset);
            Assert.AreEqual(4, parser.RemainingBytes);

            // Verify other data is still there
            var otherData = Block.ParseT<uint>(parser);
            Assert.AreEqual(0xCCBBAA00, otherData); // Little endian
        }

        [TestMethod]
        public void BlankLine_OnlyBlankLines()
        {
            var bytes = new byte[]
            {
                // "\r\n" (blank line 1)
                0x0D, 0x0A, // 2 bytes
                // "\n" (blank line 2)
                0x0A, // 1 byte
                // "\r\n" (blank line 3)
                0x0D, 0x0A, // 2 bytes
                // Other data
                0x12, 0x34
            };
            var parser = new BinaryParser(bytes);

            var block1 = Block.ParseStringLineA(parser);
            Assert.AreEqual("", block1);
            Assert.IsTrue(block1.BlankLine);
            Assert.AreEqual(0, block1.Offset);
            Assert.AreEqual(0, block1.Length);
            Assert.AreEqual(2, block1.Size);

            var block2 = Block.ParseStringLineA(parser);
            Assert.AreEqual("", block2);
            Assert.IsTrue(block2.BlankLine);
            Assert.AreEqual(2, block2.Offset);
            Assert.AreEqual(0, block2.Length);
            Assert.AreEqual(1, block2.Size);

            var block3 = Block.ParseStringLineA(parser);
            Assert.AreEqual("", block3);
            Assert.IsTrue(block3.BlankLine);
            Assert.AreEqual(3, block3.Offset);
            Assert.AreEqual(0, block3.Length);
            Assert.AreEqual(2, block3.Size);

            Assert.AreEqual(5, parser.Offset);
            Assert.AreEqual(2, parser.RemainingBytes);
        }

        [TestMethod]
        public void BlankLine_MixedLineEndings()
        {
            var bytes = new byte[]
            {
                // "text\r\n"
                0x74, 0x65, 0x78, 0x74, 0x0D, 0x0A, // 6 bytes
                // "\n" (blank line with LF only)
                0x0A, // 1 byte
                // "more\n"
                0x6D, 0x6F, 0x72, 0x65, 0x0A, // 5 bytes
                // "\r\n" (blank line with CRLF)
                0x0D, 0x0A // 2 bytes
            };
            var parser = new BinaryParser(bytes);

            var block1 = Block.ParseStringLineA(parser);
            Assert.AreEqual("text", block1);
            Assert.IsFalse(block1.BlankLine);
            Assert.AreEqual(0, block1.Offset);
            Assert.AreEqual(4, block1.Length);
            Assert.AreEqual(6, block1.Size);

            var block2 = Block.ParseStringLineA(parser);
            Assert.AreEqual("", block2);
            Assert.IsTrue(block2.BlankLine);
            Assert.AreEqual(6, block2.Offset);
            Assert.AreEqual(0, block2.Length);
            Assert.AreEqual(1, block2.Size);

            var block3 = Block.ParseStringLineA(parser);
            Assert.AreEqual("more", block3);
            Assert.IsFalse(block3.BlankLine);
            Assert.AreEqual(7, block3.Offset);
            Assert.AreEqual(4, block3.Length);
            Assert.AreEqual(5, block3.Size);

            var block4 = Block.ParseStringLineA(parser);
            Assert.AreEqual("", block4);
            Assert.IsTrue(block4.BlankLine);
            Assert.AreEqual(12, block4.Offset);
            Assert.AreEqual(0, block4.Length);
            Assert.AreEqual(2, block4.Size);

            Assert.AreEqual(14, parser.Offset);
            Assert.AreEqual(0, parser.RemainingBytes);
        }
    }
}