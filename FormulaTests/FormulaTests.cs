using Microsoft.VisualStudio.TestTools.UnitTesting;

using SpreadsheetUtilities;

using System.Collections;

using System.Collections.Generic;


namespace FormulaObjectTester

{

    [TestClass]

    public class FormulaObjectTester

    {

        [TestMethod]

        public void TestConstructorInt()

        {


            Assert.ThrowsException<FormulaFormatException>(() => new Formula("x + + 1"));

        }

        [TestMethod]

        public void TestTooManyRightParent()

        {


            Assert.ThrowsException<FormulaFormatException>(() => new Formula("(x + + 1 ))"));

        }

        [TestMethod]

        public void LeftParenthesisException()

        {


            Assert.ThrowsException<FormulaFormatException>(() => new Formula("((x)"));

        }

        [TestMethod]

        public void TestTooManyLeftParen()

        {

            Assert.ThrowsException<FormulaFormatException>(() => new Formula("((((x + + 1 ))"));

        }

        [TestMethod]

        public void SpacesBetweenNumbers()

        {

            Assert.ThrowsException<FormulaFormatException>(() => new Formula("1  1 + 1"));

        }

        [TestMethod]

        public void TestInvalidVar()

        {

            Assert.ThrowsException<FormulaFormatException>(() => new Formula("1x + 2", s => s, s => false));

        }

        [TestMethod]

        public void TestFirstTokenInValid()

        {

            Assert.ThrowsException<FormulaFormatException>(() => new Formula(")1 + 1"));

            Assert.ThrowsException<FormulaFormatException>(() => new Formula("1 + 1("));

        }


        [TestMethod]

        public void TestToString()

        {

            Formula test = new Formula("X + Y");

            Assert.AreEqual("X+Y", test.ToString());

        }

        [TestMethod]

        public void TestGetVariables()

        {

            Formula test = new Formula("X + Y");

            List<string> correct = new List<string>();

            correct.Add("X");

            correct.Add("Y");

            IEnumerable<string> final = correct;

            int i = 0;

            foreach (string s in test.GetVariables())

            {

                Assert.AreEqual(correct[i], s);

                i++;

            }

        }


        [TestMethod]

        public void TestEquals()

        {

            Assert.AreEqual(new Formula("X+Y"), new Formula("X + Y"));

        }

        [TestMethod]

        public void TestOverloadedEqualsOperator()

        {

            Assert.IsTrue(new Formula("X+Y") == new Formula("X+Y"));

        }


        [TestMethod]

        public void TestOverloadedNotEqualsOperator()

        {

            Assert.IsTrue(new Formula("X+X") != new Formula("X + Y"));

        }


        [TestMethod]

        public void TestEqualsNull()

        {

            Assert.IsFalse(new Formula("X+Y").Equals(null));

        }


        [TestMethod]

        public void TestGetHashCode()

        {

            Assert.IsTrue("X+Y".GetHashCode().Equals(new Formula("X+Y").GetHashCode()));

        }

        [TestMethod]

        public void TestNotEqualsOperatorNull()

        {

            Assert.IsTrue(new Formula("X+Y") != (null));

        }




        [TestMethod(), Timeout(5000)]

        [TestCategory("1")]

        public void TestSingleNumber()

