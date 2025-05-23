using System;
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
            Assert.AreEqual(0, parser.GetSize());
            Assert.IsTrue(parser.Empty);
        }

        [TestMethod]
        public void Constructor_ByteArray_CorrectSize()
        {
            byte[] data = { 1, 2, 3, 4 };
            var parser = new BinaryParser(4, data);
            Assert.AreEqual(4, parser.GetSize());
            Assert.IsFalse(parser.Empty);
        }

        [TestMethod]
        public void Constructor_ByteArray_WithCap()
        {
            byte[] data = { 1, 2, 3, 4, 5 };
            var parser = new BinaryParser(3, data);
            Assert.AreEqual(3, parser.GetSize());
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, parser.GetAddress());
        }

        [TestMethod]
        public void Constructor_Stream_ReadsCorrectly()
        {
            byte[] data = { 10, 20, 30, 40 };
            using (var ms = new MemoryStream(data))
            {
                var parser = new BinaryParser(ms);
                Assert.AreEqual(4, parser.GetSize());
            }
        }

        [TestMethod]
        public void Constructor_Stream_WithCap()
        {
            byte[] data = { 10, 20, 30, 40 };
            using (var ms = new MemoryStream(data))
            {
                var parser = new BinaryParser(ms, 2);
                Assert.AreEqual(2, parser.GetSize());
                CollectionAssert.AreEqual(new byte[] { 10, 20 }, parser.GetAddress());
            }
        }

        [TestMethod]
        public void Constructor_ListByte()
        {
            var list = new List<byte> { 1, 2, 3 };
            var parser = new BinaryParser(list);
            Assert.AreEqual(3, parser.GetSize());
        }

        [TestMethod]
        public void Advance_And_Rewind()
        {
            byte[] data = { 1, 2, 3, 4 };
            var parser = new BinaryParser(4, data);
            Assert.AreEqual(4, parser.GetSize());
            parser.Advance(2);
            Assert.AreEqual(2, parser.Offset);
            Assert.AreEqual(2, parser.GetSize());
            parser.Rewind();
            Assert.AreEqual(0, parser.Offset);
            Assert.AreEqual(4, parser.GetSize());
        }

        [TestMethod]
        public void SetOffset_Works()
        {
            byte[] data = { 1, 2, 3, 4 };
            var parser = new BinaryParser(4, data);
            parser.Offset = 3;
            Assert.AreEqual(3, parser.Offset);
            CollectionAssert.AreEqual(new byte[] { 4 }, parser.GetAddress());
        }

        [TestMethod]
        public void GetAddress_ReturnsCorrectBytes()
        {
            byte[] data = { 1, 2, 3, 4 };
            var parser = new BinaryParser(4, data);
            parser.Advance(2);
            CollectionAssert.AreEqual(new byte[] { 3, 4 }, parser.GetAddress());
        }

        [TestMethod]
        public void SetCap_And_ClearCap()
        {
            byte[] data = { 1, 2, 3, 4, 5 };
            var parser = new BinaryParser(5, data);
            parser.Advance(1);
            CollectionAssert.AreEqual(new byte[] { 2, 3, 4, 5 }, parser.GetAddress());
            parser.SetCap(2);
            Assert.AreEqual(2, parser.GetSize());
            CollectionAssert.AreEqual(new byte[] { 2, 3 }, parser.GetAddress());
            parser.SetCap(3);
            Assert.AreEqual(3, parser.GetSize());
            CollectionAssert.AreEqual(new byte[] { 2, 3, 4 }, parser.GetAddress());
            parser.ClearCap();
            Assert.AreEqual(2, parser.GetSize());
            CollectionAssert.AreEqual(new byte[] { 2, 3 }, parser.GetAddress());
            parser.ClearCap();
            Assert.AreEqual(4, parser.GetSize());
            CollectionAssert.AreEqual(new byte[] { 2, 3, 4, 5 }, parser.GetAddress());
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
    }
}
