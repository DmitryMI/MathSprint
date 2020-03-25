using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MethodsLib.Tests
{
    [TestClass]
    public class MyCalcTests
    {
        // Этот должен быть положительным
        [TestMethod]
        public void TestMethod1()
        {
            // arrange
            int x = 10;
            int y = 20;
            int expected = 30;

            // act
            MyCalc c = new MyCalc();
            int actual = c.Sum(x, y);

            // assert
            Assert.AreEqual(expected, actual);
        }

        /*
        // Этот должен быть отрицательным
        [TestMethod]
        public void TestMethod2()
        {
            // arrange
            int x = 10;
            int y = 20;
            int expected = 20;

            // act
            MyCalc c = new MyCalc();
            int actual = c.Sum(x, y);

            // assert
            Assert.AreEqual(expected, actual);
        }
        */
    }
}
