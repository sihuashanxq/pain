
namespace Pain.Compilers.Parsers;

public class Token
{
    public object Value { get; }

    public TokenType Type { get; }

    public Position Position { get; }

    public Token(TokenType type, object value, Position position)
    {
        Type = type;
        Value = value;
        Position = position;
    }
}