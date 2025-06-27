using BlockParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BlockParserTests
{
    [TestClass]
    public class PartialBlockParserTests
    {
        internal class ExpandingBlock : Block
        {
            public BlockT<byte>[] f1;

            protected override void Parse()
            {
                var _f1 = new List<BlockT<byte>>();
                while (!parser.Empty && parser.RemainingBytes >= sizeof(byte))
                {
                    var b = ParseT<byte>();
                    if (!b.Parsed) break; // Stop parsing when we hit a zero byte
                    _f1.Add(b);
                }

                f1 = _f1.ToArray();
            }

            protected override void ParseBlocks()
            {
                SetText("TestBlock");
                int i = 0;
                foreach (var b in f1)
                {
                    AddChild(b, $"f1{i} = 0x{b:X2}");
                    i++;
                }
            }
        }

        internal class ParentBlock : Block
        {
            public BlockT<int> size;
            public ExpandingBlock expandingBlock;
            public BlockT<int> footer;

            protected override void Parse()
            {
                size = ParseT<int>();
                parser.PushCap(size);
                expandingBlock = Parse<ExpandingBlock>();
                parser.PopCap();
                footer = ParseT<int>();
            }

            protected override void ParseBlocks()
            {
                SetText("ParentBlock");
                AddChild(size, $"size = 0x{size:X8}");
                AddChild(expandingBlock, "ExpandingBlock");
                AddChild(footer, $"footer = 0x{footer:X8}");
            }
        }

        [TestMethod]
        public void Test_TestExpandingBlock()
        {
            // CreateBlock a byte array for the parser to read (4 bytes size (3), 3 bytes, 4 bytes footer)
            // Then read this array using TestBlock
            byte[] data = new byte[] { 0x03, 0x0,0x0,0x0, 0x0A, 0x0B, 0x0C, 0x04, 0x05, 0x06, 0x07};
            var parser = new BinaryParser(data);
            var block = new ParentBlock();
            block.Parse(parser, true);

            Assert.AreEqual(3, block.size, "Array length read");
            Assert.AreEqual(3, block.expandingBlock.f1.Length, "ExpandingBlock's f1 length");
            Assert.AreEqual(0x0A, block.expandingBlock.f1[0], "First byte in f1 should be 0x0A");
            Assert.AreEqual(0x0B, block.expandingBlock.f1[1], "Second byte in f1 should be 0x0B");
            Assert.AreEqual(0x0C, block.expandingBlock.f1[2], "Third byte in f1 should be 0x0C");
            Assert.AreEqual(0x07060504, block.footer, "Footer should be 0x07060504");
        }

        internal class GrandParentBlock : Block
        {
            public BlockT<byte> size;
            public ParentBlock[] parents;
            public BlockT<int> footer;
            public BlockT<int> footer2;

            protected override void Parse()
            {
                size = ParseT<byte>();
                parser.PushCap(size);
                var _parents = new List<ParentBlock>();
                while (!parser.Empty)
                {
                    var parent = Parse<ParentBlock>();
                    if (!parent.Parsed) break;
                    _parents.Add(parent);
                }
                parents = _parents.ToArray();
                parser.PopCap();
                footer = ParseT<int>();
                footer2 = ParseT<int>();
            }

            protected override void ParseBlocks()
            {
                SetText("GrandParentBlock");
                AddChild(size, $"size = 0x{size:X8}");
                foreach (var parent in parents)
                {
                    AddChild(parent, "ParentBlock");
                }
                AddChild(footer, $"footer = 0x{footer:X8}");
                AddChild(footer2, $"footer2 = 0x{footer2:X8}");
            }
        }

        [TestMethod]
        public void Test_NestedSizedBlocks()
        {
            byte[] data = new byte[] { 0x15,
                0x03, 0x0,0x0,0x0, 0x0A, 0x0B, 0x0C, 0x04, 0x05, 0x06, 0x07,
                0x02, 0x0,0x0,0x0, 0x0D, 0x0E, 0x0A, 0x0B, 0x0C, 0x0D,
                0x01, 0x6, 0xFF, 0x0E,
                0xAA, 0xBB, 0xCC, 0x0D};
            var parser = new BinaryParser(data);
            var block = new GrandParentBlock();
            block.Parse(parser, true);
            Assert.AreEqual(21, block.size, "Array size read");
            Assert.AreEqual(2, block.parents.Length, "GrandParentBlock's parents length");
            Assert.AreEqual(3, block.parents[0].size, "First ParentBlock's size should be 3");
            Assert.AreEqual(0x0A, block.parents[0].expandingBlock.f1[0], "First byte in first ParentBlock's f1 should be 0x0A");
            Assert.AreEqual(0x0B, block.parents[0].expandingBlock.f1[1], "Second byte in first ParentBlock's f1 should be 0x0B");
            Assert.AreEqual(0x0C, block.parents[0].expandingBlock.f1[2], "Third byte in first ParentBlock's f1 should be 0x0C");
            Assert.AreEqual(0x07060504, block.parents[0].footer, "First ParentBlock's footer should be 0x07060504");
            Assert.AreEqual(2, block.parents[1].size, "Second ParentBlock's size should be 2");
            Assert.AreEqual(0x0D, block.parents[1].expandingBlock.f1[0], "First byte in second ParentBlock's f1 should be 0x0D");
            Assert.AreEqual(0x0E, block.parents[1].expandingBlock.f1[1], "Second byte in second ParentBlock's f1 should be 0x0E");
            Assert.AreEqual(0x0D0C0B0A, block.parents[1].footer, "Second ParentBlock's footer should be 0x0D0C0B0A");
            Assert.AreEqual(0x0EFF0601, block.footer, "GrandParentBlock's footer should be 0x0EFF0601");
            Assert.AreEqual(0x0DCCBBAA, block.footer2, "GrandParentBlock's footer2 should be 0xDDCCBBAA");
        }
    }
}