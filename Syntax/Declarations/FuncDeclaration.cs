using System;
using System.Collections.Generic;
using Interpreter.Syntax.Statements;

namespace Interpreter.Syntax.Declarations
{
    public class FuncDeclaration : Declaration
    {
        public BlockStatement Body { get; }

        public override SyntaxKind Kind => SyntaxKind.FuncDeclaration;

        public IEnumerable<ParameterDeclaration> Parameters { get; }

        public string ReturnType { get; }

        public FuncDeclaration(SourceSpan span, string name, string returnType, 
            IEnumerable<ParameterDeclaration> parameters, BlockStatement body) : base(span, name)
        {
            ReturnType = returnType;
            Parameters = parameters;
            Body = body;
            Console.WriteLine($"{Display()} {Kind}");
            Console.WriteLine($"{Display()}  name {name} ");
           // Console.WriteLine($"{Display()}  body {Body.Display()} ");
        }
    }
}
