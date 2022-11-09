using System.Globalization;
using System.Text;
namespace Pain.Compilers.Parsers;

public static class StringExtensions
{
    public static bool Between(this char ch, char from, char to)
    {
        return ch <= to && ch >= from;
    }

    public static bool StartsWithString(this ReadOnlySpan<char> span, string str)
    {
        return span.StartsWith(str.AsSpan());
    }

    public static bool IsEndNumber(this char ch)
    {
        if (char.IsWhiteSpace(ch))
        {
            return true;
        }

        return ch switch
        {
            ','or '+' or '-' or '/' or '*' or '%' or '&' or '^' or '|' or '!' or '>' or '<' or '=' or '{' or '}' or '(' or ')' or ';' or '[' or ']' => true,
            _ => false,
        };
    }
}

public class Lexer
{
    private int _index;

    private int _line;

    private int _cell;

    private readonly ReadOnlyMemory<char> _source;

    public string FileName { get; }

    public Lexer(string fileName, string source)
    {
        _source = source.AsMemory();
        FileName = fileName;
    }

    public Token ScanNext()
    {
        while (true)
        {
            if (Peek().IsEmpty)
            {
                return new Token(TokenType.EOF, "EOF", Position);
            }

            if (SkipComments() || SkipWhiteSpace())
            {
                continue;
            }

            if (IsString())
            {
                return ScanString();
            }

            if (IsNumber())
            {
                return ScanNumber();
            }

            if (IsIdentifer())
            {
                return ScanIdentifier();
            }

            return ScanPunctuation();
        }
    }

    private Token ScanIdentifier()
    {
        var buf = new StringBuilder();
        var position = new Position(FileName, _line, _cell);
        while (true)
        {
            var ch = Peek();
            if (ch.IsEmpty)
            {
                break;
            }

            if (ch.Is(c => IsIdentifer(c) || (buf.Length != 0 && IsIdentiferPart(c))))
            {
                buf.Append(ch.Char);
                MoveNext();
                continue;
            }

            break;
        }

        var token = new Token(TokenType.Identifier, buf.ToString(), position);
        return LookupKeyword(token);
    }

    public Token ScanString()
    {
        var ch = Peek();
        var buf = new StringBuilder();
        var sep = ch.Char;
        var position = Position;
        MoveNext();

        while (true)
        {
            var chars = Peek(2);
            if (chars.IsEmpty)
            {
                continue;
            }

            if (chars.StartsWithString("\r\n") || chars.StartsWithString("\n\r"))
            {
                ch = new SourceChar(chars[1], false);
                buf.Append(chars);
                MoveNext(2, 1);
                continue;
            }

            var c = chars[0];
            if (c == sep && !ch.Is('\\'))
            {
                MoveNext();
                break;
            }

            ch = new SourceChar(c, false);
            buf.Append(c);
            MoveNext();
        }

        return new Token(TokenType.LiteralString, buf.ToString(), position);
    }

    private Position Position => new Position(FileName, _line, _cell);

    private bool SkipComments()
    {
        if (!Peek().Is('`'))
        {
            return false;
        }

        while (true)
        {
            var chars = Peek(2);
            if (chars.IsEmpty)
            {
                break;
            }

            if (chars.StartsWithString("\r\n") || chars.StartsWithString("\n\r"))
            {
                MoveNext(2, 1);
                continue;
            }

            if (chars[0] == '`')
            {
                MoveNext();
                break;
            }

            if (chars[1] == '`')
            {
                MoveNext(2);
                break;
            }

            MoveNext();
        }

        return true;
    }

    private bool SkipWhiteSpace()
    {
        var whites = 0;
        while (true)
        {
            var chars = Peek(2);
            if (chars.IsEmpty)
            {
                break;
            }

            if (chars.StartsWithString("\r\n") || chars.StartsWithString("\n\r"))
            {
                MoveNext(2, 1);
                whites++;
                continue;
            }

            if (char.IsWhiteSpace(chars[0]))
            {
                MoveNext();
                whites++;
                continue;
            }

            break;
        }

        return whites > 0;
    }

    public Token ScanNumber()
    {
        var position = Position;
        var chars = Peek(2);
        var number = 0d;
        var numberBase = 10;

        if (chars.Length == 2 && chars[0] == '0')
        {
            switch (chars[1])
            {
                case 'x':
                case 'X':
                    numberBase = 16;
                    MoveNext(2);
                    break;
            }
        }

        var buf = new StringBuilder();
        while (true)
        {
            var ch = Peek();
            if (ch.IsEmpty)
            {
                break;
            }

            if (ch.Is('.'))
            {
                if (numberBase != 10)
                {
                    throw new Exception("bad number format");
                }

                buf.Append(ch.Char);
                MoveNext();
                continue;
            }

            var code = int.MaxValue;
            if (ch.Char.Between('0', '9'))
            {
                code = ch.Char - '0';
            }
            else if (ch.Char.Between('a', 'f'))
            {
                code = ch.Char - 'a' + 10;
            }
            else if (ch.Char.Between('A', 'F'))
            {
                code = ch.Char - 'A' + 10;
            }
            else if (ch.Char.IsEndNumber())
            {
                break;
            }

            if (code >= numberBase)
            {
                throw new Exception("bad number format");
            }

            buf.Append(ch.Char);
            MoveNext();
        }

        if (numberBase == 16)
        {
            number = Int64.Parse(buf.ToString(), NumberStyles.HexNumber);
        }
        else
        {
            number = double.Parse(buf.ToString());
        }

        return new Token(TokenType.Number, number, position);
    }

