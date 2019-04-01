using System;
using System.Collections.Generic;

namespace Interpreter.Syntax.Statements
{
    public class SwitchStatement : Statement
    {
        public IEnumerable<CaseStatement> Cases { get; }

        public override SyntaxKind Kind => SyntaxKind.SwitchStatement;

        public SwitchStatement(SourceSpan span, IEnumerable<CaseStatement> cases) : base(span)
        {
            Cases = cases;
            Console.WriteLine($"    {base.Display()}Switch statement: ");
        }
    }
}
