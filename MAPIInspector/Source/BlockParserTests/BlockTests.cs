using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;

namespace Parser.Tests
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
        }

        [TestMethod]
        public void SetText_AssignsText()
        {
            var b = Block.Create();
            b.SetText("abc");
            Assert.AreEqual("abc", b.Text);
        }

        [TestMethod]
        public void SetText_Format_AssignsFormattedText()
        {
            var b = Block.Create();
            b.SetText("Value: {0}", 42);
            Assert.AreEqual("Value: 42", b.Text);
        }

        [TestMethod]
        public void SetSize_AssignsSize()
        {
            var b = Block.Create();
            b.SetSize(123);
            Assert.AreEqual(123, b.Size);
        }

        [TestMethod]
        public void SetOffset_AssignsOffset()
        {
            var b = Block.Create();
            b.SetOffset(456);
            Assert.AreEqual(456, b.Offset);
        }

        [TestMethod]
        public void ShiftOffset_UpdatesOffsetAndChildren()
        {
            var parent = Block.Create();
            var child = Block.Create();
            parent.AddChild(child);
            parent.SetOffset(10);
            child.SetOffset(20);
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
            parent.SetSource(99);
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
        public void AddChild_WithFormat_SetsFormattedText()
        {
            var b = Block.Create();
            var child = Block.Create();
            b.AddChild(child, "val={0}", 7);
            Assert.AreEqual("val=7", child.Text);
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
        public void AddHeader_Format_AddsFormattedTextOnlyNode()
        {
            var b = Block.Create();
            b.AddHeader("h{0}", 1);
            Assert.AreEqual("h1", b.Children[0].Text);
        }

        [TestMethod]
        public void AddLabeledChild_AddsNodeWithChild()
        {
            var b = Block.Create();
            var child = Block.Create();
            child.SetOffset(5);
            child.SetSize(10);
            b.AddLabeledChild("label", child);
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
            b.SetOffset(2);
            b.SetSize(3);
            b.AddSubHeader("sub");
            Assert.AreEqual(1, b.Children.Count);
            Assert.AreEqual("sub", b.Children[0].Text);
            Assert.AreEqual(2, b.Children[0].Offset);
            Assert.AreEqual(3, b.Children[0].Size);
        }

        [TestMethod]
        public void AddSubHeader_Format_AddsFormattedNode()
        {
            var b = Block.Create();
            b.AddSubHeader("s{0}", 9);
            Assert.AreEqual("s9", b.Children[0].Text);
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
            var b = Block.Create(10, 20, "t{0}", 1);
            Assert.AreEqual(10, b.Size);
            Assert.AreEqual(20, b.Offset);
            Assert.AreEqual("t1", b.Text);
        }

        [TestMethod]
        public void Create_WithText()
        {
            var b = Block.Create("abc{0}", 2);
            Assert.AreEqual("abc2", b.Text);
        }

        [TestMethod]
        public void Parse_StaticAndInstance_CallsParse()
        {
            var emptyParser = new BinaryParser();
            var tbEmpty = Block.Parse<TestBlock>(emptyParser, false);
            Assert.IsFalse(tbEmpty.ParseCalled);
            emptyParser.Rewind();

            var tbEmpty2 = new TestBlock();
            tbEmpty2.Parse(emptyParser, false);
            Assert.IsFalse(tbEmpty2.ParseCalled);

            var fullParser = new BinaryParser(new byte[] { 1, 2, 3, 4 });
            var tbFull = Block.Parse<TestBlock>(fullParser, false);
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
            var s = tb.ToStringBlock();
            Assert.IsTrue(s.Contains("abc"));
        }

        [TestMethod]
        public void Children_IsReadOnly()
        {
            var b = Block.Create();
            Assert.IsInstanceOfType(b.Children, typeof(IReadOnlyList<Block>));
        }
    }
}
