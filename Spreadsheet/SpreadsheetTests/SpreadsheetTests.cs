using Microsoft.VisualStudio.TestTools.UnitTesting
using SS;

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
    }
}
