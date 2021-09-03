using System;
using FormulaEvaluator;
using static FormulaEvaluator.Evaluator;

namespace EvalTester
{
    class Program
    {
        static void Main(string[] args)
        {

            //Tests for Basic Addition
            Console.WriteLine("Answer should be 3 and was: " + Evaluator.Evaluate("1+2", l));
            Console.WriteLine("Answer should be 6 and was: " + Evaluator.Evaluate("1+2+3", l));
            Console.WriteLine("Answer should be 10 and was: " + Evaluate("1+2+3+4", l));
            Console.WriteLine("Answer should be 15 and was: " + Evaluate("1+2+3+4+5", l));

            //Tests For subtraction
            Console.WriteLine("Answer should be 0 and was: " + Evaluator.Evaluate("1-1", l));
            Console.WriteLine("Answer should be 1 and was: " + Evaluator.Evaluate("2-1", l));
            Console.WriteLine("Answer should be -1 and was: " + Evaluator.Evaluate("1-2", l));
            Console.WriteLine("Answer should be -35 and was: " + Evaluator.Evaluate("10-9-8-7-6-5-4-3-2-1", l));

            //Tests For Multiplication
            Console.WriteLine("Answer Should be 0 and was: " + Evaluate("0*0",l));
            Console.WriteLine("Answer Should be 2 and was: " + Evaluate("1*2", l));
            Console.WriteLine("Answer Should be 8 and was: " + Evaluate("2*2*2", l));
            Console.WriteLine("Answer Should be 10 and was: " + Evaluate("1*2*5", l));
            //Tests for Division
            Console.WriteLine("Answer Should be 0 and was: " + Evaluate("1/2", l));

            //Test basic operations plus variable
            Console.WriteLine("Answer should be 2 and was: " + Evaluate("1+a1" , l));

            //Test Expression
            Console.WriteLine("Answer Should be 3 and was: " + Evaluate("(1+2)", l));
            Console.WriteLine("Answer Should be 0 and was: " + Evaluate("(1/2)", l));
            Console.WriteLine("Answer Should be 2 and was: " + Evaluate("(1*2)", l));
            Console.WriteLine("Answer Should be -1 and was: " + Evaluate("(1-2)", l));

            Console.WriteLine("Answer Should be 2 and was: " + Evaluate("(1-2) + 3", l));
            Console.WriteLine("Answer should be 3 and was: " + Evaluate("a1 + (1+1)", l));
            Console.WriteLine("answer should be 4 and was " + Evaluate("3 + 3 / b3",  l));
            Console.WriteLine("answer should be 4 and was " + Evaluate("3 + 3 / bb3", l));
            Console.WriteLine("Answer should be 100 and was: " + Evaluate("100/(2/2)", l));
            Console.WriteLine("Answer should be 1 and was: " + Evaluate("(1+2) / 4 * 2", l));

            //Exception Test
            //Console.WriteLine(Evaluate("(1+2) /0", l));
            //Console.WriteLine("Exception should be thrown: " + Evaluate("==", l));
            //Console.WriteLine("ExceptionShould be thrown Expression was" + Evaluate("3x + 3", l));
            Console.WriteLine(Evaluate("2 2", l));
        }
        public static int l(string s)
        {

            if(s == "a1")
            {
                return 1;

            }
            else if (s == "b1")
            {
                return 2;
            }
            else if (s == "b3")
            {
                return 3;
            }
            else if (s == "bb3")
            {
                return 3;
            }
            else
                return 0;
        }
    }
}
