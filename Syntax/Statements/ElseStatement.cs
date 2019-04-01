using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Syntax.Statements
{
    public class ElseStatement : Statement
    {
        public BlockStatement Body { get; }

        public override SyntaxKind Kind => SyntaxKind.ElseStatement;

        public ElseStatement(SourceSpan span, BlockStatement body) : base(span)
        {
            Body = body;
        }
    }
}
