
namespace Interpreter.Syntax.Declarations
{
    public abstract class Declaration : SyntaxNode
    {
        public override SyntaxCatagory Catagory => SyntaxCatagory.Declaration;


        public string Name { get; }

        public override string Display()
        {
            return $"    " + base.Display();
            
            //Console.WriteLine($"    {Kind} span {Span} priority {Span.Priority}");
        }

        protected Declaration(SourceSpan span, string name) : base(span)
        {
            Name = name;
            
            //Console.WriteLine($"{Display()} "); 
        }
    }
    
}
