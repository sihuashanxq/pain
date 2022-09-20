
namespace Pain.Compilers.Parsers;

public enum TokenType
{
    Illegal,
    EOF,
    Identifier,
    Number,
    Native,
    LiteralString,
    Null,
    Undef,
    True,
    False,
    Let,
    Const,
    Is,
    Import,
    Module,
    From,
    Class,
    Static,
    Extends,
    This,
    Super,
    New,
    Func,
    Return,
    If,
    Else,
    For,
    In,
    Break,
    Switch,
    Case,
    Default,
    Semicolon,
    Comma,
    Colon,
    Dot,
    Assign,
    AddAssign,
    SubtractAssign,
    MultiplyAssign,
    DivideAssign,
    ModuloAssign,
    LeftShiftAssign,
    RightShiftAssign,
    BitOrAssign,
    BitAndAssign,
    BitXorAssign,
    OpenParen,
    CloseParen,
    OpenBrace,
    CloseBrace,
    OpenSquare,
    CloseSquare,
    Add,
    Subtract,
    Multiply,
    Divide,
    Modulo,
    Increment,
    Decrement,
    LeftShift,
    RightShift,
    BitAnd,
    BitOr,
    BitXor,
    BitNot,
    Not,
    Equal,
    NotEqual,
    Greater,
    GreaterOrEqual,
    Less,
    LessOrEqual,
    And,
    Or,
    Arrow,
    Continue,
    As
}

public static class TokenTypeExtensions
{
    public static bool IsAssign(this TokenType type)
    {
        return type == TokenType.Assign || type == TokenType.AddAssign ||
             type == TokenType.Assign || type == TokenType.AddAssign ||
             type == TokenType.SubtractAssign || type == TokenType.MultiplyAssign ||
             type == TokenType.DivideAssign || type == TokenType.ModuloAssign ||
             type == TokenType.LeftShiftAssign || type == TokenType.RightShiftAssign ||
             type == TokenType.BitOrAssign || type == TokenType.BitAndAssign ||
             type == TokenType.BitXorAssign;
    }
}