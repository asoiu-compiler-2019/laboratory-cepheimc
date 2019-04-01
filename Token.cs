using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public sealed class Token : IEquatable<Token>
    {
        private Lazy<TokenCatagory> catagory;

        public TokenCatagory Catagory => catagory.Value;

        public TokenKind Kind { get; }

        public SourceSpan Span { get; }

        public string Value { get; }

        public Token(TokenKind kind, string contents, SourceLocation start, SourceLocation end)
        {
            Kind = kind;
            Value = contents;
            Span = new SourceSpan(start, end);

            catagory = new Lazy<TokenCatagory>(GetTokenCatagory);
        }

        public bool IsTrivia()
        {
            return Catagory == TokenCatagory.WhiteSpace;
        }

        private TokenCatagory GetTokenCatagory()
        {
            switch (Kind)
            {
                case TokenKind.Colon:
                case TokenKind.Semicolon:
                case TokenKind.Comma:
                case TokenKind.Dot:
                    return TokenCatagory.Punctuation;

                case TokenKind.Equal:
                case TokenKind.NotEqual:
                case TokenKind.LessThan:
                case TokenKind.LessThanOrEqual:
                case TokenKind.GreaterThan:
                case TokenKind.GreaterThanOrEqual:
                case TokenKind.BooleanOr:
                case TokenKind.BooleanAnd:
                case TokenKind.Assignment:
                    return TokenCatagory.Operator;

                
                case TokenKind.NewLine:
                case TokenKind.WhiteSpace:
                    return TokenCatagory.WhiteSpace;
                    
                case TokenKind.LeftBracket:
                case TokenKind.LeftParenthesis:
                case TokenKind.RightBracket:
                case TokenKind.RightParenthesis:
                    return TokenCatagory.Grouping;

                case TokenKind.Identifier:
                case TokenKind.Keyword:
                    return TokenCatagory.Identifier;

                case TokenKind.StringLiteral:
                case TokenKind.IntegerLiteral:
                case TokenKind.FloatLiteral:
                    return TokenCatagory.Constant;

                case TokenKind.Error:
                    return TokenCatagory.Invalid;

                default: return TokenCatagory.Unknown;
            }
        }

        public static bool operator !=(Token left, string right)
        {
            return left?.Value != right;
        }

        public static bool operator !=(string left, Token right)
        {
            return right?.Value != left;
        }

        public static bool operator !=(Token left, TokenKind right)
        {
            return left?.Kind != right;
        }

        public static bool operator !=(TokenKind left, Token right)
        {
            return right?.Kind != left;
        }

        public static bool operator ==(Token left, string right)
        {
            return left?.Value == right;
        }

        public static bool operator ==(string left, Token right)
        {
            return right?.Value == left;
        }

        public static bool operator ==(Token left, TokenKind right)
        {
            return left?.Kind == right;
        }

        public static bool operator ==(TokenKind left, Token right)
        {
            return right?.Kind == left;
        }

        public override bool Equals(object obj)
        {
            if (obj is Token)
            {
                return Equals((Token)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(Token other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Value == Value &&
                   other.Span == Span &&
                   other.Kind == Kind;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Span.GetHashCode() ^ Kind.GetHashCode();
        }

    }
}
