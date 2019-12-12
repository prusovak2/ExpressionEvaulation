using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluation
{
    /// <summary>
    /// abstract class representing a node of evaluation tree
    /// </summary>
    public abstract class Token :IDisposable
    {
        /// <summary>
        /// returns value of token, recursive
        /// </summary>
        /// <exception cref="DivideByZeroException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <returns></returns>
        public abstract int Evaluate();
        public abstract void Dispose();
    }
    /// <summary>
    /// represents numeric value in evaluation tree
    /// </summary>
    public class Number : Token
    {
        int Value { get; set; }

        public Number(int val)
        {
            this.Value = val;
        }
        
        public override int Evaluate()
        {
            return this.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
        public override void Dispose()
        {
            this.Value = 0;
        }
    }/// <summary>
    /// abstract class uniting binary operators
    /// </summary>
    public abstract class Operator : Token
    {
        public abstract Token FirstArg { get; set; }
        public abstract Token SecondArg { get; set; }

        public override void Dispose()
        {
            if (this.FirstArg != null)
            {
                this.FirstArg.Dispose();
            }
            if (this.SecondArg != null)
            {
                this.SecondArg.Dispose();
            }
            this.FirstArg = null;
            this.SecondArg = null;
        }

    }
    /// <summary>
    /// represents addition
    /// </summary>
    public class Add : Operator
    {
        public override Token FirstArg { get; set; }
        public override Token SecondArg { get; set; }

        public override string ToString()
        {
            return "+";
        }
        /// <summary>
        /// returns sum of arguments
        /// </summary>
        /// <returns></returns>
        public override int Evaluate()
        {
            int result =checked(this.FirstArg.Evaluate() + this.SecondArg.Evaluate());
            return result;
        }
      
    }
    /// <summary>
    /// represents subtraction
    /// </summary>
    public class Subtract : Operator
    {
        public override Token FirstArg { get; set; }
        public override Token SecondArg { get; set; }

        public override string ToString()
        {
            return "-";
        }
        /// <summary>
        /// returns difference of arguments
        /// </summary>
        /// <returns></returns>
        public override int Evaluate()
        {
            int result = checked(this.FirstArg.Evaluate() - this.SecondArg.Evaluate());
            return result;
        }
    }
    /// <summary>
    /// represent division
    /// </summary>
    public class Divide : Operator
    {
        public override Token FirstArg { get; set; }
        public override Token SecondArg { get; set; }
        public override string ToString()
        {
            return "/";
        }
        /// <summary>
        /// returns quotient of arguments
        /// </summary>
        /// <exception cref="DivideByZeroException"></exception>
        /// <returns></returns>
        public override int Evaluate()
        {
            int divisor = SecondArg.Evaluate();
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }
            int result = checked(this.FirstArg.Evaluate() / divisor);
            return result;

        }
    }
    /// <summary>
    /// represents multiplication
    /// </summary>
    public class Multiply : Operator
    {
        public override Token FirstArg { get; set; }
        public override Token SecondArg { get; set; }

        public override string ToString()
        {
            return "*";
        }
        /// <summary>
        /// returns a product of arguments
        /// </summary>
        /// <returns></returns>
        public override int Evaluate()
        {
            int result = checked(this.FirstArg.Evaluate() * this.SecondArg.Evaluate());
            return result;
        }
    }
}
