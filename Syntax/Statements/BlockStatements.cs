using System;
using System.Collections.Generic;

namespace Interpreter.Syntax.Statements
{
    public class BlockStatement : Statement
    {
        public IEnumerable<SyntaxNode> Contents { get; }

        public override SyntaxKind Kind => SyntaxKind.BlockStatement;

        public BlockStatement(SourceSpan span, IEnumerable<SyntaxNode> contents) : base(span)
        {
            Contents = contents;
            /*Console.WriteLine($"  Block statement: ");
            foreach (var c in Contents)
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
                    if (l.Reference.Kind == SyntaxKind.IdentifierExpression)
                    {
                        IdentifierExpression r = (IdentifierExpression)l.Reference;
                        Console.WriteLine($"          {Display()}  ref {r.Kind} {r.Identifier}");
                    }
                    else
                    {
                        Console.WriteLine($"          {Display()}  ref {c.Kind}");
                    }
                }
                else
                {
                    Console.WriteLine($"          {base.Display()}contest {c.Display()} {c.Kind}");
                }
            
            }*/
        }
    }
}
