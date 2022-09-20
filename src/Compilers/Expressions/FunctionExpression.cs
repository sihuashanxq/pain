using System;
namespace Pain.Compilers.Expressions
{
    public class FunctionExpression:Syntax
    {
        public override SyntaxType Type => SyntaxType.Function;

        public string Name { get; }

        public bool IsStatic { get; }

        public bool IsNative { get; }

        public Syntax Body { get; }

        public bool IsConstructor { get; }

        public Syntax[] Parameters { get; }

        public FunctionExpression(string name, bool isStatic, bool isNative, bool isConstructor, Syntax[] parameters, Syntax body)
        {
            Name = name;
            Body = body;
            IsStatic = isStatic;
            IsNative = isNative;
            Parameters = parameters;
            IsConstructor = isConstructor;
        }

        public override T Accept<T>(SyntaxVisitor<T> visitor)
        {
            return visitor.VisitFunction(this);
        }
    }
}
