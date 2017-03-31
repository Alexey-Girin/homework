namespace ParseTreeNamespace.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParseTreeTests
    {
        [TestInitialize]
        public void Initialize()
        {
            parseTree = new ParseTree();
        }

        [TestMethod]
        public void EmptуExpressionTest()
        {
            try
            {
                double result = parseTree.Calculate("(/ 1 0)");
            }
            catch(Exception e)
            {
                Assert.AreEqual(e.Message, "деление на ноль");
            }
        }

        [TestMethod]
        public void UsualExpressionTest()
        {
            double result = parseTree.Calculate("(/ (+ (- 57 67) (/ 36 2)) (/ (* 8 8) (/ (- 80 84) 2)))");
            Assert.AreEqual(result, -0.25);
        }

        ParseTree parseTree;
    }
}
