
namespace Interpreter.Syntax
{
    public abstract class SyntaxNode
    {
        public abstract SyntaxCatagory Catagory { get; }

        public abstract SyntaxKind Kind { get; }

        public SourceSpan Span { get; }

        public SyntaxNode Parent;

        public int priority;

        public virtual string Display()
        {
            return $"";
        }

        protected SyntaxNode(SourceSpan span)
        {
           Span = span;
           //priority++;
           Display();
        }
    }
}
