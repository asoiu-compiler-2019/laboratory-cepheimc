using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Syntax
{
    public enum SyntaxKind
    {
        Invalid,
        UnaryExpression,
        SourceDocument,
        BinaryExpression,
        IdentifierExpression,
        ConstantExpression,
        ReferenceExpression,
        FuncCallExpression,
        ParameterDeclaration,
        BlockStatement,
        LambdaExpression,
        NewExpression,
        IfStatement,
        ElseStatement,
        SwitchStatement,
        CaseStatement,
        EmptyStatement,
        BreakStatement,
        VariableDeclaration,
        FuncDeclaration,
        ReturnStatement,
    }
}
