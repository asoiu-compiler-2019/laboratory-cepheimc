
namespace Interpreter.Syntax.Statements
{
    public class EmptyStatement : Statement
    {
        public override SyntaxKind Kind => SyntaxKind.EmptyStatement;

        public EmptyStatement(SourceSpan span) : base(span)
        {
        }
    }
}
