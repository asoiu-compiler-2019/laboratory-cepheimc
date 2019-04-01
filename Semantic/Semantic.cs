using System;
using System.Collections.Generic;
using System.Linq;
using Interpreter.Parser;
using Interpreter.Syntax;
using Interpreter.Syntax.Declarations;
using Interpreter.Syntax.Expressions;
using Interpreter.Syntax.Statements;
using BinaryExpression = Interpreter.Syntax.Expressions.BinaryExpression;
using Expression = Interpreter.Syntax.Expressions.Expression;

namespace Interpreter.Semantic
{
    class Semantic
    {
        private Parsers parser;
        public List<string> errors;

        public Semantic(Parsers p)
        {
            parser = p;
            errors = new List<string>();
        }

        public void PrintStatement()
        {
            foreach (var p in parser.statm)
            {
                Console.WriteLine($" {p.Span}      stat {p.Kind}   ");
            }
        }

        public void Analyze(SyntaxNode node)
        {
            if (node.Kind == SyntaxKind.VariableDeclaration)
            {
                CheckVariable();
            }

            if (node.Kind == SyntaxKind.FuncDeclaration)
            {
                CheckFunc();
            }

            if (node.Kind == SyntaxKind.ParameterDeclaration)
            {
                CheckParam();
            }

            if (node.Kind == SyntaxKind.BinaryExpression)
            {
                CheckAssign();
            }

            if (node.Kind == SyntaxKind.FuncCallExpression)
            {
                CheckFuncCall();
            }

            if (node.Kind == SyntaxKind.SwitchStatement)
            {
                CheckSwitch();
            }

            if (node.Kind == SyntaxKind.CaseStatement)
            {
                CheckCase();
            }

           /* if (node.Kind == SyntaxKind.IfStatement)
            {
                CheckIf();
            }

            if (node.Kind == SyntaxKind.ElseStatement)
            {
                CheckElse();
            }*/
        }

        public void AnalyzeFile()
        {
            foreach (var node in parser.statm)
            {
                Analyze(node);
            }
        }

        public List<VariableDeclaration> FindVariable()
        {
            int i = 0;
            List<VariableDeclaration> variable = new List<VariableDeclaration>();
            while (i != parser.statm.Count)
            {
                if (parser.statm[i].Kind == SyntaxKind.VariableDeclaration)
                {
                    variable.Add((VariableDeclaration)parser.statm[i]);
                }

                i++;
            }
           
            return variable;
        }

        public List<ParameterDeclaration> FindParam()
        {
            int i = 0;
            List<ParameterDeclaration> param = new List<ParameterDeclaration>();
            while (i != parser.statm.Count)
            {
                if (parser.statm[i].Kind == SyntaxKind.ParameterDeclaration)
                {
                    param.Add((ParameterDeclaration)parser.statm[i]);
                }

                i++;
            }
            
            return param;
        }

        public List<FuncDeclaration> FindFunction()
        {
            int i = 0;
            List<FuncDeclaration> func = new List<FuncDeclaration>();
            while (i != parser.statm.Count)
            {
                if (parser.statm[i].Kind == SyntaxKind.FuncDeclaration)
                {
                    func.Add((FuncDeclaration)parser.statm[i]);
                }

                i++;
            }
            
            return func;
        }

        public void CheckVariable()
        {
            Dictionary<string, string> variable = new Dictionary<string, string>();
            List<VariableDeclaration> list = FindVariable();
            if (list.Count != 0)
            {
                foreach (var v in list)
                {

                    if (v.Type != "object" && v.Type != "photo" && v.Type != "document")
                    {
                        Console.WriteLine($"1111");
                        errors.Add($"Error: variable type incorrect in line {v.Span.Start.Line}");
                       // Console.WriteLine($"Error: variable type incorrect in line {v.Span.Start.Line}");
                    }

                    if (list.Count(a => a.Name == v.Name) > 1)
                    {
                        errors.Add($"Error: variable name incorrect in line {v.Span.Start.Line}");
                       // Console.WriteLine($"Error: variable name incorrect in line {v.Span.Start.Line}");
                    }

                    if(errors.Count == 0)
                    {
                        variable.Add(v.Name, v.Type);
                    }
                    
                }
                
            }

            foreach (var v in variable)
            {
               // Console.WriteLine($"var name {v.Key} type {v.Value}");
            }
        }

        public void CheckFunc()
        {
            Dictionary<string, string> func = new Dictionary<string, string>();
            List<FuncDeclaration> list = FindFunction();
            if (list.Count != 0)
            {
                foreach (var v in list)
                {
                    if (v.ReturnType != "Object")
                    {
                        errors.Add($"Error: variable type incorrect in line {v.Span.Start.Line}");
                       // Console.WriteLine($"Error: variable type incorrect in line {v.Span.Start.Line}");
                    }

                    if (list.Count(a => a.Name == v.Name) > 1)
                    {
                        errors.Add($"Error: variable name incorrect in line {v.Span.Start.Line}");
                       // Console.WriteLine($"Error: variable name incorrect in line {v.Span.Start.Line}");
                    }

                    if (errors.Count == 0)
                    {
                        func.Add(v.Name, v.ReturnType);
                    }

                }

            }

            foreach (var v in func)
            {
               // Console.WriteLine($"func name {v.Key} return type {v.Value}");
            }
        }

