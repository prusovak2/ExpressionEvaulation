using System;

namespace ExpressionEvaluation
{
    //some unit tests available on https://github.com/prusovak2/ExpressionEvaulation
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            Expression expression = new Expression();
            //try to parse input to an evaluation tree
            bool validExpression = expression.ReadExpression(input);
            int? result= expression.Evaluate();
            if (result != null)
            {
                Console.WriteLine(expression.Result);
            }
        }
    }
}
