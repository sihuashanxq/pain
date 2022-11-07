using System;
using System.Text;
namespace Pain.Compilers.Expressions
{
    public class FunctionExpression : Syntax
    {
        public override SyntaxType Type => SyntaxType.Function;

        public string Name { get; set; }

        public bool Local { get; }

        public Syntax Body { get; set; }

        public bool Native { get; }

        public ParameterExpression[] Parameters { get; }

        public HashSet<ICaptureable> CapturedVariables { get; }

        public Dictionary<ICaptureable, ICaptureable> CaptureVariables { get; }

        public FunctionExpression(string name, bool native, bool local, ParameterExpression[] parameters, Syntax body)
        {
            Name = name;
            Body = body;
            Local = local;
            Native = native;
            Parameters = parameters;
            CaptureVariables = new Dictionary<ICaptureable, ICaptureable>();
            CapturedVariables = new HashSet<ICaptureable>();
        }

        public override T Accept<T>(SyntaxVisitor<T> visitor)
        {
            return visitor.VisitFunction(this);
        }

        public override string ToString()
        {
            return new StringBuilder().
                Append("func  ").
                Append(Name).
                Append("(").
                Append(string.Join(",", Parameters.Select(i => i.Name))).
                AppendLine("){").
                AppendLine(Body.ToString()).
                AppendLine("}").ToString();
        }
    }
}
