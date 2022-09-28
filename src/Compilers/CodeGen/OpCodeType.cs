namespace Pain.Compilers.CodeGen;
public enum OpCodeType : byte
{
    Add,

    Mod,

    Mul,

    Neg,

    Sub,

    Div,

    Shl,

    Shr,

    Xor,

    Or,

    And,

    Not,

    Gt,

    Gte,

    Eq,

    New,

    Nop,

    Pop,

    Pop2,

    Pop3,

    Pop4,

    Popn,

    Push,

    Ldloc,

    Ldnull,

    Ldundf,

    Ldfld,

    Ldstr,

    Ldnum,

    Ldtoken,

    Stloc,

    Stobj,

    Stfld,

    Ret,

    Brfalse,

    Brtrue,

    Br,

    Call,

    Dup,

    Swap1_2,

    Box,

    Unbox
}