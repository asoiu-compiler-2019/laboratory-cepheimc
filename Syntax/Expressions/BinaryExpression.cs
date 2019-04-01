
using System;

namespace Interpreter.Syntax.Expressions
{
    public class BinaryExpression : Expression
    {
        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

        public Expression Left { get; }

        public BinaryOperator Operator { get; }

        public Expression Right { get; }

        public BinaryExpression(SourceSpan span, Expression left, Expression right, BinaryOperator op) : base(span)
        {
            Left = left;
            Right = right;
            Operator = op;
          /*  Console.WriteLine($"  {Display()} {Kind}");
            Console.WriteLine($"    {Display()} Operator {Operator.ToString()}");
            if (Left.Kind == SyntaxKind.IdentifierExpression)
            {
                IdentifierExpression l = (IdentifierExpression) left;
                Console.WriteLine($"      {Display()} LEFT {l.Kind} {l.Identifier}");
            }
            else
            
                if (Left.Kind == SyntaxKind.ConstantExpression)
                {
                    ConstantExpression l = (ConstantExpression)left;
                    Console.WriteLine($"      {Display()} LEFT {l.Kind} {l.Value}");
                }
                else
                {
                    Console.WriteLine($"      {Display()} LEFT {Left.Kind}");
                } 

            
            if (Right.Kind == SyntaxKind.IdentifierExpression)
            {
                IdentifierExpression l = (IdentifierExpression)right;
                Console.WriteLine($"      {Display()} RIGHT {l.Kind} {l.Identifier}");
            }
            else

            if (Right.Kind == SyntaxKind.ConstantExpression)
            {
                ConstantExpression l = (ConstantExpression)right;
                Console.WriteLine($"     {Display()} RIGHT {l.Kind} {l.Value}");
            }
            else
            {
                Console.WriteLine($"      {Display()} RIGHT {Right.Kind}");
            }

            */
        }
    }

    public enum BinaryOperator
    {
        #region Assignment

        Assign, //=

        #endregion Assignment

        #region Equality

        Equal, //==
        NotEqual, //!=

        #endregion Equality

        #region Relational

        GreaterThan, //>
        LessThan,    //<
        GreaterThanOrEqual,  //>=
        LessThanOrEqual,   //<=

        #endregion Relational

    }
}