    private Token ScanPunctuation()
    {
        var position = Position;
        switch (Peek().Char)
        {
            case '!':
                MoveNext();
                switch (Peek().Char)
                {
                    case '=':
                        MoveNext();
                        return new Token(TokenType.NotEqual, "!=", position);
                    default:
                        return new Token(TokenType.Not, "!", position);
                }
            case '=':
                MoveNext();
                switch (Peek().Char)
                {
                    case '>':
                        MoveNext();
                        return new Token(TokenType.Arrow, "=>", position);
                    case '=':
                        MoveNext();
                        return new Token(TokenType.Equal, "==", position);
                    default:
                        return new Token(TokenType.Assign, "=", position);
                }
            case '+':
                MoveNext();
                switch (Peek().Char)
                {
                    case '+':
                        MoveNext();
                        return new Token(TokenType.Increment, "++", position);
                    case '=':
                        MoveNext();
                        return new Token(TokenType.AddAssign, "+=", position);
                    default:
                        return new Token(TokenType.Add, "+", position);
                }
            case '-':
                MoveNext();
                switch (Peek().Char)
                {
                    case '-':
                        MoveNext();
                        return new Token(TokenType.Decrement, "--", position);
                    case '=':
                        MoveNext();
                        return new Token(TokenType.SubtractAssign, "-=", position);
                    default:
                        return new Token(TokenType.Subtract, "-", position);
                }
            case '*':
                MoveNext();
                switch (Peek().Char)
                {
                    case '=':
                        MoveNext();
                        return new Token(TokenType.MultiplyAssign, "*=", position);
                    default:
                        return new Token(TokenType.Multiply, "*", position);
                }
            case '/':
                MoveNext();
                switch (Peek().Char)
                {
                    case '=':
                        MoveNext();
                        return new Token(TokenType.DivideAssign, "/=", position);
                    default:
                        return new Token(TokenType.Divide, "/", position);
                }
            case '%':
                MoveNext();
                switch (Peek().Char)
                {
                    case '=':
                        MoveNext();
                        return new Token(TokenType.ModuloAssign, "%=", position);
                    default:
                        return new Token(TokenType.Modulo, "%", position);
                }
            case '&':
                MoveNext();
                switch (Peek().Char)
                {
                    case '&':
                        MoveNext();
                        return new Token(TokenType.And, "&&", position);
                    case '=':
                        MoveNext();
                        return new Token(TokenType.BitAndAssign, "&=", position);
                    default:
                        return new Token(TokenType.BitAnd, "&", position);
                }
            case '|':
                MoveNext();
                switch (Peek().Char)
                {
                    case '|':
                        MoveNext();
                        return new Token(TokenType.Or, "||", position);
                    case '=':
                        MoveNext();
                        return new Token(TokenType.BitOrAssign, "|=", position);
                    default:
                        return new Token(TokenType.BitOr, "|", position);
                }
            case '~':
                MoveNext();
                return new Token(TokenType.BitNot, "~", position);
            case '<':
                MoveNext();
                switch (Peek().Char)
                {
                    case '<':
                        MoveNext();
                        switch (Peek().Char)
                        {
                            case '=':
                                MoveNext();
                                return new Token(TokenType.LeftShiftAssign, "<<=", position);
                            default:
                                return new Token(TokenType.LeftShift, "<<", position);
                        }
                    case '=':
                        MoveNext();
                        return new Token(TokenType.LessOrEqual, "<=", position);
                    default:
                        return new Token(TokenType.Less, "<", position);
                }
            case '>':
                MoveNext();
                switch (Peek().Char)
                {
                    case '>':
                        MoveNext();
                        switch (Peek().Char)
                        {
                            case '=':
                                MoveNext();
                                return new Token(TokenType.RightShiftAssign, ">>=", position);
                            default:
                                return new Token(TokenType.RightShift, ">>", position);
                        }
                    case '=':
                        MoveNext();
                        return new Token(TokenType.GreaterOrEqual, ">=", position);
                    default:
                        return new Token(TokenType.Greater, ">", position);
                }
            case '^':
                MoveNext();
                switch (Peek().Char)
                {
                    case '=':
                        MoveNext();
                        return new Token(TokenType.BitXorAssign, "^=", position);
                    default:
                        return new Token(TokenType.BitXor, "^", position);
                }
            case ';':
                MoveNext();
                return new Token(TokenType.Semicolon, ";", position);
            case ',':
                MoveNext();
                return new Token(TokenType.Comma, ",", position);
            case '(':
                MoveNext();
                return new Token(TokenType.OpenParen, "(", position);
            case ')':
                MoveNext();
                return new Token(TokenType.CloseParen, ')', position);
            case '[':
                MoveNext();
                return new Token(TokenType.OpenSquare, '[', position);
            case ']':
                MoveNext();
                return new Token(TokenType.CloseSquare, ']', position);
            case '{':
                MoveNext();
                return new Token(TokenType.OpenBrace, '{', position);
            case '}':
                MoveNext();
                return new Token(TokenType.CloseBrace, '}', position);
            case '.':
                MoveNext();
                return new Token(TokenType.Dot, ".", position);
            case ':':
                MoveNext();
                return new Token(TokenType.Colon, ":", position);
            default:
                MoveNext();
                return new Token(TokenType.Illegal, "Illegal", position);
        }
    }

