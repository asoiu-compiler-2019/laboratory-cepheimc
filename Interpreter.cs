using System;
using System.Collections.Generic;
using Interpreter.Parser;
using Interpreter.Syntax;
using Interpreter.Syntax.Expressions;
using Interpreter.Syntax.Statements;
using BinaryExpression = Interpreter.Syntax.Expressions.BinaryExpression;
using ConstantExpression = Interpreter.Syntax.Expressions.ConstantExpression;

namespace Interpreter
{
    public class Interpreter
    {
        private Parsers parser;
        private Dictionary<string, string> variable;

        public Interpreter(Parsers p)
        {
            parser = p;
            variable = new Dictionary<string, string>();
        }

        public void CreateCode()
        {
            foreach (var node in parser.statm)
            {
                if (node.Kind == SyntaxKind.BlockStatement)
                {
                    BlockStatement block = (BlockStatement) node;

                    foreach (var contest in block.Contents)
                    {
                        if (contest.Kind == SyntaxKind.BinaryExpression)
                        {
                            CreateVariable(contest);
                        }

                        if (contest.Kind == SyntaxKind.SwitchStatement)
                        {
                            CreateSwitch(contest);
                        }

                        if (contest.Kind == SyntaxKind.IfStatement)
                        {
                            CreateIf(contest);
                        }

                        if (contest.Kind == SyntaxKind.FuncCallExpression)
                        {
                            FuncCall(contest);
                        }
                    }
                }
                
            }
        }

        void CreateVariable(SyntaxNode node)
            {
                BinaryExpression binExpr = (BinaryExpression) node;
                if (variable.ContainsKey(((IdentifierExpression) binExpr.Left).Identifier))
                    variable.Remove(((IdentifierExpression) binExpr.Left).Identifier);

                if(binExpr.Right.Kind == SyntaxKind.ConstantExpression)
                    variable.Add(((IdentifierExpression) binExpr.Left).Identifier,
                        ((ConstantExpression) binExpr.Right).Value);

                if (binExpr.Right.Kind == SyntaxKind.IdentifierExpression)
                    variable.Add(((IdentifierExpression)binExpr.Left).Identifier,
                        ((IdentifierExpression)binExpr.Right).Identifier);
        }

        void FuncCall(SyntaxNode node)
        {
            FuncCallExpression n = (FuncCallExpression) node;
            if (((IdentifierExpression) n.Reference).Identifier == "print")
            {
                foreach (var argument in n.Arguments)
                {
                    if (argument.Kind == SyntaxKind.ConstantExpression)
                        Print(((ConstantExpression) argument).Value);

                    if (argument.Kind == SyntaxKind.IdentifierExpression)
                    {
                        string s = variable[((IdentifierExpression) argument).Identifier];
                        Print(s);
                    }
                }
            }

            if (((IdentifierExpression) n.Reference).Identifier == "read")
            {
                foreach (var argument in n.Arguments)
                {
                    if (argument.Kind == SyntaxKind.ConstantExpression)
                        Read(((ConstantExpression) argument).Value);
                    
                    if (argument.Kind == SyntaxKind.IdentifierExpression)
                    {
                        string s = variable[((IdentifierExpression) argument).Identifier];
                        Read(s);
                    }
                }
            }

            if (((IdentifierExpression) n.Reference).Identifier == "load")
            {
                foreach (var argument in n.Arguments)
                {
                    if (argument.Kind == SyntaxKind.ConstantExpression)
                        Load(((ConstantExpression) argument).Value);

                    if (argument.Kind == SyntaxKind.IdentifierExpression)
                    {
                        string s = variable[((IdentifierExpression) argument).Identifier];
                        Load(s);
                    }
                }
            }
        }

        void CreateSwitch(SyntaxNode node)
        {
            SwitchStatement sw = (SwitchStatement) node;

            string v = variable[((IdentifierExpression)sw.Identifier).Identifier];

            foreach (var cases in sw.Cases)
            {
                foreach (var c in cases.Cases)
                {
                    if (variable[((IdentifierExpression)c).Identifier] == v)
                    {
                        foreach (var body in cases.Body)
                        {
                            if (body.Kind == SyntaxKind.FuncCallExpression)
                                FuncCall(body);

                            if (body.Kind == SyntaxKind.BinaryExpression)
                                CreateVariable(body);
                        }
                    }

                }
            }
        }

        void CreateIf(SyntaxNode node)
        {
            IfStatement ifS = (IfStatement)node;

            if (ifS.Body.Kind == SyntaxKind.BinaryExpression)
            {
                CreateVariable(ifS.Body);
            }

            if (ifS.Body.Kind == SyntaxKind.FuncCallExpression)
            {
                FuncCall(ifS.Body);
            }

            if(ifS.ElseStatement != null)
            {
                if (ifS.ElseStatement.Kind == SyntaxKind.BinaryExpression)
                {
                    CreateVariable(ifS.ElseStatement);
                }

                if (ifS.ElseStatement.Kind == SyntaxKind.FuncCallExpression)
                {
                    FuncCall(ifS.ElseStatement);
                }
            }

        }
        
        void Print(string s)
        {
            Console.WriteLine("function print  " + s);
        }

        void Read(string s)
        {
            Console.WriteLine("function read  " + s);
        }

        void Load(string s)
        {
            Console.WriteLine("function load  " + s);
        }
    }
}
