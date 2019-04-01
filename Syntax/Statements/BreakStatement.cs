
namespace Interpreter.Syntax.Statements
{
    public class BreakStatement : EmptyStatement
    {
        public override SyntaxKind Kind => SyntaxKind.BreakStatement;

        public BreakStatement(SourceSpan span) : base(span)
        {
        }
    }
}
