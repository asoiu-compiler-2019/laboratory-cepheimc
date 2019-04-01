using System;
using System.Collections.Generic;
using System.Linq;
using Interpreter.Syntax.Expressions;
using BinaryExpression = Interpreter.Syntax.Expressions.BinaryExpression;
using ConstantExpression = Interpreter.Syntax.Expressions.ConstantExpression;
using Expression = Interpreter.Syntax.Expressions.Expression;
using FuncCallExpression = Interpreter.Syntax.Expressions.FuncCallExpression;
using NewExpression = Interpreter.Syntax.Expressions.NewExpression;

namespace Interpreter.Parser
{
    public sealed partial class Parsers
    {
        private bool IsAssignmentOperator()
        {
           // Console.WriteLine($"{current.Kind}");
            switch (current.Kind)
            {
                case TokenKind.Assignment:
                   // Console.WriteLine($" ====");
                    return true;

                default: return false;
            }
        }

        private bool IsEqualityOperator()
        {
            switch (current.Kind)
            {
                case TokenKind.Equal:
                case TokenKind.NotEqual:
                    return true;

                default: return false;
            }
        }

        private bool IsLogicalOperator()
        {
            switch (current.Kind)
            {
                case TokenKind.BooleanOr:
                case TokenKind.BooleanAnd:
                    return true;

                default: return false;
            }
        }

        private bool IsRelationalOperator()
        {
            switch (current.Kind)
            {
                case TokenKind.GreaterThan:
                case TokenKind.LessThan:
                case TokenKind.GreaterThanOrEqual:
                case TokenKind.LessThanOrEqual:
                    return true;

                default: return false;
            }
        }

        private Expression ParseExp()
        {
            SourceLocation? start = null;

            if (start == null)
            {
                start = current.Span.Start;
            }
            var expression = ParsePrimaryExpression();

            statm.Add(expression);
           // Console.WriteLine($"{expression.Kind} start {expression.Span}");
            return expression;
        }

        private Expression ParseRelationalExpression()
        {
            Expression left = ParseExp();

            while (IsRelationalOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseExp();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
                statm.Add(left);
            }
            return left;
        }

        private Expression ParseLogicalExpression()
        {
            Expression left = ParseEqualityExpression();
            while (IsLogicalOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseEqualityExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
                statm.Add(left);
            }
            return left;
        }

        private Expression ParseAssignmentExpression()
        {
           // Console.WriteLine($" start in assign");

            Expression left = ParsePrimaryExpression();
            Token lasts = last; 
            if (IsAssignmentOperator())
            {
                // Assignment is right-associative.
               
                var op = ParseBinaryOperator();
                Expression right = ParseAssignmentExpression();

               // Console.WriteLine($"in assign");

                BinaryExpression b = new BinaryExpression(CreateSpan(left), left, right, op);
                statm.Add(b);
                return b;
            }
            
            return left;
        }

        private BinaryOperator ParseBinaryOperator()
        {
            var token = Take();
            switch (token.Kind)
            {
                case TokenKind.Assignment: return BinaryOperator.Assign;
                case TokenKind.Equal: return BinaryOperator.Equal;
                case TokenKind.NotEqual: return BinaryOperator.NotEqual;
                case TokenKind.GreaterThan: return BinaryOperator.GreaterThan;
                case TokenKind.LessThan: return BinaryOperator.LessThan;
                case TokenKind.GreaterThanOrEqual: return BinaryOperator.GreaterThanOrEqual;
                case TokenKind.LessThanOrEqual: return BinaryOperator.LessThanOrEqual;
            }

            index--;
            throw UnexpectedToken("Binary Operator");
        }

        private Expression ParseConstantExpression()
        {
            ConstantKind kind;
            if (current == "true" || current == "false")
            {
                kind = ConstantKind.Boolean;
            }
            else if (current == TokenKind.StringLiteral)
            {
                kind = ConstantKind.String;
            }
            else if (current == TokenKind.IntegerLiteral)
            {
                kind = ConstantKind.Integer;
            }
            else if (current == TokenKind.FloatLiteral)
            {
                kind = ConstantKind.Float;
            }
            else
            {
                throw UnexpectedToken("Constant");
            }

            var token = Take();

            var expr = new ConstantExpression(token.Span, token.Value, kind);
            statm.Add(expr);

            return expr;
        }

        private Expression ParseEqualityExpression()
        {
            Expression left = ParseRelationalExpression();
            while (IsEqualityOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseRelationalExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
                statm.Add(left);
            }

            return left;
        }

        private Expression ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        private Expression ParseIdentiferExpression()
        {
            var token = Take(TokenKind.Identifier);

            IdentifierExpression i = new IdentifierExpression(token.Span, token.Value);
            statm.Add(i);
            return i;
        }

       
        private Expression ParseFuncCallExpression()
        {
            var hint = ParseIdentiferExpression();
            return ParseFuncCallExpression(hint);
        }