        public void CheckParam()
        {
            Dictionary<string, string> func = new Dictionary<string, string>();
            List<ParameterDeclaration> list = FindParam();
            if (list.Count != 0)
            {
                foreach (var v in list)
                {
                    if (v.Type != "Object")
                    {
                        errors.Add($"Error: variable type incorrect in line {v.Span.Start.Line}");
                       // Console.WriteLine($"Error: variable type incorrect in line {v.Span.Start.Line}");
                    }

                    if (list.Count(a => a.Name == v.Name) > 1)
                    {
                        errors.Add($"Error: variable name incorrect in line {v.Span.Start.Line}");
                       // Console.WriteLine($"Error: variable name incorrect in line {v.Span.Start.Line}");
                    }

                    if (errors.Count == 0)
                    {
                        func.Add(v.Name, v.Type);
                    }

                }

            }

            foreach (var v in func)
            {
               // Console.WriteLine($"param name {v.Key} type {v.Value}");
            }
        }

        public bool IsIdentifier(Expression expr)
        {
            if(expr.Kind == SyntaxKind.IdentifierExpression)
            {
                IdentifierExpression id = (IdentifierExpression)expr;
                foreach (var v in FindVariable())
                {
                    if (v.Name == id.Identifier)
                    {
                        return true;
                    }
                }

                foreach (var v in FindFunction())
                {
                    if (v.Name == id.Identifier)
                    {
                        return true;
                    }
                }

                foreach (var v in FindParam())
                {
                    if (v.Name == id.Identifier)
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        public void CheckAssign()
        {
            Dictionary<string, string> func = new Dictionary<string, string>();
            int i = 0;
            List<BinaryExpression> assign = new List<BinaryExpression>();
            while (i != parser.statm.Count)
            {
                if (parser.statm[i].Kind == SyntaxKind.BinaryExpression)
                {
                    // Console.WriteLine($"{parser.statm[i].Span.Start.Line}");
                    assign.Add((BinaryExpression)parser.statm[i]);
                }

                i++;
            }

            if (assign.Count != 0)
            {
                foreach (var v in assign)
                {
                    if (v.Operator != BinaryOperator.Assign)
                    {
                        errors.Add($"Error: operator in line {v.Span.Start.Line}");
                      //  Console.WriteLine($"Error: operator in line {v.Span.Start.Line}");
                    }
                    if (!IsIdentifier(v.Left))
                    {
                        errors.Add($"Error: left part of binary not identifier in line {v.Span.Start.Line}");
                      //  Console.WriteLine($"Error: left part of binary not identifier in line {v.Span.Start.Line}");
                    }

                    if (!IsIdentifier(v.Right) && v.Right.Kind != SyntaxKind.ConstantExpression)
                    {
                        errors.Add($"Error: right part of binary not identifier in line {v.Span.Start.Line}");
                       // Console.WriteLine($"Error: right part of binary not identifier in line {v.Span.Start.Line}");
                    }

                    if (errors.Count == 0)
                    {
                       // func.Add(v.Left, v.Right);
                    }

                }

            }

            foreach (var v in func)
            {
                //Console.WriteLine($"param name {v.Key} type {v.Value}");
            }
        }

        public void CheckSwitch()
        {
            Dictionary<string, string> func = new Dictionary<string, string>();
            int i = 0;
            List<SwitchStatement> sw = new List<SwitchStatement>();
            while (i != parser.statm.Count)
            {
                if (parser.statm[i].Kind == SyntaxKind.SwitchStatement)
                {
                    // Console.WriteLine($"{parser.statm[i].Span.Start.Line}");
                    sw.Add((SwitchStatement)parser.statm[i]);
                }

                i++;
            }

            if (sw.Count != 0)
            {
                foreach (var v in sw)
                {
                    if (!v.Cases.Any())
                    {
                        errors.Add($"Error: no caseStatement in switch line {v.Span.Start.Line}");
                       // Console.WriteLine($"Error: no caseStatement in switch line {v.Span.Start.Line}");
                    }
                    

                    if (errors.Count == 0)
                    {
                        // func.Add(v.Left, v.Right);
                    }

                }

            }

        }

        public void CheckCase()
        {
            Dictionary<string, string> func = new Dictionary<string, string>();
            int i = 0;
            List<CaseStatement> c = new List<CaseStatement>();
            while (i != parser.statm.Count)
            {
                if (parser.statm[i].Kind == SyntaxKind.CaseStatement)
                {
                    // Console.WriteLine($"{parser.statm[i].Span.Start.Line}");
                    c.Add((CaseStatement)parser.statm[i]);
                }

                i++;
            }

            if (c.Count != 0)
            {
                foreach (var v in c)
                {
                    if (v.Cases.Count() != v.Body.Count())
                    {
                        errors.Add($"Error: case without body {v.Span.Start.Line}");
                      //  Console.WriteLine($"Error: case without body {v.Span.Start.Line}");
                    }

                    foreach (var cas in v.Cases)
                    {
                        if (!IsIdentifier(cas))
                        {
                            errors.Add($"Error: no identifier in case {v.Span.Start.Line}");
                           // Console.WriteLine($"Error: no identifier in case {v.Span.Start.Line}");
                        }
                        
                    }

                    foreach (var body in v.Body)
                    {
                        if (!IsIdentifier((Expression)body))
                        {
                            errors.Add($"Error: no identifier in body {v.Span.Start.Line}");
                           // Console.WriteLine($"Error: no identifier in body {v.Span.Start.Line}");
                        }

                        if (body.Catagory != SyntaxCatagory.Expression && body.Catagory != SyntaxCatagory.Statement)
                        {
                            errors.Add($"Error: no body in case {v.Span.Start.Line}");
                           // Console.WriteLine($"Error: no body in case {v.Span.Start.Line}");
                        }
                    }

                    if (errors.Count == 0)
                    {
                        // func.Add(v.Left, v.Right);
                    }

                }

            }
            
        }

        public void CheckFuncCall()
        {
            int i = 0;
            List<FuncCallExpression> funcCall = new List<FuncCallExpression>();
            while (i != parser.statm.Count)
            {
                if (parser.statm[i].Kind == SyntaxKind.FuncCallExpression)
                {
                    // Console.WriteLine($"{parser.statm[i].Span.Start.Line}");
                    funcCall.Add((FuncCallExpression)parser.statm[i]);
                }

                i++;
            }

            if (funcCall.Count != 0)
            {
                foreach (var v in funcCall)
                {
                    /*if (IsIdentifier(v.Reference))
                    {
                        errors.Add($"Error: no identifier of func name {v.Span.Start.Line}");
                        Console.WriteLine($"Error: no identifier of func name {v.Span.Start.Line}");
                    }*/

                    foreach (var p in v.Arguments)
                    {
                        if (!IsIdentifier(p) && p.Kind != SyntaxKind.ConstantExpression)
                        {
                            errors.Add($"Error: no identifier of func parameter {v.Span.Start.Line}");
                           // Console.WriteLine($"Error: no identifier of func parameter {v.Span.Start.Line}");
                        }
                    }
                    

                    if (errors.Count == 0)
                    {
                        // func.Add(v.Left, v.Right);
                    }

                }

            }
            
        }

        public void CheckIf()
        {
            Dictionary<string, string> func = new Dictionary<string, string>();
            int i = 0;
            List<IfStatement> ifS = new List<IfStatement>();
            while (i != parser.statm.Count)
            {
                if (parser.statm[i].Kind == SyntaxKind.IfStatement)
                {
                    // Console.WriteLine($"{parser.statm[i].Span.Start.Line}");
                    ifS.Add((IfStatement)parser.statm[i]);
                }

                i++;
            }

            if (ifS.Count != 0)
            {
                foreach (var v in ifS)
                {
                    /*if (v.)
                    {
                        errors.Add($"Error: operator in line {v.Span.Start.Line}");
                        Console.WriteLine($"Error: operator in line {v.Span.Start.Line}");
                    }*/
                    

                    if (errors.Count == 0)
                    {
                        // func.Add(v.Left, v.Right);
                    }

                }

            }

            foreach (var v in func)
            {
                //Console.WriteLine($"param name {v.Key} type {v.Value}");
            }
        }

        public void CheckElse()
        {
            Dictionary<string, string> func = new Dictionary<string, string>();
            int i = 0;
            List<BinaryExpression> assign = new List<BinaryExpression>();
            while (i != parser.statm.Count)
            {
                if (parser.statm[i].Kind == SyntaxKind.BinaryExpression)
                {
                    // Console.WriteLine($"{parser.statm[i].Span.Start.Line}");
                    assign.Add((BinaryExpression)parser.statm[i]);
                }

                i++;
            }

            if (assign.Count != 0)
            {
                foreach (var v in assign)
                {
                    if (v.Operator != BinaryOperator.Assign)
                    {
                        errors.Add($"Error: operator in line {v.Span.Start.Line}");
                        Console.WriteLine($"Error: operator in line {v.Span.Start.Line}");
                    }
                    if (!IsIdentifier(v.Left))
                    {
                        errors.Add($"Error: left part of binary not identifier in line {v.Span.Start.Line}");
                        Console.WriteLine($"Error: left part of binary not identifier in line {v.Span.Start.Line}");
                    }

                    if (!IsIdentifier(v.Right) && v.Right.Kind != SyntaxKind.ConstantExpression)
                    {
                        errors.Add($"Error: right part of binary not identifier in line {v.Span.Start.Line}");
                        Console.WriteLine($"Error: right part of binary not identifier in line {v.Span.Start.Line}");
                    }

                    if (errors.Count == 0)
                    {
                        // func.Add(v.Left, v.Right);
                    }

                }

            }

            foreach (var v in func)
            {
                //Console.WriteLine($"param name {v.Key} type {v.Value}");
            }
        }
    }
}
