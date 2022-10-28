using Pain.Compilers.Expressions;
using System.Text;
namespace Pain.Compilers.Parsers.Definitions
{
    public class ClassDefinition
    {
        public string Name { get; }

        public string Super { get; }

        public List<FunctionExpression> Functions { get; }

        public ClassDefinition(string name, string super, IEnumerable<FunctionExpression> functions)
        {
            Name = name;
            Super = super;
            Functions = new List<FunctionExpression>(functions);
        }

        public override string ToString()
        {
            var buf = new StringBuilder();
            buf.Append("class ").Append(Name).AppendLine("{");
            foreach (var item in Functions)
            {
                buf.AppendLine(item.ToString());
            }

            buf.AppendLine("} ");
            return buf.ToString();
        }
    }
}

