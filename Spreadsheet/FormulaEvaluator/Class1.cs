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
                    if (operators.Peek.equals("*") || operators.Peek.equals("/"))
                    {
                        vals.Push(Evaluator.Calculate(int.Parse(substrings[i]), vals.Pop(), operators.Pop());
                    }
                    else
                        vals.Push(int.Parse(substring[i]));

                }


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
            bool letters = true;
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsLetter(s[i]) && letters == true)
                    continue;

            }
        }


    }



}