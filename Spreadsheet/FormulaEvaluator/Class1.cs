using System;
using System.Collections
using System.Text.RegularExpressions
namespace FormulaEvaluator
{
    public static class Evaluator
    {
        public delegate int Lookup(String v);
        Stack vals = new Stack();
        Stack operators = new Stack();
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(s, "(\()|(\))|(-)|(\+)|(\*)|(/)");
            for(int i = 0; i < substrings.Length; i++)
            {
                if(Char.IsDigit(substrings[i]))
                    if (operators.Peek.equals("*") || operators.Peek.equals("/"))
                    {
                        
                    }
            }
        }
        private int Calculate(int a, int b, String op)
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
      
    }

   
 
}
