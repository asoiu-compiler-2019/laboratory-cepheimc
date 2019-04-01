using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter.Lexer
{
    public sealed class Lexical
    {
        private static readonly string[] _Keywords = { "break", "case", "switch", "func", "if", "else",
                                                       "var", "null", "true", "false", "photo", "document", "string" };

        private StringBuilder builder;
        private int column;
        private ErrorSink errorSink;
        private int index;
        private int line;
        private SourceCode sourceCode;
        private SourceLocation tokenStart;

        public ErrorSink ErrorSink => errorSink;

        private char ch => sourceCode[index];

        private char last => Peek(-1);

        private char next => Peek(1);

        public Lexical()
            : this(new ErrorSink())
        {
        }

        public Lexical(ErrorSink errorSink)
        {
            this.errorSink = errorSink;
            builder = new StringBuilder();
            sourceCode = null;
        }

        public IEnumerable<Token> LexFile(string sourceCode)
        {
            return LexFile(new SourceCode(sourceCode));
        }

        public IEnumerable<Token> LexFile(SourceCode sourceCode)
        {
            this.sourceCode = sourceCode;
            builder.Clear();
            line = 1;
            index = 0;
            column = 0;
            CreateToken(TokenKind.EndOfFile);

            return LexContents();
        }

        private void AddError(string message, Severity severity)
        {
            var span = new SourceSpan(tokenStart, new SourceLocation(index, line, column));
            errorSink.AddError(message, sourceCode, severity, span);
        }

        private void Advance()
        {
            index++;
            column++;
        }

        private void Consume()
        {
            builder.Append(ch);
            Advance();
        }

        private Token CreateToken(TokenKind kind)
        {
            string contents = builder.ToString();
            SourceLocation end = new SourceLocation(index, line, column);
            SourceLocation start = tokenStart;

            tokenStart = end;
            builder.Clear();

            return new Token(kind, contents, start, end);
        }

        private void DoNewLine()
        {
            line++;
            column = 0;
        }

        private bool IsDigit()
        {
            return char.IsDigit(ch);
        }

        private bool IsEOF()
        {
            return ch == '\0';
        }

        private bool IsIdentifier()
        {
            return IsLetterOrDigit() || ch == '_';
        }

        private bool IsKeyword()
        {
            return _Keywords.Contains(builder.ToString());
        }

        private bool IsLetter()
        {
            return char.IsLetter(ch);
        }

        private bool IsLetterOrDigit()
        {
            return char.IsLetterOrDigit(ch);
        }

        private bool IsNewLine()
        {
            return ch == '\n';
        }

        private bool IsPunctuation()
        {
            return "<>{}()[]!%^&*+-=/.,?;:'|".Contains(ch);
        }

        private bool IsWhiteSpace()
        {
            return (char.IsWhiteSpace(ch) || IsEOF()) && !IsNewLine();
        }

        private IEnumerable<Token> LexContents()
        {
            while (!IsEOF())
            {
                yield return LexToken();
            }

            yield return CreateToken(TokenKind.EndOfFile);
        }

        private Token LexToken()
        {
            if (IsEOF())
            {
                return CreateToken(TokenKind.EndOfFile);
            }
            else if (IsNewLine())
            {
                return ScanNewLine();
            }
            else if (IsWhiteSpace())
            {
                return ScanWhiteSpace();
            }
            else if (IsDigit())
            {
                return ScanInteger();
            }
            else if (IsLetter() || ch == '_')
            {
                return ScanIdentifier();
            }
            else if (ch == '"')
            {
                return ScanStringLiteral();
            }
            else if (ch == '\'')
            {
                return ScanPunctuation();
            }
            else if (ch == '.' && char.IsDigit(next))
            {
                return ScanFloat();
            }
            else if (IsPunctuation())
            {
                return ScanPunctuation();
            }
            else
            {
                return ScanWord();
            }
        }

        private char Peek(int ahead)
        {
            return sourceCode[index + ahead];
        }
        
        private Token ScanFloat()
        {
            if (ch == 'f')
            {
                Advance();
                if ((!IsWhiteSpace() && !IsPunctuation() && !IsEOF()) || ch == '.')
                {
                    return ScanWord(message: "Remove 'f' in floating point number.");
                }
                return CreateToken(TokenKind.FloatLiteral);
            }
            int preDotLength = index - tokenStart.Index;

            if (ch == '.')
            {
                Consume();
            }
            while (IsDigit())
            {
                Consume();
            }

            if (last == '.')
            {
                // .e10 is invalid.
                return ScanWord(message: "Must contain digits after '.'");
            }

            if (ch == 'e')
            {
                Consume();
                if (preDotLength > 1)
                {
                    return ScanWord(message: "Coefficient must be less than 10.");
                }

                if (ch == '+' || ch == '-')
                {
                    Consume();
                }
                while (IsDigit())
                {
                    Consume();
                }
            }

            if (ch == 'f')
            {
                Consume();
            }

            if (!IsWhiteSpace() && !IsPunctuation() && !IsEOF())
            {
                if (IsLetter())
                {
                    return ScanWord(message: "'{0}' is an invalid float value");
                }
                return ScanWord();
            }

            return CreateToken(TokenKind.FloatLiteral);
        }

        private Token ScanIdentifier()
        {
            while (IsIdentifier())
            {
                Consume();
            }

            if (!IsWhiteSpace() && !IsPunctuation() && !IsEOF())
            {
                return ScanWord();
            }

            if (IsKeyword())
            {
                return CreateToken(TokenKind.Keyword);
            }

            return CreateToken(TokenKind.Identifier);
        }

        private Token ScanInteger()
        {
            while (IsDigit())
            {
                Consume();
            }

            if (ch == 'f' || ch == '.' || ch == 'e')
            {
                return ScanFloat();
            }

            if (!IsWhiteSpace() && !IsPunctuation() && !IsEOF())
            {
                return ScanWord();
            }

            return CreateToken(TokenKind.IntegerLiteral);
        }

        private Token ScanNewLine()
        {
            Consume();

            DoNewLine();

            return CreateToken(TokenKind.NewLine);
        }

        private Token ScanPunctuation()
        {
            switch (ch)
            {
                case ';':
                    Consume();
                    return CreateToken(TokenKind.Semicolon);
                case '\'':
                    Consume();
                    return CreateToken(TokenKind.Error);

                case ':':
                    Consume();
                    return CreateToken(TokenKind.Colon);

                case '{':
                    Consume();
                    return CreateToken(TokenKind.LeftBracket);

                case '}':
                    Consume();
                    return CreateToken(TokenKind.RightBracket);
                    
                case '(':
                    Consume();
                    return CreateToken(TokenKind.LeftParenthesis);

                case ')':
                    Consume();
                    return CreateToken(TokenKind.RightParenthesis);

                case '[':
                    Consume();
                    return CreateToken(TokenKind.Error);
                case ']':
                    Consume();
                    return CreateToken(TokenKind.Error);

                case '>':
                    Consume();
                    if (ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.GreaterThanOrEqual);
                    }
                    else if (ch == '>')
                    {
                        Consume();
                        return CreateToken(TokenKind.Error);
                    }

                    return CreateToken(TokenKind.GreaterThan);

                case '<':
                    Consume();
                    if (ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.LessThanOrEqual);
                    }
                    else if (ch == '<')
                    {
                        Consume();
                        return CreateToken(TokenKind.Error);
                    }

                    return CreateToken(TokenKind.LessThan);

                case '+':
                    Consume();
                    if (ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.Error);
                    }
                    else if (ch == '+')
                    {
                        Consume();
                        return CreateToken(TokenKind.Error);
                    }
                    return CreateToken(TokenKind.Error);

                case '-':
                    Consume();
                    if (ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.Error);
                    }
                    else if (ch == '-')
                    {
                        Consume();
                        return CreateToken(TokenKind.Error);
                    }
                    return CreateToken(TokenKind.Error);

                case '=':
                    Consume();
                    if (ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.Equal);
                    }
                    
                    return CreateToken(TokenKind.Assignment);

                case '!':
                    Consume();
                    if (ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.NotEqual);
                    }

                    return CreateToken(TokenKind.Error);

                case '.':
                    Consume();
                    return CreateToken(TokenKind.Dot);

                case ',':
                    Consume();
                    return CreateToken(TokenKind.Comma);

                case '&':
                    Consume();
                    if (ch == '&')
                    {
                        Consume();
                        return CreateToken(TokenKind.BooleanAnd);
                    }

                    return CreateToken(TokenKind.Error);

                case '|':
                    Consume();
                    if (ch == '|')
                    {
                        Consume();
                        return CreateToken(TokenKind.BooleanOr);
                    }
                    
                    return CreateToken(TokenKind.Error);

               default: return ScanWord();
            }
        }

        private Token ScanStringLiteral()
        {
            Advance();

            while (ch != '"')
            {
                if (IsEOF())
                {
                    AddError("Unexpected End Of File", Severity.Fatal);
                    return CreateToken(TokenKind.Error);
                }
                Consume();
            }

            Advance();

            return CreateToken(TokenKind.StringLiteral);
        }

        private Token ScanWhiteSpace()
        {
            while (IsWhiteSpace())
            {
                Consume();
            }
            return CreateToken(TokenKind.WhiteSpace);
        }

        private Token ScanWord(Severity severity = Severity.Error, string message = "Unexpected Token '{0}'")
        {
            while (!IsWhiteSpace() && !IsEOF() && !IsPunctuation())
            {
                Consume();
            }
            AddError(string.Format(message, builder.ToString()), severity);
            return CreateToken(TokenKind.Error);
        }
    }
}
