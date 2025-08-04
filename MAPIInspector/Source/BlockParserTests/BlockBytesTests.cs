using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockParser;

namespace BlockParserTests
{
    [TestClass]
    public class BlockBytesTests
    {
        [TestMethod]
        public void Parse_ShouldParseBytesCorrectly()
        {
            var data = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var parser = new BinaryParser(data);
            var block = Block.ParseBytes(parser, 4);

            CollectionAssert.AreEqual(data, new List<byte>(block.Data));
            Assert.AreEqual(4, block.Count);
            Assert.IsFalse(block.Empty);
        }

        [TestMethod]
        public void Parse_ShouldNotParse_WhenSizeIsTooSmall()
        {
            var data = new byte[] { 0x01, 0x02 };
            var parser = new BinaryParser(data);
            var block = Block.ParseBytes(parser, 4);

            Assert.AreEqual(0, block.Count);
            Assert.IsTrue(block.Empty);
        }

        [TestMethod]
        public void ToTextStringA_ShouldReturnCorrectString()
        {
            var data = new byte[] { 0x41, 0x42, 0x43, 0x00 };
            var parser = new BinaryParser(data);
            var block = Block.ParseBytes(parser, 4);
            var text = block.ToTextString();

            Assert.AreEqual("ABC.", text);
        }

        [TestMethod]
        public void ToHexString_ShouldReturnCorrectHex()
        {
            var data = new byte[] { 0x0A, 0x1B, 0x2C };
            var parser = new BinaryParser(data);
            var block = Block.ParseBytes(parser, 3);

            Assert.AreEqual("0A1B2C", block.ToHexString());
        }

        [TestMethod]
        public void Equal_ShouldReturnTrueForEqualData()
        {
            var data = new byte[] { 0x10, 0x20, 0x30 };
            var parser = new BinaryParser(data);
            var block = Block.ParseBytes(parser, 3);

            Assert.IsTrue(block.Equal(3, data));
        }

        [TestMethod]
        public void Equal_ShouldReturnFalseForDifferentData()
        {
            var data = new byte[] { 0x10, 0x20, 0x30 };
            var parser = new BinaryParser(data);
            var block = Block.ParseBytes(parser, 3);
            var other = new byte[] { 0x10, 0x20, 0x31 };

            Assert.IsFalse(block.Equal(3, other));
        }

        [TestMethod]
        public void Parse_ShouldRespectMaxBytes()
        {
            var data = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var parser = new BinaryParser(data);
            var block = Block.ParseBytes(parser, 4, 3);

            Assert.AreEqual(0, block.Count);
        }
    }
}
