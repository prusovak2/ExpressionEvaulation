using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("ExpressionTests")]

namespace ExpressionEvaulation
{
    public class Expression
    {
        public int? Result { get; private set; }
        public Token Root { get; set; }

        public int? Evaluate()
        {
            int? result = null;
            if(this.Root is null)
            {
                return result;
            }
            try { result = this.Root.Evaluate(); }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Divide Error");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow Error");
            }
            this.Result = result;
            return result;
        }

        /// <summary>
        /// root stays null when input is flawed!
        /// </summary>
        /// <param name="input"></param>      
        public bool ReadExpression(string input)
        {
            Queue<string> Words = this.SplitInputToQueueOfStrings(input);
            //TODO: some other behaviour when input is empty?
            if (Words.Count > 0)
            {
                string firstWord = Words.Dequeue();
                try { this.Root = this.TryParseToken(firstWord, Words, true); }
                catch (FormatException)
                {
                    if (this.Root != null)
                    {
                        this.Root.Dispose();
                        this.Root = null;                        
                    }
                    Console.WriteLine("Format Error");
                    return false;

                }
                if (Words.Count > 0)
                {
                    if (this.Root != null)
                    {
                        Root.Dispose();
                        this.Root = null;                       
                    }
                    Console.WriteLine("Format Error");
                    return false;

                }
                return true;

            }
           // Console.WriteLine("Format Error");
            return false; //empty queue
            
        }

        internal Queue<string> SplitInputToQueueOfStrings(string input)
        {
            Queue<string> Words = new Queue<string>();
            int i = 0;
            while(i<input.Length)
            {
                string word = "";
                while (i < input.Length && input[i]!= ' ') 
                {
                    word += input[i];
                    i++;
                }
                Words.Enqueue(word);
                i++;
            }
            return Words;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="Words"></param>
        /// /// <exception cref="FormatException"></exception>
        /// <returns></returns>
        internal Token TryParseToken(string word, Queue<string> Words, bool operantExpected)
        {
            switch (word)
            {
                case "+":
                    {
                        Add newToken = new Add();
                        string anotherWord = Expression.DequeueSafely(Words);
                        newToken.FirstArg = this.TryParseToken(anotherWord, Words, false);
                        anotherWord = Expression.DequeueSafely(Words);
                        newToken.SecondArg = this.TryParseToken(anotherWord, Words, false);
                        return newToken;
                    }
                case "-":
                    {
                        Subtract newToken = new Subtract();
                        string anotherWord = Expression.DequeueSafely(Words);
                        newToken.FirstArg = this.TryParseToken(anotherWord, Words, false);
                        anotherWord = Expression.DequeueSafely(Words);
                        newToken.SecondArg = this.TryParseToken(anotherWord, Words, false);
                        return newToken;
                    }
                case "*":
                    {
                        Multiply newToken = new Multiply();
                        string anotherWord = Expression.DequeueSafely(Words);
                        newToken.FirstArg = this.TryParseToken(anotherWord, Words, false);
                        anotherWord = Expression.DequeueSafely(Words);
                        newToken.SecondArg = this.TryParseToken(anotherWord, Words, false);
                        return newToken;
                    }
                case "/":
                    {
                        Divide newToken = new Divide();
                        string anotherWord = Expression.DequeueSafely(Words);
                        newToken.FirstArg = this.TryParseToken(anotherWord, Words, false);
                        anotherWord = Expression.DequeueSafely(Words);
                        newToken.SecondArg = this.TryParseToken(anotherWord, Words, false);
                        return newToken;
                    }
                case "~":
                    {
                        if (operantExpected)
                        {
                            throw new FormatException("invalid input string");
                        }
                        string anotherWord = Expression.DequeueSafely(Words);
                        if (int.TryParse(anotherWord, out int value)&& value >= 0)
                        {
                            Number newToken = new Number((-1)*value);
                            return newToken;
                        }
                        else
                        {
                            throw new FormatException("invalid input string");
                        }
                    }
                default:
                    {
                        if (operantExpected)
                        {
                            throw new FormatException("invalid input string");
                        }
                        if (int.TryParse(word, out int value) && value>=0)
                        {
                            Number newToken = new Number(value);
                            return newToken;
                        }
                        else
                        {
                            throw new FormatException("invalid input string");
                        }
                    }
                    
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue"></param>
        /// <exception cref="FormatException"></exception>
        /// <returns></returns>
        private static string DequeueSafely(Queue<string> queue)
        {
            if (queue.Count <= 0)
            {
                throw new FormatException("Something missing input");
            }
            return queue.Dequeue();
        }

        public string ExpressionTreeToString()
        {
            if(this.Root is null)
            {
                return "Expression is empty";
            }
            StringBuilder builder = new StringBuilder();
            int tabCounter = 0;
            ExpressionTreeToString(this.Root, builder, tabCounter);
            return builder.ToString();
        }
        private void ExpressionTreeToString(Token Root, StringBuilder builder, int tabCounter)
        {
            if(Root is Number)
            {
                for (int i = 0; i < tabCounter; i++)
                {
                    builder.Append("\t");
                }
                builder.Append(Root.ToString());
                builder.Append("\n");
                return;
            }
            Operator op = (Operator)Root;

            tabCounter++;
            ExpressionTreeToString(op.SecondArg, builder, tabCounter);
            tabCounter--;
            
            for (int i = 0; i < tabCounter; i++)
            {
                builder.Append("\t");
            }
            builder.Append(op.ToString());
            builder.Append("\n");

            tabCounter++;
            ExpressionTreeToString(op.FirstArg , builder, tabCounter);
            tabCounter--;

        }

    }
}
