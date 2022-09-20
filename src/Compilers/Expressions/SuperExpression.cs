﻿namespace Pain.Compilers.Expressions;

public class SuperExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.Super;

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitSuper(this);
    }
}