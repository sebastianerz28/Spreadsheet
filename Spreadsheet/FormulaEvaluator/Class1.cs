using System;
using System.Collections;
using System.Text.RegularExpressions;
namespace FormulaEvaluator
{
    /// <summary>
    /// Evaluator class that contains the lookp delegate the primary evaluate function and various private helper functions to assist in the evaluation
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Delegate that allows a string to be passed inorder to look up the variable
        /// </summary>
        /// <param name="v">name of the string to be looked for</param>
        /// <returns> returns the value associated with the variable</returns>
        public delegate int Lookup(String v);

        /// <summary>
        /// Static evaluate funtion that takes in an expressions and evalutes it as an infix expression
        /// </summary>
        /// <param name="exp"> the expression to be evaluated </param>
        /// <param name="variableEvaluator"> the look up function used to lookup any variables that may be in the expression </param>
        /// <returns> returns the value that the expression simplifies to </returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            //Set up for the expression evaluator
            Stack vals = new Stack();
            Stack operators = new Stack();
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            int temp = 0;
            for (int i = 0; i < substrings.Length; i++)
            {
                //Conditional if the substring is an int
                if (int.TryParse(substrings[i], out temp))
                {
                    if (vals.Count == 0)
                        vals.Push(int.Parse(substrings[i]));
                    else if (vals.Peek().Equals(0) && int.Parse(substrings[i]) == 0 && operators.Peek().Equals("/"))
                        throw new ArithmeticException("divide by 0");
                    else if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                    {
                        if (vals.Count == 0)
                            throw new ArgumentException("No values to compute");
                        vals.Push(Evaluator.Calculate(int.Parse(substrings[i]), (int)vals.Pop(), (string)operators.Pop()));
                    }
                    else
                        vals.Push(int.Parse(substrings[i]));

                }
                //Conditional if the substring is a variable
                else if (IsVar(substrings[i]))
                {
                    substrings[i] = trim(substrings[i]);
                    if (vals.Count == 0)
                    {
                        vals.Push(variableEvaluator(substrings[i]));
                    }
                        
                    else if (vals.Peek().Equals(0) && variableEvaluator(substrings[i]) == 0 && operators.Peek().Equals("/"))
                        throw new ArithmeticException("divide by 0");
                    else if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                    {
                        if (vals.Peek() == null)
                            throw new ArgumentException("No values to compute");
                        vals.Push(Evaluator.Calculate(variableEvaluator(substrings[i]), (int) vals.Pop(), (string)operators.Pop()));
                    }
                    else
                        vals.Push(variableEvaluator(substrings[i]));
                }
                //Conditional if the substring is a + or - operator
                else if (substrings[i].Equals("+") || substrings[i].Equals("-"))
                {
                    if(operators.Count == 0)
                        operators.Push(substrings[i]);
                    else if (operators.Peek().Equals("+") || operators.Peek().Equals("-"))
                    {
                        if (vals.Count < 2)
                            throw new ArgumentException("Not enough values");
                        else
                        {
                            int x = (int) vals.Pop();
                            int y = (int)vals.Pop();
                            vals.Push(Calculate(x, y, substrings[i]));
                        }
                    }
                    else
                        operators.Push(substrings[i]);

                }
                // Conditional if the substring is a * or a / operator
                else if (substrings[i].Equals("*") || substrings[i].Equals("/"))
                {
                    operators.Push(substrings[i]);
                }
                //Conditional if the substring is a left parenthesis
                else if (substrings[i].Equals("("))
                {
                    operators.Push(substrings[i]);
                }
                //Conditional if the substring is a right parenthesis
                else if (substrings[i].Equals(")"))
                {
                    if (operators.Peek().Equals("+") || operators.Peek().Equals("-"))
                    {
                        if (vals.Count < 2)
                            throw new ArgumentException("Not enough values");
                        else
                        {
                            int x = (int)vals.Pop();
                            int y = (int)vals.Pop();
                            vals.Push(Calculate(x, y, (string)operators.Pop()));
                        }
                    }
                    if (operators.Peek().Equals("("))
                    {
                        operators.Pop();
                    }
                    else
                        throw new ArgumentException("Expected left parenthesis: ( but was different");
                    if (operators.Count == 0)
                        continue;
                    if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                    {
                        if (vals.Count < 2)
                            throw new ArgumentException("Not enough values");
                        int x = (int)vals.Pop();
                        int y = (int)vals.Pop();
                        if (x == 0 && y == 0 && operators.Pop().Equals("/"))
                            throw new ArithmeticException("Divide by 0");
                        vals.Push(Evaluator.Calculate(x, y, (string)operators.Pop()));
                    }
                }
            }
            if (operators.Count == 0)
                if (vals.Count != 1)
                    throw new ArgumentException("too many values or too little values");
                else
                    return (int)vals.Pop();
            else
            {
                if (operators.Count == 1 && (operators.Peek().Equals("+") || operators.Peek().Equals("-")) && vals.Count == 2)
                {
                    int x = (int)vals.Pop();
                    int y = (int)vals.Pop();
                    return Calculate(x, y, (string)operators.Pop());
                }
                else if (operators.Count != 1)
                    throw new ArgumentException("too many operators");
                else if (operators.Peek().Equals("+") || operators.Peek().Equals("-"))
                    throw new ArgumentException("wrong type of operators");
                else
                    throw new ArgumentException("too many vals");
            }

        }
        private static int Calculate(int a, int b, String op)
        {
            if (op.Equals("+"))
                return a + b;
            else if (op.Equals("-"))
                return b - a;
            else if (op.Equals("*"))
                return a * b;
            else
            {
                if (a == 0)
                    throw new DivideByZeroException("Trying to divide by 0");
                else
                    return b / a;
            }
                
        }
        /// <summary>
        /// Private helper function to check if the string is a variable, trims before scanning
        /// Will throw an exception if the variable begins in a number
        /// </summary>
        /// <param name="s">string to be checked</param>
        /// <returns> returns true if </returns>
        private static bool IsVar(string s)
        {
            if (s.Equals("") || s.Equals(" "))
                return false;
            else if (s.StartsWith(" "))
                s = s.Remove(0,1);
            else if (s.EndsWith(" "))
                s = s.Remove(s.Length-1);

            if (Char.IsDigit(s[0]))
            {
                throw new ArgumentException("Variable is formattted wrong");
            }
            int i = 0;
            while (Char.IsLetter(s[i]))
                i++;
            while (Char.IsDigit(s[i]))
            {
                i++;
                if (i == s.Length)
                    break;
            }
            if (i > 0 && Char.IsLetter(s[i - 1]) )
                throw new ArgumentException("Letters cannot be bythemselves or after a digit");
            return i == s.Length;
        }
        /// <summary>
        /// Private helper function to trim whitespace off of a variable
        /// </summary>
        /// <param name="s">variable string to be trimmed</param>
        /// <returns> returns a trimmed string with no white space</returns>
        private static string trim(string s)
        {
            if (s.StartsWith(" "))
                s = s.Remove(0,1);
            if (s.EndsWith(" "))
                s = s.Remove(s.Length - 1);
            return s;
        }
    }

}

