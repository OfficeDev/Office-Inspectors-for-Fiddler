using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockParser.Tests
{
    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void ParseMAPITest()
        {
            // Arrange
            string expected = "Parsing MAPI data...";
            
            // Act
            string result = Parser.ParseMAPI();
            
            // Assert
            Assert.AreEqual(expected, result);
        }
	}
}