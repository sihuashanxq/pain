using System;
using Pain.Compilers.Expressions;
namespace Pain.Compilers.Parsers.Statements
{
    public class ClassDefinition
    {
        public string Name { get; }

        public FunctionExpression Constructor { get; }

        public FunctionExpression[] Functions { get; }

        public ClassDefinition(string name,FunctionExpression constructor, FunctionExpression[] functions)
        {
            Name = name;
            Functions = functions;
            Constructor = constructor;
        }
    }
}

