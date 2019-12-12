using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaulation
{
    public abstract class Token :IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DivideByZeroException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <returns></returns>
        public abstract int Evaluate();
        public abstract void Dispose();
    }

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
    }

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
    public class Add : Operator
    {
        public override Token FirstArg { get; set; }
        public override Token SecondArg { get; set; }

        public override string ToString()
        {
            return "+";
        }
        public override int Evaluate()
        {
            int result =checked(this.FirstArg.Evaluate() + this.SecondArg.Evaluate());
            return result;
        }
      
    }
    public class Subtract : Operator
    {
        public override Token FirstArg { get; set; }
        public override Token SecondArg { get; set; }

        public override string ToString()
        {
            return "-";
        }
        public override int Evaluate()
        {
            int result = checked(this.FirstArg.Evaluate() - this.SecondArg.Evaluate());
            return result;
        }
    }
    public class Divide : Operator
    {
        public override Token FirstArg { get; set; }
        public override Token SecondArg { get; set; }
        public override string ToString()
        {
            return "/";
        }
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
    public class Multiply : Operator
    {
        public override Token FirstArg { get; set; }
        public override Token SecondArg { get; set; }

        public override string ToString()
        {
            return "*";
        }
        public override int Evaluate()
        {
            int result = checked(this.FirstArg.Evaluate() * this.SecondArg.Evaluate());
            return result;
        }
    }
}
