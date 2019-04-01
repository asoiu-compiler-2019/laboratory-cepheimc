using System.Collections.Generic;
using Interpreter.Syntax.Declarations;
using Interpreter.Syntax.Statements;

namespace Interpreter.Syntax.Expressions
{
    public class LambdaExpression : Expression
    {
        public BlockStatement Body { get; }

        public override SyntaxKind Kind => SyntaxKind.LambdaExpression;

        public IEnumerable<ParameterDeclaration> Parameters { get; }

        public LambdaExpression(SourceSpan span, IEnumerable<ParameterDeclaration> parameters, BlockStatement body) : base(span)
        {
            Parameters = parameters;
            Body = body;
        }

        public FuncDeclaration ToMethodDeclaration(string name)
        {
            return new FuncDeclaration(Span, name, "Object", Parameters, Body);
        }
    }
}
