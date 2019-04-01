using System;
using System.Collections.Generic;

namespace Interpreter.Syntax
{
    public class SourceDoc : SyntaxNode
    {
        public override SyntaxCatagory Catagory => SyntaxCatagory.Document;

        public IEnumerable<SyntaxNode> Children { get; }

        public override SyntaxKind Kind => SyntaxKind.SourceDocument;

        public SourceCode SourceCode { get; }

        public SourceDoc(SourceSpan span, SourceCode sourceCode, IEnumerable<SyntaxNode> children)
            : base(span)
        {
            SourceCode = sourceCode;
            Children = children;

            Console.WriteLine($"{base.Display()}Source code");
            foreach (var c in children)
            {
                Console.WriteLine($"  {base.Display()}{c.Kind}");
            }
        }
    }
}
