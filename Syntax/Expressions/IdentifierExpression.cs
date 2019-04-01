
using System;

namespace Interpreter.Syntax.Expressions
{
    public class IdentifierExpression : Expression
    {
        public string Identifier { get; }

        public override SyntaxKind Kind => SyntaxKind.IdentifierExpression;

        public IdentifierExpression(SourceSpan span, string identifier) : base(span)
        {
            Identifier = identifier;
            //Console.WriteLine($"{Display()}  Identifier {Identifier}");
        }
    }
}
