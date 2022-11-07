using Pain.Compilers.Expressions;
using System.Text;
namespace Pain.Compilers.Parsers.Definitions
{
    public class Class
    {
        public string Name { get; }

        public string Super { get; set; }

        public List<FunctionExpression> Functions { get; }

        public Class(string name, string super)
        {
            Name = name;
            Super = super;
            Functions = new List<FunctionExpression>();
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

        public void AddFunction(FunctionExpression function)
        {
            Functions.Add(function);
        }
    }
}

