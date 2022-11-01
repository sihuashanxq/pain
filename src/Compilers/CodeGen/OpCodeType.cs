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

    Pop,

    Push,

    Ldloc,

    Ldarg,

    Ldtoken,

    Ldnull,

    Ldfld,

    Ldstr,

    Ldnum,

    Stloc,

    Stfld,

    Starg,

    Ret,

    Brfalse,

    Brtrue,

    Br,

    Call,

    Dup,

    Swap1_2
}