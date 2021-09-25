using SpreadsheetUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace  SS
{
    class Spreadsheet : AbstractSpreadsheet
    {
        private DependencyGraph dependencies;
        private Dictionary<string, Cell> cells;
        public Spreadsheet()
        {
            dependencies = new DependencyGraph();
            cells = new Dictionary<string, Cell>();
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (name == null || IsInvalid(name))
                throw new InvalidNameException();   
            else
                
            {
                if (cells.TryGetValue(name, out Cell c))
                {
                    if(c.formula != null)
                    {
                        return c.formula;
                    }
                    else if(c.var != null)
                    {
                        return c.var;
                    }
                    else
                    {
                        return c.val;
                    }
                }
                else
                    throw new InvalidNameException();
            }
                
        }

        /// <summary>
        /// Enumerates  the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return cells.Keys;
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
        public override IList<string> SetCellContents(string name, double number)
        {
            if (name == null || IsInvalid(name))
                throw new InvalidNameException();
            else
            {
                if(cells.TryGetValue(name, out Cell val))
                {
                    val = new Cell(number);
                }
                else
                {
                    cells.Add(name, new Cell(number));
                }
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
        public override IList<string> SetCellContents(string name, string text)
        {
            if (name == null || IsInvalid(name))
            {
                throw new InvalidNameException();
            }
            if(text == null)
            {
                throw new ArgumentNullException();
            }
            else
            {

                if (cells.TryGetValue(name, out Cell val))
                {
                    val = new Cell(text);
                }
                else
                {
                    dependencies.AddDependency(name, null);
                    cells.Add(name, new Cell(text));
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
        public override IList<string> SetCellContents(string name, Formula formula)
        {
            if (name == null || IsInvalid(name))
                throw new InvalidNameException();
            if (formula == null)
                throw new ArgumentNullException();
           
            {
                if (cells.TryGetValue(name, out Cell val))
                {
                    GetCellsToRecalculate(name);
                    val = new Cell(formula);
                    dependencies.ReplaceDependents(name, new HashSet<string>());
                }
                else
                {
                    GetCellsToRecalculate(name);
                    cells.Add(name, new Cell(formula));
                    dependencies.ReplaceDependents(name, new HashSet<string>(GetCellsToRecalculate(name)));
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

        private bool IsInvalid(string name)
        {
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            if (Regex.IsMatch(name, varPattern))
                return false;
            else
                return true;
        }

        private class Cell
        {
            public string var;
            public double val;
            public Formula formula;
            public Cell(string contents)
            {
                var = contents;

            }
            public Cell(double contents)
            {
                val = contents;

            }
            public Cell(Formula contents)
            {
                formula = contents;

            }
            
        }
    }
    /// <summary>
    /// 
    /// </summary>
    
}

