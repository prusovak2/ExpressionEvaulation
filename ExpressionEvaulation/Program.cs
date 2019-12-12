using System;

namespace ExpressionEvaulation
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            Expression expression = new Expression();
            bool validExpression = expression.ReadExpression(input);
            int? result= expression.Evaluate();
            if (result != null)
            {
                Console.WriteLine(expression.Result);
            }
        }
    }
}
