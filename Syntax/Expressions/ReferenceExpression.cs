using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Syntax.Expressions
{
    public class ReferenceExpression : Expression
    {
        public override SyntaxKind Kind => SyntaxKind.ReferenceExpression;

        public IEnumerable<Expression> References { get; }

        public ReferenceExpression(SourceSpan span, IEnumerable<Expression> references) : base(span)
        {
            References = references;
        }
    }
}
