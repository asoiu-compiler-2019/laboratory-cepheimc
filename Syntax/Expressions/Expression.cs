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
            
        }

        protected Expression(SourceSpan span) : base(span)
        {
            
        }
    }
}
