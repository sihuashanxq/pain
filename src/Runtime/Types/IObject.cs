using Pain.Runtime.VM;
namespace Pain.Runtime.Types
{
    public interface IObject
    {
        public bool ToBoolean(VirtualMachine vm);

        public Type GetType(VirtualMachine vm, out bool @throw);

        public void SetField(VirtualMachine vm, IObject key, IObject value, out bool @throw);

        public IObject GetField(VirtualMachine vm, IObject key, out bool @throw)
        {
            @throw = false;
            return GetType(vm, out @throw).GetFunction(vm, this, key) as IObject ?? Null.Value;
        }

        public IObject ToString(VirtualMachine vm, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.ToStringFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new IObject[0], out @throw);
        }

        public IObject Euqal(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.EqualFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject LessThan(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.LessThanFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject GreaterThan(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.GreaterThanFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject LessThanOrEqual(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.LessThanOrEqualFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject GtreaterThanOrEqual(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.GtreaterThanOrEqualFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject LeftShfit(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.LeftShiftFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject RightShfit(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.RightShiftFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject Xor(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.XOrFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject Or(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.OrFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject Not(VirtualMachine vm, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.NotFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new IObject[0], out @throw);
        }

        public IObject And(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.AndFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject Add(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.AddFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject Sub(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.SubFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject Mul(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.MulFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject Mod(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.ModFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject Div(VirtualMachine vm, IObject obj, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.DivFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return function.Call(vm, new[] { obj }, out @throw);
        }

        public IObject Call(VirtualMachine vm, IObject[] arguments, out bool @throw)
        {
            var function = GetType(vm, out @throw).GetFunction(vm, this, new String(Const.CallFunc));
            if (function == null)
            {
                throw new Exception();
            }

            return vm.Execute(function!, new[] { this }.Concat(arguments).ToArray(), out @throw) as IObject ?? Null.Value;
        }
    }
}