using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Parser.Tests
{
    [TestClass]
    public class BinaryParserTests
    {
        [TestMethod]
        public void Constructor_Empty_Defaults()
        {
            var parser = new BinaryParser();
            Assert.AreEqual(0, parser.RemainingBytes);
            Assert.IsTrue(parser.Empty);
        }

        [TestMethod]
        public void Constructor_ByteArray_CorrectSize()
        {
            byte[] data = { 1, 2, 3, 4 };
            var parser = new BinaryParser(4, data);
            Assert.AreEqual(4, parser.RemainingBytes);
            Assert.IsFalse(parser.Empty);
        }

        [TestMethod]
        public void Constructor_ByteArray_WithCap()
        {
            byte[] data = { 1, 2, 3, 4, 5 };
            var parser = new BinaryParser(3, data);
            Assert.AreEqual(3, parser.RemainingBytes);
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, parser.ReadBytes(parser.RemainingBytes));
        }

        [TestMethod]
        public void Constructor_Stream_ReadsCorrectly()
        {
            byte[] data = { 10, 20, 30, 40 };
            using (var ms = new MemoryStream(data))
            {
                var parser = new BinaryParser(ms);
                Assert.AreEqual(4, parser.RemainingBytes);
            }
        }

        [TestMethod]
        public void Constructor_Stream_WithCap()
        {
            byte[] data = { 10, 20, 30, 40 };
            using (var ms = new MemoryStream(data))
            {
                var parser = new BinaryParser(ms, 2);
                Assert.AreEqual(2, parser.RemainingBytes);
                CollectionAssert.AreEqual(new byte[] { 10, 20 }, parser.ReadBytes(parser.RemainingBytes));
            }
        }

        [TestMethod]
        public void Constructor_ListByte()
        {
            var list = new List<byte> { 1, 2, 3 };
            var parser = new BinaryParser(list);
            Assert.AreEqual(3, parser.RemainingBytes);
        }

        [TestMethod]
        public void Advance_And_Rewind()
        {
            byte[] data = { 1, 2, 3, 4 };
            var parser = new BinaryParser(4, data);
            Assert.AreEqual(4, parser.RemainingBytes);
            parser.Advance(2);
            Assert.AreEqual(2, parser.Offset);
            Assert.AreEqual(2, parser.RemainingBytes);
            parser.Rewind();
            Assert.AreEqual(0, parser.Offset);
            Assert.AreEqual(4, parser.RemainingBytes);
        }

        [TestMethod]
        public void SetOffset_Works()
        {
            byte[] data = { 1, 2, 3, 4 };
            var parser = new BinaryParser(4, data);
            parser.Offset = 3;
            Assert.AreEqual(3, parser.Offset);
            CollectionAssert.AreEqual(new byte[] { 4 }, parser.ReadBytes(parser.RemainingBytes));
        }

        [TestMethod]
        public void ReadBytes_ReturnsCorrectBytes()
        {
            byte[] data = { 1, 2, 3, 4 };
            var parser = new BinaryParser(4, data);
            parser.Advance(2);
            CollectionAssert.AreEqual(new byte[] { 3, 4 }, parser.ReadBytes(parser.RemainingBytes));
            // Shouldn't be anything left now
            CollectionAssert.AreEqual(new byte[] { }, parser.ReadBytes(parser.RemainingBytes));
        }

        // TODO: I don't trust this logic is correct
        // Revisit when we have a real world test case to think about
        [TestMethod]
        public void SetCap_And_ClearCap()
        {
            byte[] data = { 1, 2, 3, 4, 5 };
            var parser = new BinaryParser(5, data);
            parser.Advance(1);
            Assert.AreEqual(1, parser.Offset);
            parser.PushCap(2);
            Assert.AreEqual(2, parser.RemainingBytes);
            Assert.AreEqual(1, parser.Offset);
            parser.PushCap(3);
            Assert.AreEqual(3, parser.RemainingBytes);
            Assert.AreEqual(1, parser.Offset);
            parser.PopCap();
            Assert.AreEqual(2, parser.RemainingBytes);
            Assert.AreEqual(1, parser.Offset);
            parser.PopCap();
            Assert.AreEqual(4, parser.RemainingBytes);
            Assert.AreEqual(1, parser.Offset);
        }

        [TestMethod]
        public void GetSize_And_CheckSize()
        {
            byte[] data = { 1, 2, 3 };
            var parser = new BinaryParser(3, data);
            Assert.IsTrue(parser.CheckSize(2));
            parser.Advance(2);
            Assert.IsFalse(parser.CheckSize(2));
        }

        [TestMethod]
        public void Empty_Property()
        {
            byte[] data = { 1, 2 };
            var parser = new BinaryParser(2, data);
            Assert.IsFalse(parser.Empty);
            parser.Advance(2);
            Assert.IsTrue(parser.Empty);
        }

        [TestMethod]
        public void Test_StreamConstructor()
        {
            // have a stream and read a few bytes from it. Then set up a binary parser and read a few bytes
            // They should read from the start of the original stream data
            // But position in the source stream should be unchanged
            // Finally, since we leave some bytes unread, block ToString should show junk data
            byte[] data = { 1, 2, 3, 4, 5, 6 };
            using (var ms = new MemoryStream(data))
            {
                var ms1 = ms.ReadByte();
                Assert.AreEqual(1, ms1);
                Assert.AreEqual(1, ms.Position);
                var parser = new BinaryParser(ms, 3);
                Assert.AreEqual(1, ms.Position);
                Assert.AreEqual(3, parser.RemainingBytes);
                CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, parser.ReadBytes(parser.RemainingBytes));
                Assert.AreEqual(1, ms.Position);
                parser.Rewind();
                parser.Advance(2);
                Assert.AreEqual(1, ms.Position);
                CollectionAssert.AreEqual(new byte[] { 3 }, parser.ReadBytes(parser.RemainingBytes));
            }
        }

        [TestMethod]
        public void Test_ReadBytes()
        {
            // Test reading bytes from the parser
            byte[] data = { 1, 2, 3, 4, 5 };
            var parser = new BinaryParser(5, data);
            byte[] readBytes = parser.ReadBytes(3);
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, readBytes);
            Assert.AreEqual(3, parser.Offset);
            byte[] readBytes2 = parser.ReadBytes(2);
            CollectionAssert.AreEqual(new byte[] { 4, 5 }, readBytes2);
            Assert.AreEqual(5, parser.Offset);
            Assert.AreEqual(true, parser.Empty);
        }
    }
}
