using BlockParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace BlockParserTests
{
    [TestClass]
    public class BlockParserTests
    {
        internal class TestBlock : Block
        {
            public BlockT<int> f1;
            public BlockT<short> f2;

            protected override void Parse()
            {
                f1 = ParseT<int>();
                f2 = ParseT<short>();
            }

            protected override void ParseBlocks()
            {
                SetText("TestBlock");
                AddChild(f1, $"f1 = 0x{f1.Data:X8}");
                AddChild(f2, $"f2 = 0x{f2.Data:X4}");
            }
        }

        internal class TestBlock2 : Block
        {
            public BlockT<int> f1;
            public BlockT<short> f2;
            public TestBlock tb;

            protected override void Parse()
            {
                f1 = ParseT<int>();
                f2 = ParseT<short>();
                tb = Parse<TestBlock>(parser, false);
            }

            protected override void ParseBlocks()
            {
                SetText("TestBlock2");
                AddChild(f1, $"f1 = 0x{f1.Data:X8}");
                AddChild(f2, $"f2 = 0x{f2.Data:X4}");
                AddChild(tb);
            }
        }

        [TestMethod]
        public void Test_TestBlock()
        {
            // CreateBlock a byte array for the parser to read (8 bytes: 4 for int, 2 for short, 2 padding)
            // Then read this array using TestBlock
            byte[] data = new byte[] { 0x78, 0x56, 0x34, 0x12, 0xCD, 0xAB, 0x00, 0x00 }; // int: 0x12345678, short: 0xABCD
            var parser = new BinaryParser(data);
            var block = new TestBlock();
            block.Parse(parser, true);

            Assert.AreEqual(0x12345678, block.f1);
            Assert.AreEqual(unchecked((short)0xABCD), block.f2); // Use 'unchecked' to allow the constant to be treated as a short
            Assert.AreEqual("TestBlock", block.Text);
            Assert.AreEqual(3, block.Children.Count);
            Assert.AreEqual("f1 = 0x12345678", block.Children[0].Text);
            Assert.AreEqual("f2 = 0xABCD", block.Children[1].Text);
            Assert.AreEqual("Unparsed data", block.Children[2].Text);
            Assert.AreEqual("0000", block.Children[2].Children[0].Text);
            Assert.AreEqual("cb: 2", block.Children[2].Children[0].Children[0].Text);
            Assert.AreEqual(
                "TestBlock\r\n" +
                "\tf1 = 0x12345678\r\n" +
                "\tf2 = 0xABCD\r\n" +
                "\tUnparsed data\r\n" +
                "\t\t0000\r\n" +
                "\t\t\tcb: 2",
                block.FullString());
        }

        [TestMethod]
        public void Test_TestBlock2()
        {
            // CreateBlock a byte array for the parser to read (14 bytes: 4 for int, 2 for short, then 4 for int,2 for short, 2 padding)
            // Then read this array using TestBlock2
            byte[] data = new byte[] { 0x09, 0x53, 0x67, 0x08, 0x68, 0x24, 0x78, 0x56, 0x34, 0x12, 0xCD, 0xAB, 0xBE, 0xEF }; // int: 0x08675309, short: 0x2468
            var parser = new BinaryParser(data);
            var block = new TestBlock2();
            block.Parse(parser, true);

            Assert.AreEqual(0x08675309, block.f1);
            Assert.AreEqual(unchecked((short)0x2468), block.f2); // Use 'unchecked' to allow the constant to be treated as a short
            Assert.AreEqual("TestBlock2", block.Text);
            Assert.AreEqual(4, block.Children.Count);
            Assert.AreEqual("f1 = 0x08675309", block.Children[0].Text);
            Assert.AreEqual("f2 = 0x2468", block.Children[1].Text);
            Assert.AreEqual("TestBlock", block.Children[2].Text);
            Assert.AreEqual(
                "TestBlock\r\n" +
                "\tf1 = 0x12345678\r\n" +
                "\tf2 = 0xABCD",
                block.Children[2].FullString());
            Assert.AreEqual("Unparsed data", block.Children[3].Text);
            Assert.AreEqual("BEEF", block.Children[3].Children[0].Text);
            Assert.AreEqual("cb: 2", block.Children[3].Children[0].Children[0].Text);
            Assert.AreEqual(
                "TestBlock2\r\n" +
                "\tf1 = 0x08675309\r\n" +
                "\tf2 = 0x2468\r\n" +
                "\tTestBlock\r\n" +
                "\t\tf1 = 0x12345678\r\n" +
                "\t\tf2 = 0xABCD\r\n" +
                "\tUnparsed data\r\n" +
                "\t\tBEEF\r\n" +
                "\t\t\tcb: 2",
                block.FullString());
        }

        [TestMethod]
        public void Test_TestBlock2InsufficientData()
        {
            byte[] data = new byte[] { 0x09, 0x53, 0x67, 0x08, 0x68 }; // int: 0x08675309, unparsed 0x68
            var parser = new BinaryParser(data);
            var block = new TestBlock2();
            block.Parse(parser, true);

            Assert.AreEqual(0x08675309, block.f1);
            Assert.AreEqual(0, block.f2);
            Assert.AreEqual("TestBlock2", block.Text);
            Assert.AreEqual(3, block.Children.Count);
            Assert.AreEqual("f1 = 0x08675309", block.Children[0].Text);
            Assert.AreEqual("TestBlock", block.Children[1].Text);
            Assert.AreEqual("TestBlock", block.Children[1].ToString());
            Assert.AreEqual("Unparsed data", block.Children[2].Text);
            Assert.AreEqual("68", block.Children[2].Children[0].Text);
            Assert.AreEqual("cb: 1", block.Children[2].Children[0].Children[0].Text);
            Assert.AreEqual(
                "TestBlock2\r\n" +
                "\tf1 = 0x08675309\r\n" +
                "\tTestBlock\r\n" +
                "\tUnparsed data\r\n" +
                "\t\t68\r\n" +
                "\t\t\tcb: 1",
                block.FullString());
        }

        [TestMethod]
        public void Parse_Stream_AdvancesStreamByBlockSize()
        {
            var data = new byte[] { 0x09, 0x53, 0x67, 0x08, 0x68, 0x24, 0x78, 0x56, 0x34, 0x12, 0xCD, 0xAB, 0xBE, 0xEF }; // int: 0x08675309, short: 0x2468
            using (var ms = new MemoryStream(data))
            {
                var block = Block.Parse<TestBlock2>(ms, enableJunk: false);

                Assert.IsInstanceOfType(block, typeof(TestBlock2));
                Assert.AreEqual(12, block.Size, "Size of TestBlock is 12");
                Assert.AreEqual(block.Size, ms.Position, "Stream position should advance by block size");
            }
        }

        public enum TestEnum : short
        {
            Value1 = 0xBCD,
            Value2 = 0x1234
        }
        public enum TestEnumNoType
        {
            Value1 = 0xBCDEF,
            Value2 = 0x12345
        }

        internal class TestEnumBlock : Block
        {
            public BlockT<TestEnum> f1;
            public BlockT<TestEnumNoType> f2;
            protected override void Parse()
            {
                f1 = ParseT<TestEnum>();
                f2 = ParseT<TestEnumNoType>();
            }
            protected override void ParseBlocks()
            {
                SetText("TestBlock");
                AddChildBlockT(f1, "f1");
                AddChildBlockT(f2, "f2");
            }
        }

        [TestMethod]
        public void TestEnumParsing()
        {
            // TestEnum : short (2 bytes), TestEnumNoType : int (4 bytes)
            // TestEnum.Value1 = 0x0BCD (little endian: CD 0B)
            // TestEnumNoType.Value1 = 0x0BCDEF (little endian: EF CD 0B 00)
            var data = new byte[] { 0xCD, 0x0B, 0xEF, 0xCD, 0x0B, 0x00 };

            var parser = new BinaryParser(data);
            var block = new TestEnumBlock();
            block.Parse(parser, true);

            Assert.AreEqual(TestEnum.Value1, block.f1);
            Assert.AreEqual(TestEnumNoType.Value1, block.f2);
            Assert.AreEqual("TestBlock", block.Text);
            Assert.AreEqual("TestBlock\r\n" +
                "\tf1:Value1\r\n" +
                "\tf2:Value1",
                block.FullString());
        }
    }
}