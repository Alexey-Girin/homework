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
        public void EmptуExpression()
        {
            try
            {
                stackCalc.Calculation("");
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "ошибка ввода");
            }
        }

        [TestMethod]
        public void Sum()
        {
            Assert.AreEqual(stackCalc.Calculation("21 30 +"), 51);
        }

        [TestMethod]
        public void Subtraction()
        {
            Assert.AreEqual(stackCalc.Calculation("23 25 -"), -2);
        }

        [TestMethod]
        public void Multiplication()
        {
            Assert.AreEqual(stackCalc.Calculation("7 3 *"), 21);
        }

        [TestMethod]
        public void Division()
        {
            Assert.AreEqual(stackCalc.Calculation("7 35 /"), 0.2);
        }

        [TestMethod]
        public void DivisionByZero()
        {
            try
            {
                stackCalc.Calculation("7 0 /");
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "ошибка ввода");
            }
        }

        [TestMethod]
        public void CompoundExpression1()
        {
            Assert.AreEqual(stackCalc.Calculation("21 5 + 4 8 - /"), -6.5);
        }

        [TestMethod]
        public void CompoundExpression2()
        {
            Assert.AreEqual(stackCalc.Calculation("1 2 + 4 - 12 2 / *"), -6);
        }

        private StackCalc stackCalc;
    }
}
