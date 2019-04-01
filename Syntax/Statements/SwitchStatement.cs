using System;
using System.Collections.Generic;
using Interpreter.Syntax.Expressions;

namespace Interpreter.Syntax.Statements
{
    public class SwitchStatement : Statement
    {
        public Expression Identifier { get; }
        public IEnumerable<CaseStatement> Cases { get; }

        public override SyntaxKind Kind => SyntaxKind.SwitchStatement;

        public SwitchStatement(SourceSpan span, Expression identifier, IEnumerable<CaseStatement> cases) : base(span)
        {
            Identifier = identifier;
            Cases = cases;
           // Console.WriteLine($"    {Display()}Switch statement: ");
        }
    }
}
