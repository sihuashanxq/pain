namespace Pain.Compilers.Expressions;

public enum SyntaxType
{
    ConstString = 1,
    ConstNumber,
    ConstBoolean,
    ConstNull,
    ConstUndef,
    New,
    Call,
    This,
    Super,
    Member,
    If,
    For,
    Return,
    Break,
    Block,
    Continue,
    Let,
    Class,
    Function,
    Import,
    Module,
    Parameter,
    // =
    Assign,
    // +
    Add,
    // -
    Subtract,
    // *
    Multiply,
    // /
    Divide,
    // %
    Modulo,
    // <<
    BitLeftShift,
    // >>
    BitRightShift,
    // &
    BitAnd,
    // |
    BitOr,
    // ^
    BitXor,
    // ~
    BitNot,
    Not,
    EqualTo,
    NotEqualTo,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    And,
    Or,
    Is,
    Name,
    Switch,
    Empty,

    ArrayInit,

    MemberInit,

    Throw,

    Try,

    Catch,

    Finally
}