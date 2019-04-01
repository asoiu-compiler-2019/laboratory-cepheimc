using Interpreter.Syntax.Expressions;

namespace Interpreter.Syntax.Statements
{
    public class ReturnStatement : Statement
    {
        public override SyntaxKind Kind => SyntaxKind.ReturnStatement;

        public Expression Value { get; }

        public ReturnStatement(SourceSpan span, Expression value) : base(span)
        {
            Value = value;
        }
    }
}
