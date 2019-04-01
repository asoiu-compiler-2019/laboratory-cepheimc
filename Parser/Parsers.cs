using System;
using System.Collections.Generic;
using System.Linq;
using Interpreter.Syntax;
using Interpreter.Syntax.Statements;

namespace Interpreter.Parser
{
    public sealed partial class Parsers
    {
        private bool error;
        private ErrorSink errorSink;
        private int index;
        private Options options;
        private SourceCode sourceCode;
        private IEnumerable<Token> tokens;

        public List<SyntaxNode> statm = new List<SyntaxNode>();

        private Token current => tokens.ElementAtOrDefault(index) ?? tokens.Last();

        private Token last => Peek(-1);

        private Token next => Peek(1);

        public Parsers() : this(new ErrorSink())
        {
        }

        public Parsers(ErrorSink errorSink)
        {
            this.errorSink = errorSink;
        }

        private void AddError(Severity severity, string message, SourceSpan? span = null)
        {
            this.errorSink.AddError(message, sourceCode, severity, span ?? CreateSpan(current));
        }

        private void Advance()
        {
            index++;
        }

        private SourceSpan CreateSpan(SourceLocation start, SourceLocation end)
        {
            return new SourceSpan(start, end, index);
        }

        private SourceSpan CreateSpan(Token start)
        {
            return CreateSpan(start.Span.Start, current.Span.End);
        }

        private SourceSpan CreateSpan(SyntaxNode start)
        {
            return CreateSpan(start.Span.Start, current.Span.End);
        }

        private SourceSpan CreateSpan(SourceLocation start)
        {
            return CreateSpan(start, current.Span.End);
        }

        private void InitializeParser(SourceCode sourceCode, IEnumerable<Token> tokens, Options options)
        {
            this.sourceCode = sourceCode;
            this.tokens = tokens.Where(g => !g.IsTrivia()).ToArray();
            this.options = options;
            this.index = 0;
        }

        private void MakeBlock(Action action, TokenKind openKind = TokenKind.LeftBracket, TokenKind closeKind = TokenKind.RightBracket)
        {
            Take(openKind);

            MakeStatement(action, closeKind);
        }

        private void MakeStatement(Action action, TokenKind closeKind = TokenKind.Semicolon)
        {
            try
            {
                while (current != closeKind && current != TokenKind.EndOfFile)
                {
                    action();
                }
            }
            catch (SyntaxException)
            {
                while (current != closeKind && current != TokenKind.EndOfFile)
                {
                    Take();
                }
            }
            finally
            {
                if (error)
                {
                    if (last == closeKind)
                    {
                        index--;
                    }
                    if (current != closeKind)
                    {
                        while (current != closeKind && current != TokenKind.EndOfFile)
                        {
                            Take();
                        }
                    }
                    error = false;
                }
                if (closeKind == TokenKind.Semicolon)
                {
                    TakeSemicolon();
                }
                else
                {
                    Take(closeKind);
                }
            }
        }

        private Token Peek(int ahead)
        {
            return tokens.ElementAtOrDefault(index + ahead) ?? tokens.Last();
        }

        private SyntaxException SyntaxError(Severity severity, string message, SourceSpan? span = null)
        {
            error = true;
            AddError(severity, message, span);
            return new SyntaxException(message);
        }

        private Token Take()
        {
            var token = current;
            Advance();
            return token;
        }

        private Token Take(TokenKind kind)
        {
            if (current != kind)
            {
                throw UnexpectedToken(kind);
            }
            return Take();
        }

        private Token Take(string contextualKeyword)
        {
            if (current != TokenKind.Identifier && current != contextualKeyword)
            {
                throw UnexpectedToken(contextualKeyword);
            }
            return Take();
        }

        private Token TakeKeyword(string keyword)
        {
            if (current != TokenKind.Keyword && current != keyword)
            {
                throw UnexpectedToken(keyword);
            }
            return Take();
        }

        private Token TakeSemicolon()
        {
            if (options.EnforceSemicolons || current == TokenKind.Semicolon)
            {
                return Take(TokenKind.Semicolon);
            }
            return current;
        }

        private SyntaxException UnexpectedToken(TokenKind expected)
        {
            return UnexpectedToken(expected.ToString());
        }

        private SyntaxException UnexpectedToken(string expected)
        {
            Advance();
            var value = string.IsNullOrEmpty(last?.Value) ? last?.Kind.ToString() : last?.Value;
            string message = $"Unexpected '{value}'.  Expected '{expected}'";

            return SyntaxError(Severity.Error, message, last.Span);
        }
    }
}
