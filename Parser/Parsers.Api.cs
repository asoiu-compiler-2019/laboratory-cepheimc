using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interpreter.Syntax;
using Interpreter.Syntax.Expressions;

namespace Interpreter.Parser
{
    public sealed partial class Parsers
    {
        public Expression ParseExpression(SourceCode sourceCode, IEnumerable<Token> tokens)
        {
            InitializeParser(sourceCode, tokens, Options.OptionalSemicolons);
           
            try
            {
                return ParseExpression();
            }
            catch (SyntaxException)
            {
                // Errors are located in the ErrorSink.
                return null;
            }
        }

        public SourceDoc ParseFile(SourceCode sourceCode, IEnumerable<Token> tokens)
        {
            return ParseFile(sourceCode, tokens, Options.Default);
        }

        public SourceDoc ParseFile(SourceCode sourceCode, IEnumerable<Token> tokens, Options options)
        {
           
            InitializeParser(sourceCode, tokens, options);
           
            try
            {
                return ParseDocument();
            }
            catch (SyntaxException)
            {
                return null;
            }
        }

        public SyntaxNode ParseStatement(SourceCode sourceCode, IEnumerable<Token> tokens)
        {
            return ParseStatement(sourceCode, tokens, Options.Default);
        }

        public SyntaxNode ParseStatement(SourceCode sourceCode, IEnumerable<Token> tokens, Options options)
        {

            InitializeParser(sourceCode, tokens, options);

            try
            {
                return ParseStatement();
            }
            catch (SyntaxException)
            {
                return null;
            }
        }
    }
}
