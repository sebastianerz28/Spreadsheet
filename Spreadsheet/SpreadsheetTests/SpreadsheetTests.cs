using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.Xml;

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
            test.SetContentsOfCell("A4", "2");
            Assert.AreEqual(2.0, test.GetCellContents("A4"));
        }

        [TestMethod]
        public void TestGetCellContentsText()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A4", "X");
            Assert.AreEqual("X", test.GetCellContents("A4"));
        }

        [TestMethod]
        public void TestGetCellContentsFormula()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A4", "=A1 + 2");
            Assert.AreEqual("A1+2", test.GetCellContents("A4").ToString());
        }
        [TestMethod]
        public void TestSetCellContentsExistingDouble()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "1.0");
            test.SetContentsOfCell("A1", "2.0");
            Assert.AreEqual(2.0, test.GetCellContents("A1"));
            Assert.AreEqual(2.0, test.GetCellValue("A1"));
        }

        [TestMethod]
        public void TestSetCellContentsExistingString()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "A3");
            test.SetContentsOfCell("A1", "A2");
            Assert.AreEqual("A2", test.GetCellContents("A1"));
            Assert.AreEqual("A2", test.GetCellValue("A1"));
        }

        [TestMethod]
        public void TestSetCellContentsExistingFormula()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "1.0");
            test.SetContentsOfCell("A1", "=X+Y");
            Assert.AreEqual("X+Y", test.GetCellContents("A1").ToString());
            Assert.IsTrue(test.GetCellValue("A1") is FormulaError);
        }

        [TestMethod]
        public void TestCellReliance()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "2.0");
            test.SetContentsOfCell("A2", "=A1 + 1");
            Assert.AreEqual(3.0, test.GetCellValue("A2"));
        }
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestCircularException()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "=B1 * 2");
            test.SetContentsOfCell("B1", "=C1 * 2");
            test.SetContentsOfCell("C1", "=A1 * 2");
            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestDoubleInvalidName()
        {
            AbstractSpreadsheet test = new Spreadsheet(s => false, s => s, "base");
            test.SetContentsOfCell("1x", "2.0");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestTextInvalidName()
        {
            AbstractSpreadsheet test = new Spreadsheet(s => false, s => s, "base");
            test.SetContentsOfCell("1x", "x");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestFormulaInvalidName()
        {
            AbstractSpreadsheet test = new Spreadsheet(s => false, s => s, "base");
            test.SetContentsOfCell("1X",  ("=2"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestFormulaIsNull()
        {
            AbstractSpreadsheet test = new Spreadsheet(s=>false, s=>s,"base");
            Formula f = null;
            test.SetContentsOfCell("1X", "sdf");
            
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameIsNullDouble()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell((string)null, "2.0");

        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameIsNullText()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell((string)null, "x");

        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameIsNull()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell((string)null, "=x");

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
            AbstractSpreadsheet test = new Spreadsheet(s => false, s => s, "base") ;
            test.GetCellContents("1a");
        }

        [TestMethod]
        public void TestGetNonemptyCells()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "2.0");
            test.SetContentsOfCell("b11", "2.0");
            test.SetContentsOfCell("b31", "2.0");
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
            test.SetContentsOfCell("A1", null);

        }
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestCircular()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "=B1 + 2");
            test.SetContentsOfCell("B1", "=A1");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullText()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            string s = null;
            test.SetContentsOfCell("A1", s);
        }

        [TestMethod]
        public void TestDependentCellsText()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("B1", "=A1 * 2");
            test.SetContentsOfCell("C1", ("=A1 * B1"));
            Assert.IsTrue(test.SetContentsOfCell("A1", "=F1").Contains("B1"));
            Assert.IsTrue(test.SetContentsOfCell("A1", "=F1").Contains("A1"));
            Assert.IsTrue(test.SetContentsOfCell("A1", "=F1").Contains("C1"));
        }
        [TestMethod]
        public void TestBasicGetCellContents()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("B3","= A1 + 2");
            test.SetContentsOfCell("A1", "2.0");
            Assert.AreEqual(4.0, test.GetCellValue("B3"));
        }
        [TestMethod]
        public void TestGetValueTwoNestedFormulas()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("B3", "= A1 + 2");
            test.SetContentsOfCell("A1", "=A3 + A4");
            test.SetContentsOfCell("A3", "2.0");
            test.SetContentsOfCell("A4", "2.0");
            Assert.AreEqual(6.0, test.GetCellValue("B3"));
        }
        [TestMethod]
        public void TestGetValueString()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("B3", "Test");
            Assert.AreEqual("Test", test.GetCellValue("B3"));
        }

        [TestMethod]
        public void TestIsFormulaError()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("B3", "Test");
            test.SetContentsOfCell("A1", "=B3");
            Assert.IsTrue(test.GetCellValue("A1") is FormulaError);
        }
        [TestMethod]
        public void TestSpreadSheetSaveOneString()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("B3", "Test");
            test.Save("xmlBasic1.txt");
        }
        [TestMethod]
        public void TestSpreadSheetSaveOneFormula()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("B3", "=A4");
            test.Save("xmlBasic2.txt");
        }
        [TestMethod]
        public void TestSpreadSheetSaveOneDouble()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("B3", "2.0");
            test.Save("xmlBasic3.txt");
        }

        [TestMethod]
        public void TestSaveMultipleCells()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("B1", "1.0");
            test.SetContentsOfCell("B2", "2.0");
            test.SetContentsOfCell("B3", "3.0");
            test.Save("TestSaveMultipleCells.txt");

            using (XmlWriter writer = XmlWriter.Create("TestSaveMultipleCells.txt")) // NOTICE the file with no path
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "default");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "B1");
                writer.WriteElementString("contents", "1.0");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "B2");
                writer.WriteElementString("contents", "2.0");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "B3");
                writer.WriteElementString("contents", "3.0");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            Assert.AreEqual("default", test.GetSavedVersion("TestSaveMultipleCells.txt"));

        }
        [TestMethod]
        public void ReadOneString()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("B3", "Test");
            Assert.AreEqual("Test", test.GetCellValue("B3"));
            test.Save("ReadBasic.txt");
            using (XmlWriter writer = XmlWriter.Create("ReadBasic.txt")) // NOTICE the file with no path
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "default");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "B3");
                writer.WriteElementString("contents", "Test");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet builtSS = new Spreadsheet("ReadBasic.txt", s=>true, s=>s, "1.0");
            Assert.AreEqual("Test", builtSS.GetCellContents("B3"));
        }
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSavedVersionException()
        {

            using (XmlWriter writer = XmlWriter.Create("nothing.txt")) // NOTICE the file with no path
            {
            }
            AbstractSpreadsheet test = new Spreadsheet();
            test.GetSavedVersion("nothing.txt");
        }
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestBuildException()
        {
            using (XmlWriter writer = XmlWriter.Create("bogus.txt")) // NOTICE the file with no path
            {
            }
            AbstractSpreadsheet test = new Spreadsheet("bogus.txt", s=>true, s=>s, "1.1");
            
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestNoSpreadSheetTag()
        {
            using (XmlWriter writer = XmlWriter.Create("NoVersion.txt")) // NOTICE the file with no path
            {
                writer.WriteStartDocument();
                
                

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("contents", "hello");
                writer.WriteEndElement();

                
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet test = new Spreadsheet();
            test.GetSavedVersion("NoVersion.txt");
        }
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestNoVersion()
        {
            using (XmlWriter writer = XmlWriter.Create("NoSSTag.txt")) // NOTICE the file with no path
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");


                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("contents", "hello");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet test = new Spreadsheet();
            test.GetSavedVersion("NoTag.txt");
            
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValueNameNulll()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.GetCellValue(null);

        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValueNotAVar()
        {
            AbstractSpreadsheet test = new Spreadsheet();
            test.GetCellValue("11111");

        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValueNotValid()
        {
            AbstractSpreadsheet test = new Spreadsheet(s=>false, s=>s, "base");
            test.GetCellValue("A1");

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestChanged()
        {
            AbstractSpreadsheet test = new Spreadsheet(s => false, s => s, "base");
            test.GetCellValue("A1");

        }

    }
}