    private void MoveNext(int count = 1, int line = 0)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        if (line < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(line));
        }

        if (line != 0)
        {
            _cell = 0;
        }
        else
        {
            _cell += count;
        }

        _line += line;
        _index += count;
    }

    private SourceChar Peek()
    {
        var chars = Peek(1);
        if (chars.IsEmpty)
        {
            return new SourceChar(char.MaxValue, true);
        }

        return new SourceChar(chars[0], false);
    }

    private ReadOnlySpan<char> Peek(int count)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        if (_index >= _source.Length)
        {
            return ReadOnlySpan<char>.Empty;
        }

        if (_index + count >= _source.Length)
        {
            count = _source.Length - _index;
        }

        return _source.Slice(_index, count).Span;
    }

    private bool IsNumber()
    {
        var chars = Peek(2);
        if (chars.IsEmpty)
        {
            return false;
        }

        if (chars[0].Between('0', '9'))
        {
            return true;
        }

        return chars.Length == 2 && chars[0] == '.' && chars[1].Between('0', '9');
    }

    private bool IsString()
    {
        return Peek().Is(ch => ch == '\'' || ch == '"');
    }

    private bool IsIdentifer()
    {
        return Peek().Is(ch => IsIdentifer(ch));
    }

    private static bool IsIdentifer(char ch)
    {
        return ch == '_' || char.IsLetter(ch);
    }

    private static bool IsIdentiferPart(char ch)
    {
        return ch == '_' || char.IsLetter(ch) || ch.Between('0', '9');
    }

    private static Token LookupKeyword(Token token)
    {
        switch (token.Value.ToString())
        {
            case "if":
                return new Token(TokenType.If, token.Value, token.Position);
            case "else":
                return new Token(TokenType.Else, token.Value, token.Position);
            case "let":
                return new Token(TokenType.Let, token.Value, token.Position);
            case "for":
                return new Token(TokenType.For, token.Value, token.Position);
            case "class":
                return new Token(TokenType.Class, token.Value, token.Position);
            case "import":
                return new Token(TokenType.Import, token.Value, token.Position);
            case "switch":
                return new Token(TokenType.Switch, token.Value, token.Position);
            case "case":
                return new Token(TokenType.Case, token.Value, token.Position);
            case "and":
                return new Token(TokenType.And, "&&", token.Position);
            case "or":
                return new Token(TokenType.Or, "||", token.Position);
            case "extends":
                return new Token(TokenType.Extends, token.Value, token.Position);
            case "native":
                return new Token(TokenType.Native, token.Value, token.Position);
            case "from":
                return new Token(TokenType.From, token.Value, token.Position);
            case "fn":
                return new Token(TokenType.Func, token.Value, token.Position);
            case "continue":
                return new Token(TokenType.Continue, token.Value, token.Position);
            case "break":
                return new Token(TokenType.Break, token.Value, token.Position);
            case "return":
                return new Token(TokenType.Return, token.Value, token.Position);
            case "default":
                return new Token(TokenType.Default, token.Value, token.Position);
            case "new":
                return new Token(TokenType.New, token.Value, token.Position);
            case "null":
                return new Token(TokenType.Null, token.Value, token.Position);
            case "super":
                return new Token(TokenType.Super, token.Value, token.Position);
            case "this":
                return new Token(TokenType.This, token.Value, token.Position);
            case "true":
                return new Token(TokenType.True, token.Value, token.Position);
            case "false":
                return new Token(TokenType.False, token.Value, token.Position);
            case "try":
                return new Token(TokenType.Try, token.Value, token.Position);
            case "catch":
                return new Token(TokenType.Catch, token.Value, token.Position);
            case "finally":
                return new Token(TokenType.Finally, token.Value, token.Position);
                case "throw":
                return new Token(TokenType.Throw,token.Value,token.Position);
            default:
                return token;
        }
    }

    private struct SourceChar
    {
        public char Char;

        public bool IsEmpty;

        public SourceChar(char ch, bool isEmpty)
        {
            Char = ch;
            IsEmpty = isEmpty;
        }

        public bool Is(char ch)
        {
            if (IsEmpty)
            {
                return false;
            }

            return ch == Char;
        }

        public bool Is(Func<char, bool> test)
        {
            if (IsEmpty)
            {
                return false;
            }

            return test(Char);
        }
    }
}
