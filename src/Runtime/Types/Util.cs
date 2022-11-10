using System.Reflection;
namespace Pain.Runtime.Types;

public static class Util
{
    public static FunctionTable ScanFunctions(System.Type type)
    {
        var table = new FunctionTable();

        foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
        {
            var name = method.GetCustomAttribute<FunctionAttribute>()?.Name;
            if (name == null)
            {
                continue;
            }

            var function = new CompiledFunction(name, true, null!, 0, method);
            table.Add(name, function);
        }

        return table;
    }
}
