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
        [ExpectedException(typeof(DivideByZeroException))]
        public void EmptуExpressionTest()
        {
            double result = parseTree.Calculate("(/ 1 0)");
        }

        [TestMethod]
        public void UsualExpressionTest()
        {
            double result = parseTree.Calculate("(/ (+ (- 57 67) (/ 36 2)) (/ (* 8 8) (/ (- 80 84) 2)))");
            Assert.AreEqual(result, -0.25);
        }

        private ParseTree parseTree;
    }
}
