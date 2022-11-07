namespace Pain.Runtime;
using Pain.Runtime.VM;

public class RuntimeObject : IObject
{
    public RuntimeClass Class { get; }

    public Dictionary<IObject, IObject> Fields { get; }

    public RuntimeObject(RuntimeClass @class)
    {
        Class = @class;
        Fields = new Dictionary<IObject, IObject>();
    }

    public virtual RuntimeClass GetClass()
    {
        return Class;
    }

    public bool ToBoolean(VirtualMachine vm)
    {
        return true;
    }

    public IObject GetField(VirtualMachine vm, IObject key)
    {
        if (Fields.TryGetValue(key, out var value))
        {
            return value;
        }

        return GetClass().GetFunction(vm, this, key) as IObject ?? Null.Const;
    }

    public void SetField(VirtualMachine vm, IObject key, IObject value)
    {
        Fields[key] = value;
    }
}

public class RuntimeClass : IObject
{
    public string Name { get; }

    public string Module { get; }

    public string Token { get; }

    public string SuperToken { get; }

    public RuntimeClass? Super { get; private set; }

    public FunctionTable FunctionTable { get; }

    public RuntimeClass(string name, string superToken, string module, FunctionTable functionTable)
    {
        Name = name;
        Token = $"{module}.{name}";
        Module = module;
        SuperToken = superToken;
        FunctionTable = functionTable;
    }

    public Function? GetFunction(VirtualMachine vm, IObject target, IObject name)
    {
        if (FunctionTable.TryGetFunction(name.ToString()!, out var function))
        {
            return new Function(target, function!);
        }

        if (Super == null)
        {
            if (!string.IsNullOrEmpty(SuperToken))
            {
                Super = vm.GetClassLoader().Load(SuperToken);
            }
        }

        return Super?.GetFunction(vm, target, name);
    }

    public virtual IObject CreateInstance()
    {
        return new RuntimeObject(this);
    }

    public bool ToBoolean(VirtualMachine vm)
    {
        return true;
    }

    public virtual RuntimeClass GetClass()
    {
        return this;
    }

    public void SetField(VirtualMachine vm, IObject key, IObject value)
    {
        throw new NotImplementedException();
    }
}

public class BooleanType : RuntimeClass
{
    public BooleanType(string name, string module, FunctionTable functionTable) : base(name, Builtin.Object.Token, module, functionTable)
    {
        
    }

    public override IObject CreateInstance()
    {
        return new Boolean(false);
    }
}

public class NullClass : RuntimeClass
{
    public NullClass(string name, string module, FunctionTable functionTable) : base(name, Builtin.Object.Token, module, functionTable)
    {

    }

    public override IObject CreateInstance()
    {
        return new Boolean(false);
    }
}

public class RuntimeStringClass : RuntimeClass
{
    public RuntimeStringClass(string name, string module, FunctionTable functionTable) : base(name, Builtin.Object.Token, module, functionTable)
    {

    }

    public override IObject CreateInstance()
    {
        return new String(string.Empty);
    }
}

public class NumberClass : RuntimeClass
{
    public NumberClass(string name, string module, FunctionTable functionTable) : base(name, Builtin.Object.Token, module, functionTable)
    {

    }

    public override IObject CreateInstance()
    {
        return new Number(0d);
    }
}

public class ArrayType : RuntimeClass
{
    public ArrayType(string name, string module, FunctionTable functionTable) : base(name, Builtin.Object.Token, module, functionTable)
    {

    }

    public override IObject CreateInstance()
    {
        return new Array();
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class FunctionAttribute : Attribute
{
    public string Name { get; }

    public FunctionAttribute(string name)
    {
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class ClassAttribute : Attribute
{
    public string Name { get; }

    public string Module { get; }

    public ClassAttribute(string module, string name)
    {
        Name = name;
        Module = module;
    }
}