using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interpreter.Lexer;
using Interpreter.Parser;
using Interpreter.Syntax;
using Interpreter.Semantic;

namespace Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexical lexical = new Lexical();
            Parsers parser = new Parsers();
           
            string code = "func main()\n" +
                          "{\n" +
                          "photo a;\n" +
                          "photo b;\n" +
                          "photo c;\n" +
                          "a = \"test1.png\";\n" +
                          "b = \"test2.png\";\n" +
                          "c = \"test1.png\";\n" +
                          "switch(c)\n" +
                          "{\n" +
                          "case a: print(a);\n" +
                          "case b: print(b);\n" +
                          "}\n" +
                          "}\n" +
                          "main();\n";

            Console.WriteLine(code);
            int count = 0;
            var sourceCode = new SourceCode(code);
            var tokens = lexical.LexFile(sourceCode).ToArray();
            Console.WriteLine($"\nLexer\n");
            foreach (var token in tokens)
            {
                if (token.Kind == TokenKind.Error)
                {
                    count++;
                }
              //  Console.WriteLine($"line {token.Span.Start.Line} {token.Kind} ( \"{token.Value}\" ) "); //column {token.Span.Start.Column}-{token.Span.End.Column}
            }
            

            if (lexical.ErrorSink.Count() > 0 || count > 0)
            {
                Console.WriteLine($"Error");

                foreach (var error in lexical.ErrorSink)
                {
                    Console.WriteLine(new string('-', Console.WindowWidth / 3));

                    WriteError(error);
                }
                Console.WriteLine(new string('-', Console.WindowWidth / 2));
                lexical.ErrorSink.Clear();
            }
            else
            {
                 Console.WriteLine($"\nSyntax\n");
                var ast = parser.ParseFile(sourceCode, tokens);

                Console.WriteLine($"\nSemantic\n");
                Semantic.Semantic semantic = new Semantic.Semantic(parser);
               // semantic.PrintStatement();
                semantic.AnalyzeFile();
                

                if (lexical.ErrorSink.Count() > 0)
                {
                    foreach (var error in lexical.ErrorSink)
                    {
                        Console.WriteLine(new string('-', Console.WindowWidth / 3));

                        WriteError(error);
                    }
                    Console.WriteLine(new string('-', Console.WindowWidth / 2));
                    lexical.ErrorSink.Clear();
                }
                else if (semantic.errors.Count > 0)
                {
                    var err = semantic.errors.Distinct();
                    foreach (var e in err)
                    {
                        Console.WriteLine($"{e}");
                    }
                    Console.WriteLine(new string('-', Console.WindowWidth / 2));
                }
                else
                {
                    
                }
            }
            Console.ReadKey();
        }

        private static void WriteError(ErrorEntry error)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (error.Lines.Length > 1)
            {
                Console.WriteLine(error.Lines.First());
                Console.CursorLeft = error.Span.Start.Column;
                Console.WriteLine(new string('^', error.Lines[0].Length - error.Span.Start.Column));
                for (int i = 1; i < error.Lines.Length - 1; i++)
                {
                    Console.WriteLine(error.Lines[i]);
                    Console.WriteLine(new string('^', error.Lines[i].Length));
                }
                Console.WriteLine(error.Lines.Last());
                Console.WriteLine(new string('^', error.Lines.Last().Length - error.Span.End.Column));
            }
            else
            {
                Console.WriteLine(error.Lines.First());
                Console.CursorLeft = error.Span.Start.Column;
                Console.WriteLine(new string('^', error.Span.Length));
                Console.WriteLine($"{error.Severity} {error.Span}: {error.Message}");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

}
