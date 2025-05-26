using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;

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
                f1 = BlockT<int>.Parse(parser);
                f2 = BlockT<short>.Parse(parser);
            }

            protected override void ParseBlocks()
            {
                SetText("TestBlock");
                AddChild(f1, "f1 = 0x{0:X8}", f1.GetData());
                AddChild(f2, "f2 = 0x{0:X4}", f2.GetData());
            }
        }

        internal class TestBlock2 : Block
        {
            public BlockT<int> f1;
            public BlockT<short> f2;
            public TestBlock tb;

            protected override void Parse()
            {
                f1 = BlockT<int>.Parse(parser);
                f2 = BlockT<short>.Parse(parser);
                tb = Parse<TestBlock>(parser, false);
            }

            protected override void ParseBlocks()
            {
                SetText("TestBlock2");
                AddChild(f1, "f1 = 0x{0:X8}", f1.GetData());
                AddChild(f2, "f2 = 0x{0:X4}", f2.GetData());
                AddChild(tb);
            }
        }

        [TestMethod]
        public void Test_TestBlock()
        {
            // Create a byte array for the parser to read (8 bytes: 4 for int, 2 for short, 2 padding)
            // Then read this array using TestBlock
            byte[] data = new byte[] { 0x78, 0x56, 0x34, 0x12, 0xCD, 0xAB, 0x00, 0x00 }; // int: 0x12345678, short: 0xABCD
            var parser = new BinaryParser(data);
            var block = new TestBlock();
            block.Parse(parser, true);

            Assert.AreEqual(0x12345678, block.f1.GetData());
            Assert.AreEqual(unchecked((short)0xABCD), block.f2.GetData()); // Use 'unchecked' to allow the constant to be treated as a short
            Assert.AreEqual("TestBlock", block.Text);
            Assert.AreEqual(3, block.Children.Count);
            Assert.AreEqual("f1 = 0x12345678", block.Children[0].Text);
            Assert.AreEqual("f2 = 0xABCD", block.Children[1].Text);
            Assert.AreEqual("Unparsed data size = 0x00000002", block.Children[2].Text);
            Assert.AreEqual("cb: 2 lpb: 0000", block.Children[2].Children[0].Text);
            Assert.AreEqual(
                "TestBlock\r\n" +
                "\tf1 = 0x12345678\r\n" +
                "\tf2 = 0xABCD\r\n" +
                "\tUnparsed data size = 0x00000002\r\n" +
                "\t\tcb: 2 lpb: 0000",
                block.ToStringBlock()
            );
        }

        [TestMethod]
        public void Test_TestBlock2()
        {
            // Create a byte array for the parser to read (14 bytes: 4 for int, 2 for short, then 4 for int,2 for short, 2 padding)
            // Then read this array using TestBlock2
            byte[] data = new byte[] { 0x09, 0x53, 0x67, 0x08, 0x68, 0x24, 0x78, 0x56, 0x34, 0x12, 0xCD, 0xAB, 0xBE, 0xEF }; // int: 0x12345678, short: 0xABCD
            var parser = new BinaryParser(data);
            var block = new TestBlock2();
            block.Parse(parser, true);

            Assert.AreEqual(0x08675309, block.f1.GetData());
            Assert.AreEqual(unchecked((short)0x2468), block.f2.GetData()); // Use 'unchecked' to allow the constant to be treated as a short
            Assert.AreEqual("TestBlock2", block.Text);
            Assert.AreEqual(4, block.Children.Count);
            Assert.AreEqual("f1 = 0x08675309", block.Children[0].Text);
            Assert.AreEqual("f2 = 0x2468", block.Children[1].Text);
            Assert.AreEqual("TestBlock", block.Children[2].Text);
            Assert.AreEqual(
                "TestBlock\r\n" +
                "\tf1 = 0x12345678\r\n" +
                "\tf2 = 0xABCD",
                block.Children[2].ToStringBlock());
            Assert.AreEqual("Unparsed data size = 0x00000002", block.Children[3].Text);
            Assert.AreEqual("cb: 2 lpb: BEEF", block.Children[3].Children[0].Text);
            Assert.AreEqual(
                "TestBlock2\r\n" +
                "\tf1 = 0x08675309\r\n" +
                "\tf2 = 0x2468\r\n" +
                "\tTestBlock\r\n" +
                "\t\tf1 = 0x12345678\r\n" +
                "\t\tf2 = 0xABCD\r\n" +
                "\tUnparsed data size = 0x00000002\r\n" +
                "\t\tcb: 2 lpb: BEEF",
                block.ToStringBlock()
            );
        }

        [TestMethod]
        public void Test_TestBlock2InsufficientData()
        {
            byte[] data = new byte[] { 0x09, 0x53, 0x67, 0x08, 0x68 }; // int: 0x12345678, unparsed 0x68
            var parser = new BinaryParser(data);
            var block = new TestBlock2();
            block.Parse(parser, true);

            Assert.AreEqual(0x08675309, block.f1.GetData());
            Assert.AreEqual(0, block.f2.GetData());
            Assert.AreEqual("TestBlock2", block.Text);
            Assert.AreEqual(3, block.Children.Count);
            Assert.AreEqual("f1 = 0x08675309", block.Children[0].Text);
            Assert.AreEqual("TestBlock", block.Children[1].Text);
            Assert.AreEqual("TestBlock", block.Children[1].ToStringBlock());
            Assert.AreEqual("Unparsed data size = 0x00000001", block.Children[2].Text);
            Assert.AreEqual("cb: 1 lpb: 68", block.Children[2].Children[0].Text);
            Assert.AreEqual(
                "TestBlock2\r\n" +
                "\tf1 = 0x08675309\r\n" +
                "\tTestBlock\r\n" +
                "\tUnparsed data size = 0x00000001\r\n" +
                "\t\tcb: 1 lpb: 68",
                block.ToStringBlock()
            );
        }
    }
}