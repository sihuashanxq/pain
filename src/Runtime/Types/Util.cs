using System.Reflection;
namespace Pain.Runtime;

public static class Util
{
    public static FunctionTable ScanFunctions(Type type)
    {
        var functionTable = new FunctionTable();

        foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
        {
            var name = method.GetCustomAttribute<FunctionAttribute>()?.Name;
            if (name == null)
            {
                continue;
            }

            var function = new CompiledFunction(name, true, null!, 0, method.GetParameters().Length, method);
            functionTable.AddFunction(name, function);
        }

        return functionTable;
    }
}
