using Pain.Compilers.Expressions;
namespace Pain.Compilers.Parsers.Definitions
{
    public class ClassDefinition
    {
        public string Name { get; }

        public string Super { get; }

        public FunctionExpression[] Functions { get; }

        public ClassDefinition(string name,string super, FunctionExpression[] functions)
        {
            Name = name;
            Super = super;
            Functions = functions;
        }
    }
}

