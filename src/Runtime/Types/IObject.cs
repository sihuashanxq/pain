using Pain.Runtime.VM;
using Pain.Runtime.Builtin;
namespace Pain.Runtime
{
    public interface IObject
    {
        public bool ToBoolean(VirtualMachine vm);

        public RuntimeClass GetClass();

        public void SetField(VirtualMachine vm, IObject key, IObject value);

        public IObject GetField(VirtualMachine vm, IObject key)
        {
            return GetClass().GetFunction(vm, this, key) as IObject ?? Null.Const;
        }

        public IObject ToString(VirtualMachine vm)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.ToStringFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new IObject[0]);
        }

        public IObject Euqal(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.EqualFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject LessThan(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.LessThanFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject GreaterThan(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.GreaterThanFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject LessThanOrEqual(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.LessThanOrEqualFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject GtreaterThanOrEqual(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.GtreaterThanOrEqualFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject LeftShfit(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.LeftShiftFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject RightShfit(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.RightShiftFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject Xor(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.XOrFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject Or(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.OrFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject Not(VirtualMachine vm)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.NotFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new IObject[0]);
        }

        public IObject And(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.AndFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject Add(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.AddFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject Sub(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.SubFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject Mul(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.MulFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject Mod(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.ModFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject Div(VirtualMachine vm, IObject obj)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.DivFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj });
        }

        public IObject Call(VirtualMachine vm, IObject[] arguments)
        {
            var function = GetClass().GetFunction(vm, this, new String(Const.CallFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return vm.Execute(function!, new[] { this }.Concat(arguments).ToArray()) as IObject ?? Null.Const;
        }
    }
}