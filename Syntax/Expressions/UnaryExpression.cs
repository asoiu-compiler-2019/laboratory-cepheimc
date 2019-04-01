
namespace Interpreter.Syntax.Expressions
{
    public class UnaryExpression : Expression
    {
        public Expression Argument { get; }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;

        public UnaryOperator Operator { get; }

        public UnaryExpression(SourceSpan span, Expression argument, UnaryOperator op)
            : base(span)
        {
            Argument = argument;
            Operator = op;
        }
    }

    public enum UnaryOperator
    {
        Default,
    }
}
