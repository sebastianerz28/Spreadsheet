using System;
using System.Collections
using System.Text.RegularExpressions
namespace FormulaEvaluator
{
    public static class Evaluator
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Test");
        }
        public delegate int Lookup(String v);
        Stack vals = new Stack();
        Stack operators = new Stack();
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(s, "(\()|(\))|(-)|(\+)|(\*)|(/)");
            for (int i = 0; i < substrings.Length; i++)
            {
                if (Char.IsDigit(substrings[i]))
                {
                    if (vals.Count == 0)
                        throw new ArgumentException("No values to compute");
                    else if (vals.Peek == 0 && int.Parse(substrings[i]) == 0 && operators.Peek == "/")
                        throw new ArithmeticException("divide by 0");
                    else if (operators.Peek.equals("*") || operators.Peek.equals("/"))
                    {
                        vals.Push(Evaluator.Calculate(int.Parse(substrings[i]), vals.Pop(), operators.Pop());
                    }
                    else
                        vals.Push(int.Parse(substring[i]));

                }
                else if (IsVariable(substrings[i]))
                {
                    if (vals.Peek == null)
                        throw new ArgumentException("No values to compute");
                    else if (vals.Peek == 0 && variableEvaluator(substrings[i] == 0 && operators.Peek == "/")
                        throw new ArithmeticException("divide by 0");
                    else if (operators.Peek.equals("*") || operators.Peek.equals("/"))
                    {
                        vals.Push(Evaluator.Calculate(variableEvaluator(substrings[i], vals.Pop(), operators.Pop());
                    }
                    else
                        vals.Push(int.Parse(substring[i]));
                }
                else if (substrings[i] == "+" || substrings[i] == "-")
                {

                    if (operators.Peek == "+" || operators.Peek == "-")
                    {
                        if vals.Count < 2
                            throw new ArgumentException("Not enough values")
                        else
                        {
                            int x = vals.Pop;
                            iny y = vals.Pop;

                            vals.Push(Calculate(x, y, substrings[i]));
                        }

                    }
                    else
                        operators.Push(substrings[i]);




                }
                else if (substrings[i] == "*" || substrings[i] == "/")
                {
                    operators.Push(substrings[i]);
                }
                else if (substrings[i] == "(")
                {
                    operators.Push(substrings[i]);
                }
                else if (substrings[i] == ")")
                {
                    if(operators.Peek == "+" || operators.Peek == "-")
                    {
                        if (vals.Count < 2)
                            throw new ArgumentException("Not enough values");
                        else
                        {
                            int x = vals.Pop;
                            iny y = vals.Pop;
                            vals.Push(Calculate(x, y, operators.Pop);
                        }
                    }
                    if (operators.Peek == "(")
                    {
                        operators.Pop;
                    }
                    else
                        throw new ArgumentException("Expected left parenthesis: ( but was different");
                    if(operators.Peek.equals("*") || operators.Peek.equals("/"))
                    {
                        if(vals.Count < 2)
                            throw new ArgumentException("Not enough values");
                        int x = vals.Pop;
                        iny y = vals.Pop;
                        if (x == 0 && y == 0 && operators.Pop == "/")
                            throw new ArithmeticException(Divide by 0);
                        vals.Push(Evaluator.Calculate(x, y, operators.Pop());
                    }
                }
            }
            if (operators.Count == 0)
                if vals.Count > 1
                    throw new ArgumentException("too many values");
                else
                    return vals.Pop;
            else
            {
                if (operators.Count == 1 && (operators.Peek == "+" || operators.Peek == "-") && vals.Count == 2)
                {
                    int x = vals.Pop;
                    int y = vals.Pop;
                    return Calculate(x, y, operators.Pop);
                }
                else if (operators.Count != 1)
                    throw new ArgumentException("too many operators");
                else if (operators.Peek != "+" || operators.Peek != "-")
                    throw new ArgumentException("wrong type of operators");
                else
                    throw new ArgumentException("too many vals");
            }

        }
        private static int Calculate(int a, int b, String op)
        {
            if (op.Equals("+");
            return a + b;
            else if (op.Equals("-")
                return a - b;
            else if op.Equals("*")
                return a * b;
            else
                return a / b;
        }
        private static bool IsVariable(string s)
        {
            bool isBool = true;
            bool letters = true;
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsLetter(s[i]) && letters == true)
                    continue;
                else if (Char.IsLetter(s[i]) == false && letters == true)
                    letters == false
                else if (letters == false && Char.IsDigit(s[i])
                {
                    continue;
                }
                else
                    return false;

            }
        }
    }

}

