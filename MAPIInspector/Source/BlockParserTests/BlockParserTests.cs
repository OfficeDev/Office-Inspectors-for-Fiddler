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

        [TestMethod]
        public void Test_TestBlock()
        {
            // Create a byte array for the parser to read (8 bytes: 4 for int, 2 for short, 2 padding)
            // Then read this array using TestBlock
            byte[] data = new byte[] { 0x78, 0x56, 0x34, 0x12, 0xCD, 0xAB, 0x00, 0x00 }; // int: 0x12345678, short: 0xABCD
            var parser = new BinaryParser(data);
            var block = new TestBlock();
            block.Parse(parser, true);

            // Assert
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
    }
}