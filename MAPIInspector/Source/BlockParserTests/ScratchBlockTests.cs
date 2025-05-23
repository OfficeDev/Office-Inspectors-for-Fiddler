using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;

namespace BlockParserTests
{
    [TestClass]
    public class ScratchBlockTests
    {
        [TestMethod]
        public void ScratchBlock_Constructor_SetsParsedTrue()
        {
            var block = new ScratchBlock();
            var parsedField = typeof(ScratchBlock).BaseType.GetField("parsed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsTrue((bool)parsedField.GetValue(block));
        }

        [TestMethod]
        public void ScratchBlock_Parse_DoesNotThrow()
        {
            var block = new ScratchBlock();
            block.GetType().GetMethod("Parse", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(block, null);
        }

        [TestMethod]
        public void ScratchBlock_CanSetAndGetText()
        {
            var block = new ScratchBlock();
            block.SetText("TestText");
            Assert.AreEqual("TestText", block.Text);
        }

        [TestMethod]
        public void ScratchBlock_ToStringBlock_ReturnsExpected()
        {
            var block = new ScratchBlock();
            block.SetText("TestBlock");
            var str = block.ToStringBlock();
            Assert.IsTrue(str.Contains("TestBlock"));
        }
    }
}
