namespace Pain.Runtime;
using Pain.Runtime.Builtin;
using Pain.Runtime.VM;

public interface IObject
{
    public bool ToBoolean(VirtualMachine vm);

    public RuntimeClass GetClass(VirtualMachine vm);

    public void SetField(VirtualMachine vm, IObject key, IObject value);

    public IObject GetField(VirtualMachine vm, IObject key)
    {
        return GetClass(vm).GetFunction(this, key) as IObject ?? RuntimeNull.Null;
    }

    public void Constructor(VirtualMachine vm, IObject[] arguments)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.ConstructorFunc));
        if (function == null)
        {
            throw new Exception();
        }

        function.Call(vm, new IObject[0]);
    }

    public IObject ToString(VirtualMachine vm)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.ToStringFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new IObject[0]);
    }

    public IObject Euqal(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.EqualFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject LessThan(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.LessThanFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject GreaterThan(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.GreaterThanFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject LessThanOrEqual(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.LessThanOrEqualFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject GtreaterThanOrEqual(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.GtreaterThanOrEqualFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject LeftShfit(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.LeftShiftFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject RightShfit(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.RightShiftFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject Xor(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.XOrFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject Or(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.OrFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject Not(VirtualMachine vm)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.NotFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new IObject[0]);
    }

    public IObject And(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.AndFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject Add(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.AddFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject Sub(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.SubFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject Mul(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.MulFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject Mod(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.ModFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject Div(VirtualMachine vm, IObject obj)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.DivFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return function.Call(vm, new[] { obj });
    }

    public IObject Call(VirtualMachine vm, IObject[] arguments)
    {
        var function = GetClass(vm).GetFunction(this, new RuntimeString(Const.CallFunc));
        if (function == null)
        {
            throw new Exception();
        }

        return vm.Execute(function!, new[] { this }.Concat(arguments).ToArray()) as IObject ?? RuntimeNull.Null;
    }
}

public class RuntimeObject : IObject
{
    public RuntimeClass Class { get; }

    public Dictionary<IObject, IObject> Fields { get; }

    public RuntimeObject(RuntimeClass @class)
    {
        Class = @class;
        Fields = new Dictionary<IObject, IObject>();
    }

    public virtual RuntimeClass GetClass(VirtualMachine vm)
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

        return GetClass(vm).GetFunction(this, key) as IObject ?? RuntimeNull.Null;
    }

    public void SetField(VirtualMachine vm, IObject key, IObject value)
    {
        Fields[key] = value;
    }
}

public class RuntimeBoolean : IObject
{
    public bool Value { get; internal set; }

    public RuntimeBoolean(bool value)
    {
        Value = value;
    }

    public RuntimeClass GetClass(VirtualMachine vm)
    {
        return Builtin.Boolean.Class;
    }

    public bool ToBoolean(VirtualMachine vm)
    {
        return Value;
    }

    public void SetField(VirtualMachine vm, IObject key, IObject value)
    {
        throw new NotImplementedException();
    }
}

public class RuntimeNumber : IObject
{
    public double Value { get; internal set; }

    public RuntimeNumber(double value)
    {
        Value = value;
    }

    public bool ToBoolean(VirtualMachine vm)
    {
        return Value != 0;
    }

    public RuntimeClass GetClass(VirtualMachine vm)
    {
        return Builtin.Number.Class;
    }

    public void SetField(VirtualMachine vm, IObject key, IObject value)
    {
        throw new Exception();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

public class RuntimeArray : IObject
{
    public IObject[] Items;

    public RuntimeArray()
    {
        Items = new IObject[0];
    }

    public bool ToBoolean(VirtualMachine vm)
    {
        return true;
    }

    public RuntimeClass GetClass(VirtualMachine vm)
    {
        return Builtin.Array.Class;
    }

    public IObject GetField(VirtualMachine vm, IObject index)
    {
        if (index is RuntimeString str)
        {
            return GetClass(vm).GetFunction(this, index)!;
        }

        if (index is RuntimeNumber idx)
        {
            var i = idx.Value;
            if (i < 0 || i >= Items.Length)
            {
                return RuntimeNull.Null;
            }

            return Items[(int)i] ?? RuntimeNull.Null;
        }

        return RuntimeNull.Null;
    }

    public void SetField(VirtualMachine vm, IObject index, IObject value)
    {
        if (index is RuntimeNumber idx)
        {
            var i = (int)idx.Value;
            if (i < 0 || i >= Items.Length)
            {
                var items = new IObject[(int)i + 1];
                System.Array.Copy(Items, items, Items.Length);
                Items = items;
            }

            Items[i] = value;
        }
    }
}

public class RuntimeNull : IObject
{
    public static readonly IObject Null = new RuntimeNull();

    public RuntimeNull()
    {
    }

    public RuntimeClass GetClass(VirtualMachine vm)
    {
        return Builtin.Null.Class;
    }

    public IObject GetField(VirtualMachine vm, IObject key)
    {
        throw new NotImplementedException();
    }

    public void SetField(VirtualMachine vm, IObject key, IObject value)
    {
        throw new NotImplementedException();
    }

    public bool ToBoolean(VirtualMachine vm)
    {
        return false;
    }

    public override string ToString()
    {
        return "null";
    }
}

public class RuntimeFunction : IObject
{
    public IObject Target { get; }

    public Function Function { get; }

    public RuntimeFunction(IObject target, Function function)
    {
        Target = target;
        Function = function;
    }

    public IObject Call(VirtualMachine vm, IObject[] arguments)
    {
        return vm.Execute(this, new[] { Target }.Concat(arguments).ToArray())!;
    }

    public bool ToBoolean(VirtualMachine vm)
    {
        return true;
    }

    public RuntimeClass GetClass(VirtualMachine vm)
    {
        return Builtin.FunctionBuiltin.Class;
    }

    public void SetField(VirtualMachine vm, IObject key, IObject value)
    {
        throw new NotImplementedException();
    }
}

public class RuntimeClass : IObject
{
    public string Name { get; }

    public string Module { get; }

    public string Token { get; }

    public RuntimeClass? Super { get; }

    public FunctionTable FunctionTable { get; }

    public RuntimeClass(RuntimeClass? super, string name, string module, FunctionTable functionTable)
    {
        Name = name;
        Super = super;
        Token = $"{module}.{name}";
        Module = module;
        FunctionTable = functionTable;
    }

    public IObject GetField(IObject name)
    {
        return GetFunction(RuntimeNull.Null, name) as IObject ?? RuntimeNull.Null;
    }

    public RuntimeFunction? GetFunction(IObject target, IObject name)
    {
        if (FunctionTable.TryGetFunction(name.ToString()!, out var function))
        {
            return new RuntimeFunction(target, function!);
        }

        return Super?.GetFunction(target, name);
    }

    public virtual IObject CreateInstance()
    {
        return new RuntimeObject(this);
    }

    public bool ToBoolean(VirtualMachine vm)
    {
        return true;
    }

    public virtual RuntimeClass GetClass(VirtualMachine vm)
    {
        return this;
    }

    public void SetField(VirtualMachine vm, IObject key, IObject value)
    {
        throw new NotImplementedException();
    }
}

public class RuntimeBooleanClass : RuntimeClass
{
    public RuntimeBooleanClass(string name, string module, FunctionTable functionTable) : base(Builtin.Object.Class, name, module, functionTable)
    {

    }

    public override IObject CreateInstance()
    {
        return new RuntimeBoolean(false);
    }
}

public class RuntimeNullClass : RuntimeClass
{
    public RuntimeNullClass(string name, string module, FunctionTable functionTable) : base(Builtin.Object.Class, name, module, functionTable)
    {

    }

    public override IObject CreateInstance()
    {
        return new RuntimeBoolean(false);
    }
}

public class RuntimeStringClass : RuntimeClass
{
    public RuntimeStringClass(string name, string module, FunctionTable functionTable) : base(Builtin.Object.Class, name, module, functionTable)
    {

    }

    public override IObject CreateInstance()
    {
        return new RuntimeString(string.Empty);
    }
}

public class RuntimeNumberClass : RuntimeClass
{
    public RuntimeNumberClass(string name, string module, FunctionTable functionTable) : base(Builtin.Object.Class, name, module, functionTable)
    {

    }

    public override IObject CreateInstance()
    {
        return new RuntimeNumber(0d);
    }
}

public class RuntimeArrayClass : RuntimeClass
{
    public RuntimeArrayClass(string name, string module, FunctionTable functionTable) : base(Builtin.Object.Class, name, module, functionTable)
    {

    }

    public override IObject CreateInstance()
    {
        return new RuntimeArray();
    }
}

public class RuntimeString : IObject
{
    public string Value { get; internal set; }

    public RuntimeString(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }

    public bool ToBoolean(VirtualMachine vm)
    {
        return !string.IsNullOrEmpty(Value);
    }

    public RuntimeClass GetClass(VirtualMachine vm)
    {
        return Builtin.String.Class;
    }

    public void SetField(VirtualMachine vm, IObject key, IObject value)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is RuntimeString v)
        {
            return v.Value == Value;
        }

        return false;
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