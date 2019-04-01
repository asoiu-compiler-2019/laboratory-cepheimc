using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Syntax.Statements
{
    public abstract class Statement : SyntaxNode
    {
        public override SyntaxCatagory Catagory => SyntaxCatagory.Statement;

        public override string Display()
        {
            return $"    " + base.Display();
           // Console.WriteLine($"        {Kind} span {Span}  priority {Span.Priority}");
        }

        protected Statement(SourceSpan span) : base(span)
        {
            priority++;
           // Console.WriteLine($"{Display()}  p {priority}");
        }
    }
    
}
