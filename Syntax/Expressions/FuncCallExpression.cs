using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Syntax.Expressions
{
    public class FuncCallExpression : Expression
    {
        public IEnumerable<Expression> Arguments { get; }

        public override SyntaxKind Kind => SyntaxKind.FuncCallExpression;

        public Expression Reference { get; }

        public FuncCallExpression(SourceSpan span, Expression reference, IEnumerable<Expression> arguments)
            : base(span)
        {
            Reference = reference;
            Arguments = arguments;
           // Console.WriteLine($"  {Display()} {Kind} ");
            
            /*foreach (var a in Arguments)
            {
                if (a.Kind == SyntaxKind.IdentifierExpression)
                {
                    IdentifierExpression l = (IdentifierExpression)a;
                    Console.WriteLine($"  {Display()}  argument {l.Kind} {l.Identifier}");
                }
                else
                {
                    Console.WriteLine($"  {Display()}  argument {a.Kind}");
                }
            }

            if (reference.Kind == SyntaxKind.IdentifierExpression)
            {
                IdentifierExpression l = (IdentifierExpression)reference;
                Console.WriteLine($"  {Display()}  ref {l.Kind} {l.Identifier}");
            }
            else
            {
                Console.WriteLine($"  {Display()}  ref {reference.Kind}");
            }*/
           // Console.WriteLine($"{Display()}  ref {reference.Kind}");
        }
    }
}
