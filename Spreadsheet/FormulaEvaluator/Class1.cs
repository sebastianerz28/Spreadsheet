using System;
using System.Collections;
using System.Text.RegularExpressions;
namespace FormulaEvaluator
{
    public static class Evaluator
    {
        public delegate int Lookup(String v);

        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            Stack vals = new Stack();
            Stack operators = new Stack();
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            int temp = 0;
            for (int i = 0; i < substrings.Length; i++)
            {
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
                else if (substrings[i].Equals("*") || substrings[i].Equals("/"))
                {
                    operators.Push(substrings[i]);
                }
                else if (substrings[i].Equals("("))
                {
                    operators.Push(substrings[i]);
                }
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
                    throw new ArgumentException("too many values");
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
                return b / a;
        }
        private static bool IsVar(string s)
        {
            if (s.Length == 0 || s.Equals(" "))
                return false;
            else if (s.StartsWith(" "))
                s = s.Remove(s[0]);
            else if (s.EndsWith(" "))
                s = s.Remove(s.Length-1);
             
            
            int i = 0;
            
            while (Char.IsLetter(s[i]))
                i++;
            while (Char.IsDigit(s[i]))
            {
                i++;
                if (i == s.Length)
                    break;
            }

            return i == s.Length;
        }
        private static string trim(string s)
        {
            if (s.StartsWith(" "))
                s = s.Remove(s[0]);
            if (s.EndsWith(" "))
                s = s.Remove(s.Length - 1);
            return s;
        }
    }

}

