﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.MathTrials;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assets.Scripts.MathTrials.Exercises;
using Assets.Scripts.Utils;

namespace MethodsLib.Tests
{
    [TestClass]
    public class MyCalcTests
    {
        /// <summary>
        /// Testing of XML exercise parsing
        /// </summary>
        [TestMethod]
        public void TestExerciseLoader()
        {
            string xml =
                "<Exercise>\r\n\t<Image>Example</Image>\r\n\t<Text>Example text for the exercise</Text>\r\n\t<Answer>0</Answer>\r\n</Exercise>";
            var parsedData =  ExercisesLoader.ParseXml(xml);

            if (!parsedData.Text.Equals("Example text for the exercise"))
            {
                Assert.Fail("Text does not match xml source");
            }
            if (!parsedData.ImageName.Equals("Example"))
            {
                Assert.Fail("Image name d=oes not match xml source");
            }
            if (!parsedData.Answer.Equals("0"))
            {
                Assert.Fail("Answer does not match xml source");
            }
        }

        /// <summary>
        /// Testing of ArrayUtils class
        /// </summary>
        [TestMethod]
        public void TestArrayUtils()
        {
            List<int> items = new List<int>() {1, 2, 3, 4};
            int item = ArrayUtils.GetRandomItem(items);
            bool contains = items.Contains(item);
            Assert.IsTrue(contains);
        }

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

        // Должен быть положительным
        [TestMethod]
        public void TestMethod3()
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


    }

    [TestClass]
    public class MyAnotherTestClass
    {
        // Должен быть положительным
        [TestMethod]
        public void TestMethod3()
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
    }

}
