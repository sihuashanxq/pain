using System;
namespace Pain.Compilers.Expressions;

public abstract class SyntaxVisitor<T>
{
    public virtual T Visit(Syntax syntax)
    {
        return syntax.Accept(this);
    }

    protected internal abstract T VisitBinary(BinaryExpression binaryExpression);

    protected internal abstract T VisitBlock(BlockExpression blockExpression);

    protected internal abstract T VisitBreak(BreakExpression breakExpression);

    protected internal abstract T VisitContinue(ContinueExpression continueExpression);

    protected internal abstract T VisitCall(CallExpression callExpression);

    protected internal abstract T VisitConstant(ConstantExpression constantExpression);

    protected internal abstract T VisitFor(ForExpression forExpression);

    protected internal abstract T VisitFunction(FunctionExpression functionExpression);

    protected internal abstract T VisitIf(IfExpression ifExpression);

    protected internal abstract T VisitMember(MemberExpression memberExpression);

    protected internal abstract T VisitName(NameExpression nameExpression);

    protected internal abstract T VisitNew(NewExpression newExpression);

    protected internal abstract T VisitParameter(ParameterExpression parameterExpression);

    protected internal abstract T VisitReturn(ReturnExpression returnExpression);

    protected internal abstract T VisitSuper(SuperExpression superExpression);

    protected internal abstract T VisitThis(ThisExpression thisExpression);

    protected internal abstract T VisitUnary(UnaryExpression unaryExpression);

    protected internal abstract T VisitVariable(VariableExpression variablExpression);

    protected internal abstract T VisitEmpty(EmptyExpression emptyExpression);

    protected internal abstract T VisitJSONObject(JSONObjectExpression expression);

    protected internal abstract T VisitJSONArray(JSONArrayExpression expression);
}