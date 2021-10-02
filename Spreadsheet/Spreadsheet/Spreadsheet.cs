using SpreadsheetUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
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
            if (name == null || !(IsVar(name)))
                throw new InvalidNameException();
            else

            {
                if (cells.TryGetValue(name, out Cell c))
                {

                    if (!(c.formula is null))
                    {
                        return c.formula;
                    }
                    else if (!(c.var is null))
                    {
                        return c.var;
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
            if (name == null || !(IsVar(name)))
                throw new InvalidNameException();
            else
            {
                if (cells.TryGetValue(name, out Cell val))
                {
                    val.Change(number);
                    dependencies.ReplaceDependees(name, new HashSet<string>());
                }
                else
                {
                    dependencies.ReplaceDependees(name, new HashSet<string>());
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
            if (name == null || !(IsVar(name)))
            {
                throw new InvalidNameException();
            }
            else if (text == null)
            {
                throw new ArgumentNullException();
            }
            else
            {

                if (cells.TryGetValue(name, out Cell val))
                {
                    if (text == "")
                    {
                        cells.Remove(name);
                        dependencies.ReplaceDependees(name, new HashSet<string>());
                    }
                    else
                    {
                        val.Change(text);
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
                        dependencies.ReplaceDependees(name, new HashSet<string>());
                        cells.Add(name, new Cell(text));
                    }
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
            if (name == null || !(IsVar(name)))
            {
                throw new InvalidNameException();
            }
            else if (formula == null)
            {
                throw new ArgumentNullException();
            }
            else
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

        private bool IsVar(string token)
        {
            String varPattern = @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            if (Regex.IsMatch(token, varPattern))
                return true;
            else
                return false;
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

            public void Change(double d)
            {
                var = null;
                formula = null;
                val = d;
            }
            public void Change(Formula f)
            {
                formula = f;
                var = null;
                val = double.NaN;

            }
            public void Change(string s)
            {
                formula = null;
                var = s;
                val = double.NaN;
            }

        }
    }
    /// <summary>
    /// 
    /// </summary>

}

