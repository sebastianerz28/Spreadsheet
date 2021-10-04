// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private List<String> normalizedVars;
        public IEnumerable<String> tokens;
        private string stringForm;
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {

        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            tokens = GetTokens(formula);
            normalizedVars = new List<String>();
            if (tokens.Count() == 0)
                throw new FormulaFormatException("Empty formula");
            FirstAndLastValid(tokens.First(), tokens.Last());
            ReadTokens(tokens, normalize, isValid);
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> vals = new Stack<double>();
            Stack<string> operators = new Stack<string>();
            foreach(string s in tokens)
            {
                if(double.TryParse(s, out double i))
                {
                    if (vals.Count == 0)
                        vals.Push(i);
                    else if (i == 0 && operators.Peek().Equals("/"))
                        return new FormulaError("Divide By 0");
                    else if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                    {
                        vals.Push(Calculate(i, vals.Pop(), operators.Pop()));
                    }
                    else
                        vals.Push(i);
                }
                else if (IsVar(s))
                {
                    try
                    {
                        lookup(s);
                    } catch 
                    {
                        return new FormulaError("S could not be found");
                    }
                    if (vals.Count == 0)
                    {
                        vals.Push(lookup(s));
                    }

                    else if (lookup(s) == 0 && operators.Peek().Equals("/"))
                        return new FormulaError("divide by 0");
                    else if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                    {
                        vals.Push(Calculate(lookup(s), vals.Pop(), operators.Pop()));
                    }
                    else
                        vals.Push(lookup(s));
                }
                else if (s.Equals("+") || s.Equals("-"))
                {
                    if (operators.Count == 0)
                        operators.Push(s);
                    else if (operators.Peek().Equals("+") || operators.Peek().Equals("-"))
                    {
                            double x = vals.Pop();
                            double y = vals.Pop();
                            vals.Push(Calculate(x, y, operators.Pop()));
                            operators.Push(s);
                    }
                    else
                        operators.Push(s);
                }
                else if (s.Equals("*") || s.Equals("/"))
                {
                    operators.Push(s);
                }
                else if (s.Equals("("))
                {
                    operators.Push(s);
                }
                else if (s.Equals(")"))
                {
                    if (operators.Peek().Equals("+") || operators.Peek().Equals("-"))
                    {
                            double x = vals.Pop();
                            double y = vals.Pop();
                            vals.Push(Calculate(x, y, operators.Pop()));
                    }
                    if (operators.Count > 0 && operators.Peek().Equals("("))
                    {
                        operators.Pop();
                    }
                    if (operators.Count == 0)
                        continue;
                    if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                    {
                        double x = vals.Pop();
                        double y = vals.Pop();
                        if (x == 0 && operators.Peek().Equals("/"))
                            return new FormulaError("Divide by 0");
                        vals.Push(Calculate(x, y, operators.Pop()));
                    }
                }
                

            }
            if (operators.Count == 0)
                    return vals.Pop();
            else
            {
                    double x = vals.Pop();
                    double y = vals.Pop();
                    return Calculate(x, y, operators.Pop());
            }
        }


        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return normalizedVars;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return stringForm;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            
            if (ReferenceEquals(obj, null) || (obj.GetType() != this.GetType()))
                return false;
            else
                return obj.ToString().Equals(this.stringForm);
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (ReferenceEquals(f1, null) && ReferenceEquals(f2, null))
                return true;
            else if (ReferenceEquals(f1, null) && !ReferenceEquals(f2, null))
                return false;
            else
                return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if(f1 is null && f2 is null)
            {
                return false;
            }
            else if(f1 is null && !(f2 is null))
            {
                return true;
            }
            else if(!(f1 is null) && f2 is null)
            {
                return true;
            }
            else
            {
                return !(f1.Equals(f2));
            }
            
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return stringForm.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
        /// <summary>
        /// Reads the tokesn returned by GetTokens
        /// Will verify formula is syntactically correct will validate variables and normalize them, Adds any variables to a list that can be returned by GetVaribales
        /// Throws Exceptions as needed
        /// </summary>
        /// <param name="tokens">tokens to be itereated through</param>
        /// 
        private IEnumerable<string> ReadTokens(IEnumerable<string> tokens, Func<string, string> normalize, Func<string, bool> isValid)
        {
            
            int leftCount = 0;
            int rightCount = 0;
            string prev = "";
            List<string> normalizedFormula = new List<string>();
            //Uses a hashset to ensure we do not add any duplicate variables
            HashSet<String> vars = new HashSet<string>();
            StringBuilder builder = new StringBuilder();
            foreach(string s in tokens)
            {
                 if (IsNumber(s))
                 {
                    
                    normalizedFormula.Add(double.Parse(s).ToString());

                    builder.Append(double.Parse(s).ToString());
                 }
                else if (IsVar(s))
                {
                    string normalized = normalize(s);
                    if (isValid(normalized))
                    {
                        normalizedFormula.Add(normalized);
                        if (!vars.Contains(normalized))
                        {
                            normalizedVars.Add(normalized);
                            vars.Add(normalized);
                        }
                        builder.Append(normalized);
                    }
                    else
                        throw new FormulaFormatException("Variable: " + s + " is not valid");
                }
                else if (isLP(s))
                {
                    leftCount++;
                    normalizedFormula.Add(s);
                }
                else if (IsRp(s))
                {
                    rightCount++;
                    normalizedFormula.Add(s);
                    builder.Append(s);
                }
                
                else if (IsOperator(s))
                {
                    normalizedFormula.Add(s);
                    builder.Append(s);
                }
                FollowingRules(s, prev);
                prev = s;
                if (rightCount > leftCount)
                    throw new FormulaFormatException("Too many right parenthesis to left parenthesis number of left: " + leftCount + " number of right count: " + rightCount);
            }
            if (leftCount != rightCount)
                throw new FormulaFormatException("Number of left and right parenthesis are not the same");
            stringForm = builder.ToString();
            return normalizedFormula;

        }
        private void FollowingRules(string curr, string prev)
        {
            if (IsRp(prev) || IsNumber(prev) || IsVar(prev))
                if (!IsRp(curr) && !IsOperator(curr))
                    throw new FormulaFormatException("Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.");
                else
                    return;
            else if (isLP(prev) || IsOperator(prev))
                if (!isLP(curr) && !IsNumber(curr) && !IsVar(curr))
                    throw new FormulaFormatException("Any token that immediately follows an opening parenthesis or an operator must be either a number, a variable, or an opening parenthesis.");
            
            
        }

        /// <summary>
        /// Checks if the first and last tokens are allowed being numbers, vars, or left and right parenthesis
        /// </summary>
        /// <param name="first"></param>
        /// <param name="last"></param>
        private void FirstAndLastValid(string first,string last)
        {
            if (!IsNumber(first) && !IsVar(first) && !isLP(first))
                throw new FormulaFormatException("First Token is not valid");
            if (!IsNumber(last) && !IsVar(last) && !IsRp(last))
                throw new FormulaFormatException("Last token not valid");
        }
        /// <summary>
        /// Checks if it is a Variable
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsVar(string token)
        {
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            if (Regex.IsMatch(token, varPattern))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Checks if it is a number
        /// </summary>
        /// <param name="token"></param>
        /// <returns>returns true if it is false otherwise</returns>
        private bool IsNumber(string token)
        {
            double temp = 0;
            if (double.TryParse(token, out temp))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if it is a left parenthesis
        /// </summary>
        /// <param name="token"></param>
        /// <returns>returns true if it is false otherwise</returns>
        private bool isLP(string token)
        {
            if (token == "(")
                return true;
            else
                return false;
        }
        /// <summary>
        /// Checks if it is a left parenthesis
        /// </summary>
        /// <param name="token"></param>
        /// <returns>returns true if it is false otherwise</returns>
        private bool IsRp(string token)
        {
            if (token == ")")
                return true;
            else
                return false;
        }
        /// <summary>
        /// Checks if it is a operator
        /// </summary>
        /// <param name="token"></param>
        /// <returns>returns true if it is false otherwise</returns>
        private bool IsOperator(string token)
        {
            String varPattern = @"[\+\-*/]";
            if (Regex.IsMatch(token, varPattern))
                return true;
            else
                return false;
        }
        private double Calculate(double a, double b, string op)
        {
            if (op.Equals("+"))
                return a + b;
            else if (op.Equals("-"))
                return b - a;
            else if (op.Equals("*"))
                return a * b;
            else
            {
                    return b / a;
            }
        }
    }

  

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }

    
}

