﻿using System.Text;

namespace Pain.Compilers.Expressions;

public class IfExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.If;

    public Syntax Test { get; }

    public Syntax IfTrue { get; }

    public Syntax IfFalse { get; }

    public IfExpression(Syntax test, Syntax ifTrue, Syntax ifFalse)
    {
        Test = test;
        IfTrue = ifTrue;
        IfFalse = ifFalse;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitIf(this);
    }

    public override string ToString()
    {
        var buf = new StringBuilder();
        buf.Append("if ");
        buf.Append(Test.ToString());
        buf.AppendLine("{");
        buf.AppendLine(IfTrue.ToString()).AppendLine("}");
        if (IfFalse != null)
        {
            buf.Append("else ").Append(IfFalse.ToString());
        }

        return buf.ToString();
    }
}