        private Expression ParseFuncCallExpression(Expression reference)
        {
            var arguments = new List<Expression>();
            MakeBlock(() =>
            {
                if (current != TokenKind.RightParenthesis)
                {
                    arguments.Add(ParseExpression());
                    while (current == TokenKind.Comma)
                    {
                        Take(TokenKind.Comma);
                        arguments.Add(ParseExpression());
                    }
                }
            }, TokenKind.LeftParenthesis, TokenKind.RightParenthesis);

            var expr = new FuncCallExpression(CreateSpan(reference), reference, arguments);
            if (current == TokenKind.Dot)
            {
                return ParseReferenceExpression(expr);
            }
            statm.Add(expr);
            return expr;
        }

        private Expression ParseNewExpression()
        {
            var start = TakeKeyword("new");
            List<Expression> references = new List<Expression>();
            List<Expression> arguments = new List<Expression>();

            Expression reference;

            references.Add(ParseIdentiferExpression());
            while (current == TokenKind.Dot)
            {
                Take(TokenKind.Dot);
                references.Add(ParseIdentiferExpression());
            }

            if (references.Count == 1)
            {
                reference = references.First();
            }
            else
            {
                reference = new ReferenceExpression(CreateSpan(references.First()), references);
            }

            MakeBlock(() =>
            {
                if (current != TokenKind.RightParenthesis)
                {
                    arguments.Add(ParseExpression());
                    while (current == TokenKind.Comma)
                    {
                        arguments.Add(ParseExpression());
                    }
                }
            }, TokenKind.LeftParenthesis, TokenKind.RightParenthesis);

            var expr = new NewExpression(CreateSpan(start), reference, arguments);
            if (current == TokenKind.Dot)
            {
                return ParseReferenceExpression(expr);
            }

            statm.Add(expr);
            return expr;
        }

      /*  private Expression ParseOverrideExpression()
        {
            var start = Take(TokenKind.LeftParenthesis).Span.Start;

            if (current == TokenKind.RightParenthesis)
            {
                Take(TokenKind.RightParenthesis);
                return ParseLambdaExpression(start, new ParameterDeclaration[0]);
            }

            if (current == TokenKind.Identifier
                && (next == TokenKind.Comma
                    || next == TokenKind.Colon))
            {
                index--;
                var parameters = ParseParameterList();
                return ParseLambdaExpression(start, parameters);
            }

            var expr = ParseExpression();

            Take(TokenKind.RightParenthesis);

            if (current == TokenKind.LeftParenthesis)
            {
                return ParseMethodCallExpression(expr);
            }
            else if (current == TokenKind.Dot)
            {
                return ParseReferenceExpression(expr);
            }

            return expr;
        }*/

        private Expression ParsePredicate()
        {
            Expression expr = null;
            MakeBlock(() => { expr = ParseLogicalExpression(); }, TokenKind.LeftParenthesis,
                TokenKind.RightParenthesis);
            statm.Add(expr);
            return expr;
        }

        private Expression ParsePrimaryExpression()
        {
            if (current == TokenKind.Identifier)
            {
                if (next == TokenKind.Dot)
                {
                    return ParseReferenceExpression();
                }
                else if (next == TokenKind.LeftParenthesis)
                {
                    return ParseFuncCallExpression();
                }

                return ParseIdentiferExpression();
            }
            else if (next == TokenKind.LeftParenthesis)
            {
                return ParseFuncCallExpression();
            }
            else if (current.Catagory == TokenCatagory.Constant || current == "true" || current == "false")
            {
                return ParseConstantExpression();
            }
           /* else if (current == TokenKind.LeftParenthesis)
            {
                return ParseOverrideExpression();
            }*/
            else
            {
                if (current.Catagory == TokenCatagory.Operator)
                {
                    var token = current;
                    Advance();
                    throw SyntaxError(Severity.Error, $"'{token.Value}' is an invalid expression term.", token.Span);
                }

                throw UnexpectedToken("Expression Term");
            }
        }

        private Expression ParseReferenceExpression(Expression hint)
        {
            var references = new List<Expression>();
            references.Add(hint);

            do
            {
                Take(TokenKind.Dot);
                if (current == TokenKind.Identifier)
                {
                    var expr = ParseIdentiferExpression();
                    references.Add(expr);
                }

                if (current == TokenKind.LeftParenthesis)
                {
                    var expr = new ReferenceExpression(CreateSpan(hint), references);
                    return ParseFuncCallExpression(expr);
                }
                
            } while (current == TokenKind.Dot);

            ReferenceExpression r = new ReferenceExpression(CreateSpan(hint), references);
            statm.Add(r);
            return r;
        }

        private Expression ParseReferenceExpression()
        {
            var hint = ParseIdentiferExpression();
            return ParseReferenceExpression(hint);
        }
    }
}
