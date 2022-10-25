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

    Push,

    Ldloc,

    Ldarg,

    Ldnull,

    Ldundf,

    Ldfld,

    Ldstr,

    Ldnum,

    Ldtoken,

    Stloc,

    Stfld,

    Ret,

    Brfalse,

    Brtrue,

    Br,

    Call,

    Dup,

    Swap1_2
}