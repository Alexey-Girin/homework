namespace StackCalculator.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StackCalcTest
    {
        [TestInitialize]
        public void Initialize()
        {
            stackCalc = new StackCalc(new StackOnList());
        }

        [TestMethod]
        [ExpectedException(typeof(InputErrorExeption))]
        public void EmptуExpression()
        {
            stackCalc.Calculation("");
        }

        [TestMethod]
        public void Sum()
        {
            Assert.AreEqual(51, stackCalc.Calculation("21 30 +"));
        }

        [TestMethod]
        public void Subtraction()
        {
            Assert.AreEqual(-2, stackCalc.Calculation("23 25 -"));
        }

        [TestMethod]
        public void Multiplication()
        {
            Assert.AreEqual(21, stackCalc.Calculation("7 3 *"));
        }

        [TestMethod]
        public void Division()
        {
            Assert.AreEqual(0.2, stackCalc.Calculation("7 35 /"));
        }

        [TestMethod]
        [ExpectedException(typeof(InputErrorExeption))]
        public void DivisionByZero()
        {
            stackCalc.Calculation("7 0 /");
        }

        [TestMethod]
        public void CompoundExpression1()
        {
            Assert.AreEqual(-6.5, stackCalc.Calculation("21 5 + 4 8 - /"));
        }

        [TestMethod]
        public void CompoundExpression2()
        {
            Assert.AreEqual(-6, stackCalc.Calculation("1 2 + 4 - 12 2 / *"));
        }

        private StackCalc stackCalc;
    }
}
