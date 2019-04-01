using System;
using System.Collections.Generic;
using Interpreter.Syntax;
using Interpreter.Syntax.Expressions;
using Interpreter.Syntax.Statements;
using Interpreter.Syntax.Declarations;

namespace Interpreter.Parser
{
    public sealed partial class Parsers
    {
        public SyntaxNode ParseStatement()
        {
            SyntaxNode value = null;
            if (current == TokenKind.Keyword)
            {
                switch (current.Value)
                {
                    case "true":
                    case "false":
                        value = ParseExpression();
                        break;

                    case "if":
                        value = ParseIfStatement();
                        break;

                    case "switch":
                        value = ParseSwitchStatement();
                        break;

                    case "return":
                        value = ParseReturnStatement();
                        break;

                    case "var":
                        value = ParseVariableDeclaration();
                        break;

                    case "photo":
                        value = ParsePhotoVarDeclaration();
                        break;

                    case "document":
                        value = ParseDocVarDeclaration();
                        break;

                    default:
                        throw UnexpectedToken("if or switch");
                }
            }
            else if (current == TokenKind.Semicolon)
            {
                var token = TakeSemicolon();
                AddError(Severity.Warning, "Possibly mistaken empty statement.", token.Span);
                return new EmptyStatement(CreateSpan(token));
            }
            else
            {
                MakeStatement(() =>
                {
                    value = ParseExpression();
                });
                return value;
            }
            if (last != TokenKind.RightBracket)
            {
                TakeSemicolon();
            }
            return value;
        }

        private CaseStatement ParseCaseStatement()
        {
            List<Expression> conditions = new List<Expression>();
            List<SyntaxNode> contents = new List<SyntaxNode>();
            var start = current;

            while (current == "case")
            {
                Take();
                var condition = ParseExpression();
                Take(TokenKind.Colon);
                conditions.Add(condition);
            }

            while (current != "case" && current != TokenKind.RightBracket)
            {
                contents.Add(ParseStatement());
            }

            foreach (var c in conditions)
            {
               // Console.WriteLine($"  case {c.Display()} {c.Kind}");
            }
            foreach (var c in contents)
            {
                //Console.WriteLine($"  {base.Display()}body {c.Display()} {c.Kind}");
            }

            CaseStatement case1 = new CaseStatement(CreateSpan(start), conditions, contents);
            statm.Add(case1);
            return case1;
        }

        private ElseStatement ParseElseStatement()
        {
            var start = TakeKeyword("else");

            var body = ParseStatementOrScope();
            ElseStatement else1 = new ElseStatement(CreateSpan(start), body);
            statm.Add(else1);
            return else1;
        }

        private BlockStatement ParseExpressionOrScope()
        {
            if (current == TokenKind.LeftBracket)
            {
                return ParseScope();
            }
            else
            {
                var start = current;
                var expr = ParseExpression();
                return new BlockStatement(CreateSpan(start), new[] { expr });
            }
        }

        private IfStatement ParseIfStatement()
        {
            var start = TakeKeyword("if");
            var predicate = ParsePredicate();
            var body = ParseStatementOrScope();

            ElseStatement elseStatement = null;
            if (current == "else")
            {
                elseStatement = ParseElseStatement();
            }

            return new IfStatement(CreateSpan(start), predicate, body, elseStatement);
        }

        private ReturnStatement ParseReturnStatement()
        {
            var start = TakeKeyword("return");
            var value = ParseExpression();

            return new ReturnStatement(CreateSpan(start), value);
        }

        private BlockStatement ParseScope()
        {
            List<SyntaxNode> contents = new List<SyntaxNode>();
            var start = current;
            MakeBlock(() =>
            {
                contents.Add(ParseStatement());
            });

            foreach (var c in contents)
            {
                //Console.WriteLine($"contents {c.Kind}");
            }

            BlockStatement block = new BlockStatement(CreateSpan(start), contents);
            statm.Add(block);
            return block;
        }

        private BlockStatement ParseStatementOrScope()
        {
            if (current == TokenKind.LeftBracket)
            {
                return ParseScope();
            }
            else
            {
                var statement = ParseStatement();
                return new BlockStatement(statement.Span, new[] { statement });
            }
        }

        private SwitchStatement ParseSwitchStatement()
        {
            List<CaseStatement> cases = new List<CaseStatement>();

            var start = TakeKeyword("switch");

            Expression expr;
            MakeBlock(() => expr = ParseExpression(), TokenKind.LeftParenthesis, TokenKind.RightParenthesis);
            MakeBlock(() =>
            {
                while (current == "case")
                {
                    cases.Add(ParseCaseStatement());
                }
            });

            SwitchStatement sw = new SwitchStatement(CreateSpan(start), cases);
            statm.Add(sw);
            return sw;
        }

    }
}
