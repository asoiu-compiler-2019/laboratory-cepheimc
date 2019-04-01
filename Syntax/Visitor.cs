using Interpreter.Syntax.Declarations;
using Interpreter.Syntax.Expressions;
using Interpreter.Syntax.Statements;
using BinaryExpression = Interpreter.Syntax.Expressions.BinaryExpression;
using ConstantExpression = Interpreter.Syntax.Expressions.ConstantExpression;
using Expression = Interpreter.Syntax.Expressions.Expression;
using LambdaExpression = Interpreter.Syntax.Expressions.LambdaExpression;

namespace Interpreter.Syntax
{
    public abstract class Visitor
    {
        public void Visit(SyntaxNode node)
        {
            switch (node.Catagory)
            {
                case SyntaxCatagory.Document:
                    VisitDocument(node as SourceDoc);
                    break;

                case SyntaxCatagory.Expression:
                    VisitExpression(node as Expression);
                    break;

                case SyntaxCatagory.Statement:
                    VisitStatement(node as Statement);
                    break;

                case SyntaxCatagory.Declaration:
                    VisitDeclaration(node as Declaration);
                    break;
            }
        }

        // protected abstract void VisitArithmetic(BinaryExpression expression);

        protected abstract void VisitAssignment(BinaryExpression expression);

        protected void VisitBinary(BinaryExpression expression)
        {
            switch (expression.Operator)
            {
                case BinaryOperator.Assign:
                    VisitAssignment(expression);
                    break;

                case BinaryOperator.Equal:
                case BinaryOperator.GreaterThan:
                case BinaryOperator.GreaterThanOrEqual:
                case BinaryOperator.LessThanOrEqual:
                case BinaryOperator.NotEqual:
                    VisitLogical(expression);
                    break;
            }
        }

        //protected abstract void VisitBitwise(BinaryExpression expression);

        protected abstract void VisitBlock(BlockStatement statement);
        
        protected abstract void VisitBreak(BreakStatement statement);

        protected abstract void VisitCase(CaseStatement statement);

        protected abstract void VisitConstant(ConstantExpression expression);

        protected void VisitDeclaration(Declaration node)
        {
            switch (node.Kind)
            {
                case SyntaxKind.ParameterDeclaration:
                    VisitParameter(node as ParameterDeclaration);
                    break;

                case SyntaxKind.FuncDeclaration:
                    VisitFunc(node as FuncDeclaration);
                    break;
                case SyntaxKind.VariableDeclaration:
                    VisitVariable(node as VariableDeclaration);
                    break;
            }
        }

        protected abstract void VisitVariable(VariableDeclaration variableDeclaration);

        protected void VisitDocument(SourceDoc sourceDocument)
        {
            foreach (var node in sourceDocument.Children)
            {
                Visit(node);
            }
        }

        protected abstract void VisitElse(ElseStatement statement);

        protected abstract void VisitEmpty(EmptyStatement statement);

        protected void VisitExpression(Expression expression)
        {
            switch (expression.Kind)
            {
                case SyntaxKind.BinaryExpression:
                    VisitBinary(expression as BinaryExpression);
                    break;

                case SyntaxKind.ConstantExpression:
                    VisitConstant(expression as ConstantExpression);
                    break;

                case SyntaxKind.IdentifierExpression:
                    VisitIdentifier(expression as IdentifierExpression);
                    break;

                case SyntaxKind.LambdaExpression:
                    VisitLambda(expression as LambdaExpression);
                    break;

                case SyntaxKind.FuncCallExpression:
                    VisitFuncCall(expression as FuncCallExpression);
                    break;

                case SyntaxKind.ReferenceExpression:
                    VisitReference(expression as ReferenceExpression);
                    break;


            }
        }
        
        protected abstract void VisitIdentifier(IdentifierExpression expression);

        protected abstract void VisitIf(IfStatement statement);

        protected abstract void VisitFunc(FuncDeclaration methodDeclaration);

        protected abstract void VisitFuncCall(FuncCallExpression expression);

        protected abstract void VisitParameter(ParameterDeclaration parameterDeclaration);

       // protected abstract void VisitProperty(PropertyDeclaration propertyDeclaration);

        protected abstract void VisitReference(ReferenceExpression expression);

        protected void VisitStatement(Statement statement)
        {
            switch (statement.Kind)
            {
                case SyntaxKind.BlockStatement:
                    VisitBlock(statement as BlockStatement);
                    break;

                case SyntaxKind.BreakStatement:
                    VisitBreak(statement as BreakStatement);
                    break;

                case SyntaxKind.CaseStatement:
                    VisitCase(statement as CaseStatement);
                    break;

                case SyntaxKind.ElseStatement:
                    VisitElse(statement as ElseStatement);
                    break;

                case SyntaxKind.EmptyStatement:
                    VisitEmpty(statement as EmptyStatement);
                    break;

                case SyntaxKind.IfStatement:
                    VisitIf(statement as IfStatement);
                    break;

                case SyntaxKind.SwitchStatement:
                    VisitSwitch(statement as SwitchStatement);
                    break;
            }
        }

        protected abstract void VisitSwitch(SwitchStatement statement);

        protected abstract void VisitLambda(LambdaExpression expression);

        protected abstract void VisitLogical(BinaryExpression expression);

    }
}
