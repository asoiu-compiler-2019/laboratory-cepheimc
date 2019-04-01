using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Syntax.Expressions
{
    public abstract class Expression : SyntaxNode
    {
        public override SyntaxCatagory Catagory => SyntaxCatagory.Expression;

        public override string Display()
        {
            return $"    " + base.Display();
            
           // Console.WriteLine($"            {Kind} span {Span} priority {Span.Priority}");
        }

        protected Expression(SourceSpan span) : base(span)
        {
            
        }
    }
}
