using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockParser;

namespace BlockParserTests
{
    [TestClass]
    public class BlockTests
    {
        private class TestBlock : Block
        {
            public bool ParseCalled { get; private set; }
            protected override void Parse()
            {
                ParseCalled = true;
            }
            protected override void ParseBlocks()
            {
                // No blocks to parse in TestBlock
            }
        }

        [TestMethod]
        public void SetText_AssignsText()
        {
            var b = Block.Create();
            b.SetText("abc");
            Assert.AreEqual("abc", b.Text);
        }

        [TestMethod]
        public void SetSize_AssignsSize()
        {
            var b = Block.Create();
            b.Size = 123;
            Assert.AreEqual(123, b.Size);
        }

        [TestMethod]
        public void SetOffset_AssignsOffset()
        {
            var b = Block.Create();
            b.Offset = 456;
            Assert.AreEqual(456, b.Offset);
        }

        [TestMethod]
        public void ShiftOffset_UpdatesOffsetAndChildren()
        {
            var parent = Block.Create();
            var child = Block.Create();
            parent.AddChild(child);
            parent.Offset = 10;
            child.Offset = 20;
            parent.ShiftOffset(5);
            Assert.AreEqual(15, parent.Offset);
            Assert.AreEqual(25, child.Offset);
        }

        [TestMethod]
        public void SetSource_UpdatesSourceAndChildren()
        {
            var parent = Block.Create();
            var child = Block.Create();
            parent.AddChild(child);
            parent.Source = 99;
            Assert.AreEqual((uint)99, parent.Source);
            Assert.AreEqual((uint)99, child.Source);
        }

        [TestMethod]
        public void IsHeader_TrueWhenSizeAndOffsetZero()
        {
            var b = Block.Create();
            Assert.IsTrue(b.IsHeader);
        }

        [TestMethod]
        public void HasData_TrueWhenTextOrChildren()
        {
            var b = Block.Create();
            Assert.IsFalse(b.HasData);
            b.SetText("abc");
            Assert.IsTrue(b.HasData);
            var b2 = Block.Create();
            b2.AddChild(Block.Create());
            Assert.IsTrue(b2.HasData);
        }

        [TestMethod]
        public void AddChild_AddsChildIfSet()
        {
            var b = Block.Create();
            var child = Block.Create();
            b.AddChild(child);
            Assert.AreEqual(1, b.Children.Count);
        }

        [TestMethod]
        public void AddChild_WithText_SetsText()
        {
            var b = Block.Create();
            var child = Block.Create();
            b.AddChild(child, "childtext");
            Assert.AreEqual("childtext", child.Text);
            Assert.AreEqual(1, b.Children.Count);
        }

        [TestMethod]
        public void AddHeader_AddsTextOnlyNode()
        {
            var b = Block.Create();
            b.AddHeader("header");
            Assert.AreEqual(1, b.Children.Count);
            Assert.AreEqual("header", b.Children[0].Text);
        }

        [TestMethod]
        public void AddLabeledChild_AddsNodeWithChild()
        {
            var b = Block.Create();
            var child = Block.Create();
            child.Offset = 5;
            child.Size = 10;
            b.AddLabeledChild(child, "label");
            Assert.AreEqual(1, b.Children.Count);
            Assert.AreEqual("label", b.Children[0].Text);
            Assert.AreEqual(5, b.Children[0].Offset);
            Assert.AreEqual(10, b.Children[0].Size);
            Assert.AreEqual(child, b.Children[0].Children[0]);
        }

        [TestMethod]
        public void AddSubHeader_AddsNodeWithParentOffsetSize()
        {
            var b = Block.Create();
            b.Offset = 2;
            b.Size = 3;
            b.AddSubHeader("sub");
            Assert.AreEqual(1, b.Children.Count);
            Assert.AreEqual("sub", b.Children[0].Text);
            Assert.AreEqual(2, b.Children[0].Offset);
            Assert.AreEqual(3, b.Children[0].Size);
        }

        [TestMethod]
        public void Create_ReturnsScratchBlock()
        {
            var b = Block.Create();
            Assert.IsInstanceOfType(b, typeof(ScratchBlock));
        }

        [TestMethod]
        public void Create_WithSizeOffsetText()
        {
            var b = Block.Create(10, 20, $"t{1}");
            Assert.AreEqual(10, b.Size);
            Assert.AreEqual(20, b.Offset);
            Assert.AreEqual("t1", b.Text);
        }

