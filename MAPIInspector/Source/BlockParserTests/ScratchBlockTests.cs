using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;

namespace BlockParserTests
{
    [TestClass]
    public class ScratchBlockTests
    {
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
