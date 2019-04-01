using System;
using System.Collections.Generic;
using Interpreter.Syntax.Expressions;

namespace Interpreter.Syntax.Statements
{
    public class CaseStatement : Statement
    {
        public IEnumerable<SyntaxNode> Body { get; }

        public IEnumerable<Expression> Cases { get; }

        public override SyntaxKind Kind => SyntaxKind.CaseStatement;

        public CaseStatement(SourceSpan span, IEnumerable<Expression> cases, IEnumerable<SyntaxNode> body) : base(span)
        {
            Body = body;
            Cases = cases;
           /* Console.WriteLine($"        {base.Display()}Case statement: ");
            foreach (var c in Cases)
            {
                if (c.Kind == SyntaxKind.IdentifierExpression)
                {
                    IdentifierExpression l = (IdentifierExpression)c;
                    Console.WriteLine($"         {base.Display()}case {c.Display()} {l.Kind} {l.Identifier}");
                }
                else
                {
                    Console.WriteLine($"          {base.Display()}case {c.Display()} {c.Kind}");
                }
               
            }
            foreach (var c in Body)
            {
                if (c.Kind == SyntaxKind.FuncCallExpression)
                {
                    FuncCallExpression l = (FuncCallExpression)c;
                    Console.WriteLine($"          {base.Display()}body {c.Display()} {l.Kind}");
                    foreach (var a in l.Arguments)
                    {
                        if (a.Kind == SyntaxKind.IdentifierExpression)
                        {
                            IdentifierExpression i = (IdentifierExpression)a;
                            Console.WriteLine($"          {Display()}  argument {l.Kind} {i.Identifier}");
                        }
                        else
                        {
                            Console.WriteLine($"          {Display()}  argument {a.Kind}");
                        }
                    }
                    if (c.Kind == SyntaxKind.IdentifierExpression)
                    {
                        IdentifierExpression r = (IdentifierExpression)c;
                        Console.WriteLine($"          {Display()}  ref {r.Kind} {r.Identifier}");
                    }
                    else
                    {
                        Console.WriteLine($"          {Display()}  ref {c.Kind}");
                    }
                }
                else
                {
                    Console.WriteLine($"          {base.Display()}case {c.Display()} {c.Kind}");
                    Console.WriteLine($"          {base.Display()}body {c.Display()} {c.Kind}");
                }
                
            }*/
            
            
        }
    }
}
