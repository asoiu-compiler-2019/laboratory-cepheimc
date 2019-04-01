
using System;

namespace Interpreter.Syntax.Expressions
{
    public class ConstantExpression : Expression
    {
        public ConstantKind ConstentKind { get; }

        public override SyntaxKind Kind => SyntaxKind.ConstantExpression;

        public string Value { get; }

        public ConstantExpression(SourceSpan span, string value, ConstantKind kind)
            : base(span)
        {
            Value = value;
            ConstentKind = kind;
           //Console.WriteLine($"Constant {value}");
        }
    }

    public enum ConstantKind
    {
        Invalid,
        Integer,
        Float,
        String,
        Boolean
    }
}
