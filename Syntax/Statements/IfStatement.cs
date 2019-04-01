using Interpreter.Syntax.Expressions;

namespace Interpreter.Syntax.Statements
{
    public class IfStatement : Statement
    {
        public BlockStatement Body { get; }

        public ElseStatement ElseStatement { get; }

        public override SyntaxKind Kind => SyntaxKind.IfStatement;

        public Expression Predicate { get; }

        public IfStatement(SourceSpan span, Expression predicate, BlockStatement body, ElseStatement elseStatement)
            : base(span)
        {
            Predicate = predicate;
            Body = body;
            ElseStatement = elseStatement;
        }
    }
}
