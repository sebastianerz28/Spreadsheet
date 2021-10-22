//Sebastian Ramirez
using SpreadsheetUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private DependencyGraph dependencies;
        private Dictionary<string, Cell> cells;
        private Dictionary<string, double> evaluatedCells;
        private bool changed;

        public Spreadsheet() : base(s => true, s => s, "default")
        {
            dependencies = new DependencyGraph();
            cells = new Dictionary<string, Cell>();
            changed = false;
            evaluatedCells = new Dictionary<string, double>();
        }
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {

            evaluatedCells = new Dictionary<string, double>();
            dependencies = new DependencyGraph();
            cells = new Dictionary<string, Cell>();
            changed = false;
        }
        public Spreadsheet(string filepath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {

            evaluatedCells = new Dictionary<string, double>();
            dependencies = new DependencyGraph();
            cells = new Dictionary<string, Cell>();
            changed = false;
            BuildSpreadsheet(filepath);

        }
        /// <summary>
        /// Takes in a file and builds spreadsheet from file
        /// </summary>
        /// <param name="filepath"></param>
        private void BuildSpreadsheet(string filepath)
        {
            try
            {
                string name = null;
                string contents = null;
                XmlReader xmlReader = XmlReader.Create(filepath);
                while (xmlReader.Read())
                {
                    if (xmlReader.IsStartElement())
                    {

                        if (xmlReader.Name == "name")
                        {
                            xmlReader.Read();
                            name = xmlReader.Value;
                        }

                        else if (xmlReader.Name == "contents")
                        {
                            xmlReader.Read();
                            contents = xmlReader.Value;
                            SetContentsOfCell(name, contents);
                            changed = true;
                        }

                    }


                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException("Spreadsheet could not be built");
            }
            if (changed == false)
                throw new SpreadsheetReadWriteException("Spreadsheet not built");
        }

        // ADDED FOR PS5
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get => changed;
            protected set => changed = value;
        }




        // ADDED FOR PS5
        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            string v = "";
            bool foundSpreadSheet = false;
            try
            {
                XmlReader xmlReader = XmlReader.Create(filename);
                while (xmlReader.Read())
                {
                    if (xmlReader.Name == "spreadsheet")
                    {


                        v = xmlReader["version"];
                        foundSpreadSheet = true;
                        break;
                    }
                }
                if (foundSpreadSheet == false)
                    throw new SpreadsheetReadWriteException("Version Not Found");
            }
            catch
            {
                throw new SpreadsheetReadWriteException("Error Getting Saved Version");
            }

            if (v == null)
            {
                throw new SpreadsheetReadWriteException("version not found");
            }
            return v;
        }
        // ADDED FOR PS5
        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        public override void Save(string filename)
        {

            try
            {
                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.Indent = true;
                XmlWriter xmlWriter = XmlWriter.Create(filename, writerSettings);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("spreadsheet");
                xmlWriter.WriteAttributeString("version", Version);
                foreach (string s in cells.Keys)
                {
                    xmlWriter.WriteStartElement("cell");
                    xmlWriter.WriteElementString("name", s);
                    if (cells[s].GetContents() is Formula)
                    {
                        string eql = "=" + cells[s].GetContents().ToString();
                        xmlWriter.WriteElementString("contents", eql);
                    }
                    else
                    {
                        xmlWriter.WriteElementString("contents", cells[s].GetContents().ToString());
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.Dispose();

            }
            catch
            {
                throw new SpreadsheetReadWriteException("Spreadsheet Exception");
            }


            changed = false;
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {

            if (name == null || !(IsValid(Normalize(name))) || !IsVar(name))
                throw new InvalidNameException();
            else
            {
                name = Normalize(name);
                if (cells.TryGetValue(name, out Cell c))
                {
                    if (!(c.formula is null))
                    {
                        return c.formula;
                    }
                    else if (!(c.text is null))
                    {
                        return c.text;
                    }
                    else
                    {
                        return c.val;
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// Enumerates  the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return cells.Keys;
        }
        // ADDED FOR PS5
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {

            if (name == null || (!IsValid(name) || !IsVar(name)))
            {
                throw new InvalidNameException();
            }

            else
            {
                name = Normalize(name);
                if (cells.TryGetValue(name, out Cell c))
                {
                    return c.Evaluated;
                }
                else
                {
                    return "";
                }

            }
        }
        // ADDED FOR PS5
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown,
        ///       and no change is made to the spreadsheet.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a list consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            //Normalize Name

            if (content == null)
            {
                throw new ArgumentNullException("Content is null");

            }
            if (name == null || !(IsValid(Normalize(name))) || !IsVar(name))
            {
                throw new InvalidNameException();
            }
            name = Normalize(name);
            IList<string> list;
            if (double.TryParse(content, out double result))
            {
                list = SetCellContents(Normalize(name), result);

            }
            else if (content.Length > 0 && content[0] == '=')
            {
                Formula f = new Formula(content.Trim('='), Normalize, IsValid);
                list = SetCellContents(Normalize(name), f);

            }
            else
            {
                list = SetCellContents(name, content);

            }
            //Will Recalculate Any Cells that have a some form of dependency on name

            Recalculate(list);
            changed = true;
            return list;

        }
        /// <summary>
        /// Method to recalculate any cells that are a formula that were returned in SetContentsOfCell
        /// </summary>
        /// <param name="list">list of cells to be recalculated</param>
        private void Recalculate(IList<string> list)
        {
            foreach (string s in list)
            {
                if (cells.TryGetValue(s, out Cell c))
                {
                    if (c.formula != null)
                    {
                        object o = c.formula.Evaluate(lookup);
                        c.Evaluated = o;
                    }
                }
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected override IList<string> SetCellContents(string name, double number)
        {

            if (cells.TryGetValue(name, out Cell val))
            {
                val.Change(number);
                val.Evaluated = number;
                dependencies.ReplaceDependees(name, new HashSet<string>());
            }
            else
            {
                dependencies.ReplaceDependees(name, new HashSet<string>());
                Cell c = new Cell(number);
                c.Evaluated = number;
                cells.Add(name, c);

            }

            return new List<string>(GetCellsToRecalculate(name));
        }
        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected override IList<string> SetCellContents(string name, string text)
        {


            if (cells.TryGetValue(name, out Cell val))
            {
                if (text == "")
                {
                    cells.Remove(name);
                    val.Evaluated = text;
                    dependencies.ReplaceDependees(name, new HashSet<string>());
                }
                else
                {
                    val.Change(text);
                    val.Evaluated = text;
                    dependencies.ReplaceDependees(name, new HashSet<string>());
                }
            }
            else
            {
                if (text == "")
                {
                    cells.Remove(name);

                    dependencies.ReplaceDependees(name, new HashSet<string>());
                }
                else
                {
                    Cell c = new Cell(text);
                    c.Evaluated = text;
                    dependencies.ReplaceDependees(name, new HashSet<string>());
                    cells.Add(name, c);
                }
            }

            return new List<string>(GetCellsToRecalculate(name));
        }
        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {

            IEnumerable<string> old = dependencies.GetDependees(name);
            if (cells.TryGetValue(name, out Cell val))
            {
                try
                {
                    GetCellsToRecalculate(name);
                    dependencies.ReplaceDependees(name, formula.GetVariables());
                    GetCellsToRecalculate(name);
                    val.Change(formula);
                }
                catch (CircularException e)
                {
                    dependencies.ReplaceDependees(name, old);
                    throw e;
                }
            }
            else
            {
                try
                {
                    GetCellsToRecalculate(name);
                    dependencies.ReplaceDependees(name, formula.GetVariables());
                    GetCellsToRecalculate(name);
                    cells.Add(name, new Cell(formula));
                }
                catch (CircularException e)
                {
                    dependencies.ReplaceDependees(name, old);
                    throw e;
                }
            }

            return new List<string>(GetCellsToRecalculate(name));
        }



        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {

            return dependencies.GetDependents(name);
        }

        /// <summary>
        /// returns the double from an evaluated cell
        /// </summary>
        /// <param name="cell">cell to get value from</param>
        /// <returns>Throws exception if evaluated is not a double, returns the double otherwise</returns>
        private double lookup(string cell)
        {
            if (cells.TryGetValue(Normalize(cell), out Cell c))
            {
                if (c.Evaluated is double)
                {
                    return (double)c.Evaluated;
                }
                else
                {
                    throw new ArgumentException("Evaluated is not double");
                }
            }
            else
            {
                throw new ArgumentException("Cell Doesn't exist");
            }


        }
        private bool IsVar(string token)
        {
            String varPattern = @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            if (Regex.IsMatch(token, varPattern))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Cell Class holds evaluted value and the original contents of the cell broken up into their respective types
        /// <var name="Evaluated">Holds the evaluated double, text or FormulaError</var>
        /// 
        /// <var name="text">Holds Text</var>
        /// 
        /// <var name="val">Holds Value</var>
        /// 
        /// <var name="Formula">Holds a formula</var>
        /// </summary>
        private class Cell
        {
            public object Evaluated;
            public string text;
            public double val;
            public Formula formula;
            public Cell(string contents)
            {
                text = contents;

            }

            public Cell(double contents)
            {
                val = contents;

            }
            public Cell(Formula contents)
            {
                formula = contents;

            }
            /// <summary>
            /// Gets the contents of the cell
            /// </summary>
            /// <returns></returns>
            public object GetContents()
            {
                if (text != null)
                {
                    return text;
                }
                else if (formula != null)
                {
                    return formula;
                }
                else
                {
                    return val;
                }
            }

            /// <summary>
            /// Changes the contents of a cell to the double
            /// </summary>
            /// <param name="d">cell's contents is to be set to d</param>
            public void Change(double d)
            {
                text = null;
                formula = null;
                val = d;
            }
            /// <summary>
            /// Changes the contents of a cell to the Formula
            /// </summary>
            /// <param name="d">cell's contents is to be set to f</param>
            public void Change(Formula f)
            {
                formula = f;
                text = null;
                val = double.NaN;

            }
            /// <summary>
            /// Changes the contents of a cell to the string
            /// </summary>
            /// <param name="d">cell's contents is to be set to s</param>
            public void Change(string s)
            {
                formula = null;
                text = s;
                val = double.NaN;
            }

        }
    }

}