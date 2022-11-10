using System;
namespace Pain.Compilers.Expressions;

public abstract class SyntaxVisitor<T>
{
    public virtual T Visit(Syntax syntax)
    {
        return syntax.Accept(this);
    }

    protected internal abstract T VisitBinary(BinaryExpression expr);

    protected internal abstract T VisitBlock(BlockExpression expr);

    protected internal abstract T VisitBreak(BreakExpression expr);

    protected internal abstract T VisitContinue(ContinueExpression expr);

    protected internal abstract T VisitCall(CallExpression expr);

    protected internal abstract T VisitConstant(ConstantExpression expr);

    protected internal abstract T VisitFor(ForExpression expr);

    protected internal abstract T VisitFunction(FunctionExpression expr);

    protected internal abstract T VisitIf(IfExpression expr);

    protected internal abstract T VisitMember(MemberExpression expr);

    protected internal abstract T VisitName(NameExpression expr);

    protected internal abstract T VisitNew(NewExpression expr);

    protected internal abstract T VisitParameter(ParameterExpression expr);

    protected internal abstract T VisitReturn(ReturnExpression expr);

    protected internal abstract T VisitSuper(SuperExpression expr);

    protected internal abstract T VisitThis(ThisExpression expr);

    protected internal abstract T VisitUnary(UnaryExpression expr);

    protected internal abstract T VisitVariable(VariableExpression expr);

    protected internal abstract T VisitEmpty(EmptyExpression expr);

    protected internal abstract T VisitArrayInit(ArrayInitExpression expr);

    protected internal abstract T VisitMemberInit(MemberInitExpression expr);

    protected internal abstract T VisitTry(TryExpression expr);

    protected internal abstract T VisitCatch(CatchExpression expr);

    protected internal abstract T VisitFinally(FinallyExpression expr);

        protected internal abstract T VisitThrow(ThrowExpression expr);
}