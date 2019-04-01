using System;
using System.Collections.Generic;
using System.Linq;
using Interpreter.Syntax;
using Interpreter.Syntax.Declarations;
using Interpreter.Syntax.Expressions;
using Interpreter.Syntax.Statements;

namespace Interpreter.Parser
{
    public partial class Parsers
    {
        public List<SyntaxNode> statements;

        private SourceDoc ParseDocument()
        {
         
            List<SyntaxNode> contents = new List<SyntaxNode>();

            var start = current.Span.Start;
            while (current == "func")
            {
                contents.Add(ParseFuncDeclaration());
            }

            if (options.AllowRootStatements)
            {
                List<SyntaxNode> statements = new List<SyntaxNode>();

                var statementsStart = current.Span.Start;
                while (current != TokenKind.EndOfFile)
                {
                    statements.Add(ParseStatement());
                }
                
                contents.Add(new BlockStatement(CreateSpan(statementsStart), statements));
            }

            if (current != TokenKind.EndOfFile)
            {
                AddError(Severity.Error, "Top-level statements are not permitted within the current options.", CreateSpan(current.Span.Start, tokens.Last().Span.End));
            }

            return new SourceDoc(CreateSpan(start), sourceCode, contents);
        }
        
        private FuncDeclaration ParseFuncDeclaration()
        {
            var start = TakeKeyword("func");
            var name = ParseName();
            var returnType = "Object";

            if (current == TokenKind.LessThan)
            {
                returnType = ParseTypeAnnotation();
            }

            var parameters = ParseParameterList();

           
            var body = ParserScope();

           // Console.WriteLine($"body {body.Kind}");

            FuncDeclaration f = new FuncDeclaration(CreateSpan(start), name, returnType, parameters, body);
            statm.Add(f);
            return f;
        }

        private BlockStatement ParserScope()
        {
            return ParseScope();
           
        }

        private string ParseName()
        {
            return Take(TokenKind.Identifier).Value;
        }

        private ParameterDeclaration ParseParameterDeclaration()
        {
            var name = Take(TokenKind.Identifier);
            var type = "Object";

            if (current == TokenKind.Colon)
            {
                Take();
                type = ParseName();
            }

            ParameterDeclaration p = new ParameterDeclaration(CreateSpan(name), name.Value, type);
            statm.Add(p);
            return p;
        }

        private IEnumerable<ParameterDeclaration> ParseParameterList()
        {
            List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();
            MakeBlock(() =>
            {
                if (current == TokenKind.RightParenthesis)
                {
                    return;
                }
                parameters.Add(ParseParameterDeclaration());
                while (current == TokenKind.Comma)
                {
                    Take(TokenKind.Comma);
                    parameters.Add(ParseParameterDeclaration());
                }
            }, TokenKind.LeftParenthesis, TokenKind.RightParenthesis);

            return parameters;
        }

        private string ParseTypeAnnotation()
        {
            if (current != TokenKind.LessThan)
            {
                throw UnexpectedToken("Type Annotation");
            }
            Take(TokenKind.LessThan);
            var identifier = ParseName();
            Take(TokenKind.GreaterThan);

            return identifier;
        }

        private VariableDeclaration ParseVariableDeclaration()
        {
            var start = TakeKeyword("var");
            var name = ParseName();
            
            var type = "object";
            Expression value = null;

           
            if (current == TokenKind.Assignment)
            {
                Take();
                value = ParseExpression();
            }

            VariableDeclaration v = new VariableDeclaration(CreateSpan(start), name, type, value);
            statm.Add(v);
            return v;
        }

        private VariableDeclaration ParsePhotoVarDeclaration()
        {
            var start = TakeKeyword("photo");
            var name = ParseName();

            var type = "photo";
            Expression value = null;


            if (current == TokenKind.Assignment)
            {
                Take();
                value = ParseExpression();
            }

            VariableDeclaration v = new VariableDeclaration(CreateSpan(start), name, type, value);
            statm.Add(v);
            return v;
        }

        private VariableDeclaration ParseDocVarDeclaration()
        {
            var start = TakeKeyword("document");
            var name = ParseName();

            var type = "document";
            Expression value = null;


            if (current == TokenKind.Assignment)
            {
                Take();
                value = ParseExpression();
            }

            VariableDeclaration v = new VariableDeclaration(CreateSpan(start), name, type, value);
            statm.Add(v);
            return v;
        }
    }
}