        {

            Formula test = new Formula("5");

            Assert.AreEqual(5.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("2")]

        public void TestSingleVariable()

        {

            Formula test = new Formula("X5");

            Assert.AreEqual(13.0, test.Evaluate(s => 13));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("3")]

        public void TestAddition()

        {

            Formula test = new Formula("5+3");

            Assert.AreEqual(8.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("4")]

        public void TestSubtraction()

        {

            Formula test = new Formula("18-10");

            Assert.AreEqual(8.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("5")]

        public void TestMultiplication()

        {

            Formula test = new Formula("2*4");

            Assert.AreEqual(8.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("6")]

        public void TestDivision()

        {

            Formula test = new Formula("16/2");

            Assert.AreEqual(8.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("7")]

        public void TestArithmeticWithVariable()

        {

            Formula test = new Formula("2+X1");

            Assert.AreEqual(6.0, test.Evaluate(s => 4));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("9")]

        public void TestLeftToRight()

        {

            Formula test = new Formula("2*6+3");

            Assert.AreEqual(15.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("10")]

        public void TestOrderOperations()

        {

            Formula test = new Formula("2+6*3");

            Assert.AreEqual(20.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("11")]

        public void TestParenthesesTimes()

        {

            Formula test = new Formula("(2+6)*3");

            Assert.AreEqual(24.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("12")]

        public void TestTimesParentheses()

        {

            Formula test = new Formula("2*(3+5)");

            Assert.AreEqual(16.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("13")]

        public void TestPlusParentheses()

        {

            Formula test = new Formula("2+(3+5)");

            Assert.AreEqual(10.0, test.Evaluate(s => 0));


        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("14")]

        public void TestPlusComplex()

        {

            Formula test = new Formula("2+(3+5*9)");

            Assert.AreEqual(50.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("15")]

        public void TestOperatorAfterParens()

        {

            Formula test = new Formula("(1*1)-2/2");

            Assert.AreEqual(0.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("16")]

        public void TestComplexTimesParentheses()

        {

            Formula test = new Formula("2+3*(3+5)");

            Assert.AreEqual(26.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("17")]

        public void TestComplexAndParentheses()

        {

            Formula test = new Formula("2+3*5+(3+4*8)*5+2");

            Assert.AreEqual(194.0, test.Evaluate(s => 0));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("18")]

        public void TestDivideByZero()

        {

            Formula test = new Formula("5/0");

            Assert.IsTrue(test.Evaluate(s => 1) is FormulaError);

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("19")]

        [ExpectedException(typeof(FormulaFormatException))]

        public void TestSingleOperator()

        {

            Formula test = new Formula("+");

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("20")]

        [ExpectedException(typeof(FormulaFormatException))]

        public void TestExtraOperator()

        {

            Formula test = new Formula("2+5+");

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("21")]

        [ExpectedException(typeof(FormulaFormatException))]

        public void TestExtraParentheses()

        {

            Formula test = new Formula("2+5*7)");

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("22")]

        [ExpectedException(typeof(FormulaFormatException))]

        public void TestInvalidVariable()

        {

            Formula test = new Formula("xx", s => s, s => false);

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("23")]

        [ExpectedException(typeof(FormulaFormatException))]

        public void TestPlusInvalidVariable()

        {

            Formula test = new Formula("5+xx", s => s, s => false);

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("24")]

        [ExpectedException(typeof(FormulaFormatException))]

        public void TestParensNoOperator()

        {

            Formula test = new Formula("5+7+(5)8");

        }



        [TestMethod(), Timeout(5000)]

        [TestCategory("25")]

        [ExpectedException(typeof(FormulaFormatException))]

        public void TestEmpty()

        {

            Formula test = new Formula("");

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("26")]

        public void TestComplexMultiVar()

        {

            Formula test = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");

            Assert.AreEqual(5.142857142857142, test.Evaluate(s => (s == "x7") ? 1 : 4));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("27")]

        public void TestComplexNestedParensRight()

        {

            Formula test = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");

            Assert.AreEqual(6.0, test.Evaluate(s => 1));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("28")]

        public void TestComplexNestedParensLeft()

        {

            Formula test = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");

            Assert.AreEqual(12.0, test.Evaluate(s => 2));

        }


        [TestMethod(), Timeout(5000)]

        [TestCategory("29")]

        public void TestRepeatedVar()

        {

            Formula test = new Formula("a4-a4*a4/a4");

            Assert.AreEqual(0.0, test.Evaluate(s => 1));

        }


        [TestMethod]

        public void TestVarDivideBy0()

        {

            Formula test = new Formula("5 / X");

            Assert.IsTrue(test.Evaluate(lookup) is FormulaError);

        }

        [TestMethod]

        public void TestVarDivideBy0End()

        {

            Formula test = new Formula("5 / 1 / X");

            Assert.IsTrue(test.Evaluate(lookup) is FormulaError);

        }

        [TestMethod]

        public void TestVarDivideBy0InParenthesis()

        {

            Formula test = new Formula("((5 / 1) / X)");

            Assert.IsTrue(test.Evaluate(lookup) is FormulaError);

        }


        public static double lookup(string s)

        {

            if (s == "a1")

            {

                return 1;


            }

            else if (s == "b1")

            {

                return 2;

            }

            else if (s == "b3")

            {

                return 3;

            }

            else if (s == "bb3")

            {

                return 3;

            }

            else

                return 0;

        }

        [TestMethod]
        public void TestNullEquals()
        {
            Formula f = null;
            Assert.IsTrue(f == null);
        }

    }

}