        [TestMethod]
        public void Create_WithText()
        {
            var b = Block.Create($"abc{2}");
            Assert.AreEqual("abc2", b.Text);
        }

        [TestMethod]
        public void Parse_StaticAndInstance_CallsParse()
        {
            var emptyParser = new BinaryParser();
            var tbEmpty = Block.Parse<TestBlock>(emptyParser);
            Assert.IsFalse(tbEmpty.ParseCalled);
            emptyParser.Rewind();

            var tbEmpty2 = new TestBlock();
            tbEmpty2.Parse(emptyParser, false);
            Assert.IsFalse(tbEmpty2.ParseCalled);

            var fullParser = new BinaryParser(new byte[] { 1, 2, 3, 4 });
            var tbFull = Block.Parse<TestBlock>(fullParser);
            Assert.IsTrue(tbFull.ParseCalled);
            fullParser.Rewind();

            var tbFull2 = new TestBlock();
            tbFull2.Parse(fullParser, false);
            Assert.IsTrue(tbFull2.ParseCalled);
        }

        [TestMethod]
        public void ToStringBlock_EnsuresParsedAndFormats()
        {
            var tb = new TestBlock();
            tb.SetText("abc");
            Assert.IsTrue(tb.ToString().Contains("abc"));
        }

        [TestMethod]
        public void Children_IsReadOnly()
        {
            var b = Block.Create();
            Assert.IsInstanceOfType(b.Children, typeof(IReadOnlyList<Block>));
        }

        [TestMethod]
        public void SetText_Null_SetsEmpty()
        {
            var b = Block.Create();
            b.SetText(null);
            Assert.AreEqual(string.Empty, b.Text);
        }

        [TestMethod]
        public void AddChild_DoesNotAddNull()
        {
            var b = Block.Create();
            b.AddChild(null);
            Assert.AreEqual(0, b.Children.Count);
        }

        [TestMethod]
        public void AddChild_WithText_NullText_SetsEmpty()
        {
            var b = Block.Create();
            var child = Block.Create();
            b.AddChild(child, null);
            Assert.AreEqual(string.Empty, child.Text);
        }

        [TestMethod]
        public void AddHeader_NullText_SetsEmpty()
        {
            var b = Block.Create();
            b.AddHeader(null);
            Assert.AreEqual(string.Empty, b.Children[0].Text);
        }

        [TestMethod]
        public void AddLabeledChild_NullLabel_SetsEmpty()
        {
            var b = Block.Create();
            var child = Block.Create();
            b.AddLabeledChild(child, null);
            Assert.AreEqual(string.Empty, b.Children[0].Text);
        }

        [TestMethod]
        public void AddSubHeader_NullText_SetsEmpty()
        {
            var b = Block.Create();
            b.AddSubHeader(null);
            Assert.AreEqual(string.Empty, b.Children[0].Text);
        }

        [TestMethod]
        public void SetSource_PropagatesToNestedChildren()
        {
            var parent = Block.Create();
            var child = Block.Create();
            var grandchild = Block.Create();
            parent.AddChild(child);
            child.AddChild(grandchild);
            parent.Source = 123;
            Assert.AreEqual((uint)123, grandchild.Source);
        }

        [TestMethod]
        public void ShiftOffset_PropagatesToNestedChildren()
        {
            var parent = Block.Create();
            var child = Block.Create();
            var grandchild = Block.Create();
            parent.AddChild(child);
            child.AddChild(grandchild);
            parent.Offset = 1;
            child.Offset = 2;
            grandchild.Offset = 3;
            parent.ShiftOffset(10);
            Assert.AreEqual(11, parent.Offset);
            Assert.AreEqual(12, child.Offset);
            Assert.AreEqual(13, grandchild.Offset);
        }

        [TestMethod]
        public void ToStringBlock_WithChildren()
        {
            var b = Block.Create();
            var c = Block.Create();
            c.SetText("child");
            b.AddChild(c);
            Assert.IsTrue(b.FullString().Contains("child"));
        }

        [TestMethod]
        public void UsePipes_DefaultFalse()
        {
            var b = Block.Create();
            var method = b.GetType().GetMethod("UsePipes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = (bool)method.Invoke(b, null);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ParseBlocks_Virtual_NoException()
        {
            var b = Block.Create();
            var method = b.GetType().GetMethod("ParseBlocks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(b, null);
            Assert.IsTrue(true); // No exception means pass
        }
    }
}
