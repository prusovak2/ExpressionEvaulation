using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("ExpressionTests")]


namespace ExpressionEvaluation
{
    /// <summary>
    /// represents one expression
    /// </summary>
    public class Expression
    {
        /// <summary>
        /// null until successful evaluation
        /// </summary>
        public int? Result { get; private set; }

        /// <summary>
        /// root of evaluation tree
        /// null until expression has been read successfully
        /// </summary>
        public Token Root { get; set; }

        /// <summary>
        /// tries to evaluate expression, returns result, sets result property
        /// prints error message to stdout when expression idf somewhat flawed
        /// </summary>
        /// <returns></returns>
        public int? Evaluate()
        {
            int? result = null;
            if(this.Root is null)
            {
                return result;
            }
            //evaluate
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
        /// tries to parse input line to evaluation tree
        /// root property stays null when input is flawed!
        /// </summary>
        /// <param name="input"></param>      
        public bool ReadExpression(string input)
        {
            Queue<string> words = this.SplitInputToQueueOfStrings(input);
            //TODO: some other behaviour when input is empty?
           
            //nonempty input
            if (words.Count > 0)
            {
                string firstWord = words.Dequeue();
                try
                {
                    this.Root = this.TryParseToken(firstWord, words, false);
                }
                catch (FormatException)
                {
                    //expression is flawed, lets dispose its tree, its unfinished and useless
                    if (this.Root != null)
                    {
                        this.Root.Dispose();
                        this.Root = null;                        
                    }
                    Console.WriteLine("Format Error");
                    return false;

                }
                //something remained unprocessed after parsing of expression - some extra number or some mess at the end of expression
                if (words.Count > 0)
                {
                    //expression is flawed, lets dispose its tree, its unfinished and useless
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
            return false; //empty queue - empty input
            
        }

        /// <summary>
        /// same as String.Split('') but returns queue instead of array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal Queue<string> SplitInputToQueueOfStrings(string input)
        {
            Queue<string> Words = new Queue<string>();
            int i = 0;
            while(i<input.Length)
            {
                string word = "";

                //read word
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
        /// tries to parse a part of expression
        /// </summary>
        /// <param name="word"></param>
        /// <param name="Words"></param>
        /// <param name="operandExpected"></param>
        /// /// <exception cref="FormatException"></exception>
        /// <returns></returns>
        internal Token TryParseToken(string word, Queue<string> Words, bool operandExpected)
        {
            switch (word)
            {
                case "+":
                    {
                        Add newToken = new Add();
                        string anotherWord = Expression.DequeueSafely(Words);
                        //create left subtree
                        newToken.FirstArg = this.TryParseToken(anotherWord, Words, false);
                        anotherWord = Expression.DequeueSafely(Words);
                        //create right subtree
                        newToken.SecondArg = this.TryParseToken(anotherWord, Words, false);
                        return newToken;
                    }
                case "-":
                    {
                        Subtract newToken = new Subtract();
                        string anotherWord = Expression.DequeueSafely(Words);
                        //create left subtree
                        newToken.FirstArg = this.TryParseToken(anotherWord, Words, false);
                        anotherWord = Expression.DequeueSafely(Words);
                        //create right subtree
                        newToken.SecondArg = this.TryParseToken(anotherWord, Words, false);
                        return newToken;
                    }
                case "*":
                    {
                        Multiply newToken = new Multiply();
                        string anotherWord = Expression.DequeueSafely(Words);
                        //create left subtree
                        newToken.FirstArg = this.TryParseToken(anotherWord, Words, false);
                        anotherWord = Expression.DequeueSafely(Words);
                        //create right subtree
                        newToken.SecondArg = this.TryParseToken(anotherWord, Words, false);
                        return newToken;
                    }
                case "/":
                    {
                        Divide newToken = new Divide();
                        string anotherWord = Expression.DequeueSafely(Words);
                        //create left subtree
                        newToken.FirstArg = this.TryParseToken(anotherWord, Words, false);
                        anotherWord = Expression.DequeueSafely(Words);
                        //create right subtree
                        newToken.SecondArg = this.TryParseToken(anotherWord, Words, false);
                        return newToken;
                    }
                case "~": //negative number
                    {
                        if (operandExpected)
                        {
                            throw new FormatException("invalid input string");
                        }
                        string anotherWord = Expression.DequeueSafely(Words);
                        //number should follow unary ~
                        if (int.TryParse(anotherWord, out int value)&& value >= 0)
                        {
                            Number newToken = new Number((-1)*value);
                            return newToken;
                        }
                        else
                        {
                            //number should follow unary ~, but it does not
                            throw new FormatException("invalid input string");
                        }
                    }
                default: //number or some random stuff
                    {
                        if (operandExpected)
                        {
                            throw new FormatException("invalid input string");
                        }
                        //number
                        if (int.TryParse(word, out int value) && value>=0)
                        {
                            Number newToken = new Number(value);
                            return newToken;
                        }
                        //some random stuff
                        else
                        {
                            throw new FormatException("invalid input string");
                        }
                    }
                    
            }
        }
        /// <summary>
        /// tries to dequeue member from a queue, throws format exception when queue is empty
        /// is called when queue is expected to be nonempty and empty queue means flawed formula 
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

        /// <summary>
        /// recursive,
        /// returns indented string representation of evaluation tree
        /// </summary>
        /// <returns></returns>
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
