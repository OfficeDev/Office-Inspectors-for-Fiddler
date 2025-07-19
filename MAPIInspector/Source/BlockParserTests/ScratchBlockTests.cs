using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockParser;

namespace BlockParserTests
{
    [TestClass]
    public class ScratchBlockTests
    {
        [TestMethod]
        public void ScratchBlock_CanSetAndGetText()
        {
            var block = new ScratchBlock();
            block.Text = "TestText";
            Assert.AreEqual("TestText", block.Text);
        }

        [TestMethod]
        public void ScratchBlock_ToStringBlock_ReturnsExpected()
        {
            var block = new ScratchBlock();
            block.Text = "TestBlock";
            Assert.IsTrue(block.ToString().Contains("TestBlock"));
        }
    }
}
