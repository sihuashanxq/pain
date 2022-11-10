using Pain.Runtime.VM;
namespace Pain.Runtime.Types;

public class Console : IObject
{
    [Function("log")]
    public static IObject Log(IObject[] args, out bool @throw)
    {
        @throw = false;
        if (args == null || args.Length < 2)
        {
            System.Console.WriteLine();
        }
        else
        {
            System.Console.WriteLine(args[1]?.ToString());
        }

        return Null.Value;
    }

    public bool ToBoolean(VirtualMachine vm)
    {
        return true;
    }

    public Type GetType(VirtualMachine vm, out bool @throw)
    {
        @throw = false;
        return Builtin.ConsoleType;
    }

    public void SetField(VirtualMachine vm, IObject key, IObject value, out bool @throw)
    {
        @throw = false;
        throw new NotImplementedException();
    }
}