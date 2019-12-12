using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExpressionEvaulation;
using System.Collections.Generic;
using System;

namespace ExpressionTests
{
    [TestClass]
    public class EvaluationTests
    {
        [TestMethod]
        public void SplitInputToQueueOfStringsTest1()
        {
            string input = "/ + - 5 2 * 2 + 3 3 ~ 2";
            Expression ex = new Expression();
            Queue<string> splitted = ex.SplitInputToQueueOfStrings(input);

            string[] correct = input.Split(' ');

            foreach (var item in splitted)
            {
                System.Console.WriteLine(item);
            }

            for (int i = 0; i < correct.Length; i++)
            {
                string test = splitted.Dequeue();
                Assert.AreEqual(correct[i], test);
            }

            if (splitted.Count > 0)
            {
                Assert.Fail();
            }

           
        }
        [TestMethod]
        public void SplitInputToQueueOfStringsTest2()
        {
            string input = "/ + - 5000 212311 * 222221 + 2223 2223 ~ 2";
            Expression ex = new Expression();
            Queue<string> splitted = ex.SplitInputToQueueOfStrings(input);

            string[] correct = input.Split(' ');

            foreach (var item in splitted)
            {
                System.Console.WriteLine(item);
            }

            for (int i = 0; i < correct.Length; i++)
            {
                string test = splitted.Dequeue();
                Assert.AreEqual(correct[i], test);
            }

            if (splitted.Count > 0)
            {
                Assert.Fail();
            }


        }
        [TestMethod]
        public void SplitInputToQueueOfStringsTest3()
        {
            string input = "/ + - +";
            Expression ex = new Expression();
            Queue<string> splitted = ex.SplitInputToQueueOfStrings(input);

            string[] correct = input.Split(' ');

            foreach (var item in splitted)
            {
                System.Console.WriteLine(item);
            }

            for (int i = 0; i < correct.Length; i++)
            {
                string test = splitted.Dequeue();
                Assert.AreEqual(correct[i], test);
            }

            if (splitted.Count > 0)
            {
                Assert.Fail();
            }


        }
        [TestMethod]
        public void SplitInputToQueueOfStringsTest4()
        {
            string input = "123456789";
            Expression ex = new Expression();
            Queue<string> splitted = ex.SplitInputToQueueOfStrings(input);

            string[] correct = input.Split(' ');

            foreach (var item in splitted)
            {
                System.Console.WriteLine(item);
            }

            for (int i = 0; i < correct.Length; i++)
            {
                string test = splitted.Dequeue();
                Assert.AreEqual(correct[i], test);
            }

            if (splitted.Count > 0)
            {
                Assert.Fail();
            }


        }

        [TestMethod]
        public void ReadExpressionTest()
        {
            string input = "/ + - 5 2 * 2 + 3 3 ~ 2";
            Expression ex = new Expression();
            ex.ReadExpression(input);
            string correct = "\t-2\n/\n\t\t\t\t3\n\t\t\t+\n\t\t\t\t3\n\t\t*\n\t\t\t2\n\t+\n\t\t\t2\n\t\t-\n\t\t\t5\n";
            string s = ex.ExpressionTreeToString();
            
            Console.WriteLine(s);
            Assert.AreEqual(correct, s);

        }
        [TestMethod]
        public void ReadExpressionTest2()
        {
            string input = "";
            Expression ex = new Expression();
            bool b=ex.ReadExpression(input);
            System.Console.WriteLine(ex.ExpressionTreeToString());
            Assert.IsFalse(b);
        }
        [TestMethod]
        public void ReadExpressionTest3()
        {
            string input = "+ 1";
            Expression ex = new Expression();
            bool b =ex.ReadExpression(input);
            Assert.IsFalse(b);

        }
        [TestMethod]
        public void ReadExpressionTest4()
        {
            string input = "1 2 3 4 5 ";
            Expression ex = new Expression();
            bool b =ex.ReadExpression(input);
            string s = ex.ExpressionTreeToString();
            Console.WriteLine(s);
            Assert.IsFalse(b);

        }
        [TestMethod]
        public void ReadExpressionTest5()
        {
            string input = "* + -";
            Expression ex = new Expression();
            bool b = ex.ReadExpression(input);
            string s = ex.ExpressionTreeToString();
            Console.WriteLine(s);
            Assert.IsFalse(b);
        }
        [TestMethod]
        public void ReadExpressionTest6()
        {
            string input = "* mnau";
            Expression ex = new Expression();
            bool b = ex.ReadExpression(input);
            string s = ex.ExpressionTreeToString();
            Console.WriteLine(s);
            Assert.IsFalse(b);
        }
        [TestMethod]
        public void ReadExpressionTest7()
        {
            string input = "- 2000000000 4000000000";
            Expression ex = new Expression();
            bool b = ex.ReadExpression(input);
            string s = ex.ExpressionTreeToString();
            Console.WriteLine(s);
            Assert.IsFalse(b);
        }
        [TestMethod]
        public void EvaluateTest()
        {
            string input = "/ + - 5 2 * 2 + 3 3 ~ 2";
            Expression ex = new Expression();
            ex.ReadExpression(input);
            ex.Evaluate();
            Assert.AreEqual(-7, ex.Result);
            Console.WriteLine(ex.Result);
        }
        [TestMethod]
        public void EvaluateTest2()
        {
            string input = "+ 1 2 3";
            Expression ex = new Expression();
            ex.ReadExpression(input);
            int? a= ex.Evaluate();
            Assert.IsNull(a);
            Console.WriteLine(ex.Result);
            Assert.IsNull(a);
        }

    }
}
