using BlockParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace BlockParserTests
{
    [TestClass]
    public class NestedBlockTests
    {
        internal class ParentBlock : Block
        {
            public BlockT<int> KidCount;
            public ChildBlock[] Kids;

            protected override void Parse()
            {
                KidCount = BlockT<int>.Parse(parser);
                List<ChildBlock> tempKids = new List<ChildBlock>();
                for (int i = 0; i < KidCount.Data; i++)
                {
                    var kid = new ChildBlock();
                    kid.Parse(parser);
                    tempKids.Add(kid);
                }

                Kids = tempKids.ToArray();
            }

            protected override void ParseBlocks()
            {
                SetText("ParentBlock");
                AddChild(KidCount, "KidCount:{0}", KidCount.Data);
                AddLabeledChildren("Kids", Kids);
            }
        }

        internal class ChildBlock : Block
        {
            public BlockT<short> f2;

            protected override void Parse()
            {
                f2 = BlockT<short>.Parse(parser);
            }

            protected override void ParseBlocks()
            {
                SetText("ChildBlock");
                AddChild(f2, "f2:{0}", f2.Data);
            }
        }

        [TestMethod]
        public void Test_ParentBlock_WithJunk()
        {
            // Data: int32 kid count (3), short 1, short 3, short 5, 3 bytes junk
            byte[] data = new byte[]
            {
                0x03, 0x00, 0x00, 0x00, // KidCount = 3
                0x01, 0x00,             // Kid 1: f2 = 1
                0x03, 0x00,             // Kid 2: f2 = 3
                0x05, 0x00,             // Kid 3: f2 = 5
                0xAA, 0xBB, 0xCC        // Junk data
            };
            var parser = new BinaryParser(data);
            var block = new ParentBlock();
            block.Parse(parser, true);

            Assert.AreEqual(3, block.KidCount.Data);
            Assert.AreEqual(3, block.Kids.Length);
            Assert.AreEqual(1, block.Kids[0].f2.Data);
            Assert.AreEqual(3, block.Kids[1].f2.Data);
            Assert.AreEqual(5, block.Kids[2].f2.Data);
            // Check for junk node
            Assert.IsTrue(block.Children.Count > 1);
            var junkNode = block.Children[block.Children.Count - 1];
            Assert.IsTrue(junkNode.Text.Contains("Unparsed data size"));
            Assert.IsTrue(junkNode.Children[0].Text.Contains("cb: 3"));
            // Verify each block has the correct size and offset relative to the original array of data
            Assert.AreEqual(13, block.Size);
            Assert.AreEqual(0, block.Offset);

            // Now walk through Children checking sizes and offsets
            Assert.AreEqual("ParentBlock", block.Text, "ParentBlock text");
            Assert.AreEqual(0, block.Offset, "ParentBlock offset");
            Assert.AreEqual(sizeof(int) + sizeof(short) * 3 + sizeof(byte) * 3, block.Size, "ParentBlock size");

            // KidCount
            Assert.AreEqual("KidCount:3", block.Children[0].Text, "KidCount text");
            Assert.AreEqual(0, block.Children[0].Offset, "KidCount offset");
            Assert.AreEqual(sizeof(int), block.Children[0].Size, "KidCount size");

            // Kids
            Assert.AreEqual("Kids", block.Children[1].Text, "Kids text");
            Assert.AreEqual(sizeof(int), block.Children[1].Offset, "Kids offset");
            Assert.AreEqual(sizeof(short) * 3, block.Children[1].Size, "Kids size");

            // Junk Data
            Assert.AreEqual("Unparsed data size = 0x00000003", block.Children[2].Text, "JunkData text");
            Assert.AreEqual(sizeof(int) + sizeof(short) * 3, block.Children[2].Offset, "JunkData offset");
            Assert.AreEqual(sizeof(byte) * 3, block.Children[2].Size, "JunkData size");
        }
    }
}