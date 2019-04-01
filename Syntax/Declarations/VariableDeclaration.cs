
using System;
using System.Collections.Generic;
using Interpreter.Syntax.Expressions;

namespace Interpreter.Syntax.Declarations
{
    public class VariableDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.VariableDeclaration;

        public string Type { get; }

        public Expression Value { get; }

        public VariableDeclaration(SourceSpan span, string name, string type, Expression value) : base(span, name)
        {
            Type = type;
            Value = value;
            Console.WriteLine($"  {Display()} {Kind}");
            Console.WriteLine($"  {Display()}  name {name} ");
        }
    }
}
