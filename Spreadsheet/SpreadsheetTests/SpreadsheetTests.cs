using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        [TestMethod]
        public void TestInitialize()
        {
            AbstractSpreadsheet test = new Spreadsheet();
        }

        [TestMethod]
        public void TestGetCellContentsDouble()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("A4", 2);
            Assert.AreEqual(2.0, test.GetCellContents("A4"));
        }

        [TestMethod]
        public void TestGetCellContentsText()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("A4", "X");
            Assert.AreEqual("X", test.GetCellContents("A4"));
        }

        [TestMethod]
        public void TestGetCellContentsFormula()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("A4", new Formula("A1 + 2"));
            Assert.AreEqual("A1+2", test.GetCellContents("A4").ToString());
        }
        [TestMethod]
        public void TestSetCellContentsExistingDouble()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", 1);
            test.SetCellContents("A1", 2);
            Assert.AreEqual(2.0, test.GetCellContents("A1"));
        }

        [TestMethod]
        public void TestSetCellContentsExistingString()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", "A3");
            test.SetCellContents("A1", "A2");
            Assert.AreEqual("A2", test.GetCellContents("A1"));
        }

        [TestMethod]
        public void TestSetCellContentsExistingFormula()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", 1);
            test.SetCellContents("A1", new Formula("X+Y"));
            Assert.AreEqual("X+Y", test.GetCellContents("A1").ToString());
        }

        [TestMethod]
        public void TestCellReliance()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", 2);
            test.SetCellContents("A2", new Formula("A1 + 1"));
        }
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestCircularException()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", new Formula("B1 * 2"));
            test.SetCellContents("B1", new Formula("C1 * 2"));
            test.SetCellContents("C1", new Formula("A1 * 2"));
            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestDoubleInvalidName()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("1x", 2);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestTextInvalidName()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("1x", "x");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestFormulaInvalidName()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("1X",  new Formula("2"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestFormulaIsNull()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            Formula f = null;
            test.SetCellContents("1X", f);
            
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameIsNullDouble()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents((string)null, 2.0);

        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameIsNullText()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents((string)null, "x");

        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameIsNull()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents((string)null, new Formula("x"));

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetNull()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.GetCellContents(null);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetInvalid()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.GetCellContents("1a");
        }

        [TestMethod]
        public void TestGetNonemptyCells()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", 2.0);
            test.SetCellContents("b11", 2.0);
            test.SetCellContents("b31", 2.0);
            List<string> s = new List<string>();
            s.Add("A1");
            s.Add("b11");
            s.Add("b31");
            IEnumerator<string> e =  test.GetNamesOfAllNonemptyCells().GetEnumerator();
            for(int i = 0; i<3; i++)
            {
                e.MoveNext();
                Assert.IsTrue(e.Current.Equals(s[i]));
            }
        }
        [TestMethod]
        
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullFormula()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            Formula f = null;
            test.SetCellContents("A1", f);

        }
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestCircular()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", new Formula("B1 + 2"));
            test.SetCellContents("B1", new Formula("A1"));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullText()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            string s = null;
            test.SetCellContents("A1", s);
        }

        [TestMethod]
        public void TestDependentCellsText()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetCellContents("B1", new Formula("A1 * 2"));
            test.SetCellContents("C1", new Formula("A1 * B1"));
            Assert.IsTrue(test.SetCellContents("A1", "F1").Contains("B1"));
            Assert.IsTrue(test.SetCellContents("A1", "F1").Contains("A1"));
            Assert.IsTrue(test.SetCellContents("A1", "F1").Contains("C1"));
        }

    }
}
