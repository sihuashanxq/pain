using Pain.Runtime.VM;
namespace Pain.Runtime.Types
{
    public class Number : IObject
    {
        public double Value { get; internal set; }

        public Number(double value)
        {
            Value = value;
        }

        public bool ToBoolean(VirtualMachine vm)
        {
            return Value != 0;
        }

        public Type GetType(VirtualMachine vm)
        {
            return Builtin.NumberType;
        }

        public void SetField(VirtualMachine vm, IObject key, IObject value)
        {
            throw new Exception();
        }

        public override string ToString()
        {
            return Value.ToString();
        }


        public static bool IsNumber(IObject v2)
        {
            return v2 is Number;
        }

        [Function(Const.ToStringFunc)]
        public static IObject ToString(IObject[] args)
        {
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException();
            }

            return new String(((Number)args[0]).Value.ToString());
        }

        [Function(Const.EqualFunc)]
        public static IObject Euqal(IObject[] args)
        {
            if (args == null || args.Length != 2)
            {
                return Boolean.False;
            }

            if (args[0] is Number v1 && args[1] is Number v2)
            {
                return v1.Value == v2.Value ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }

        [Function(Const.LessThanFunc)]
        public static IObject LessThan(IObject[] args)
        {
            if (args == null || args.Length != 2)
            {
                return Boolean.False;
            }

            if (args[0] is Number v1 && args[1] is Number v2)
            {
                return v1.Value < v2.Value ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }

        [Function(Const.GreaterThanFunc)]
        public static IObject GreaterThan(IObject[] args)
        {
            if (args == null || args.Length != 2)
            {
                return Boolean.False;
            }

            if (args[0] is Number v1 && args[1] is Number v2)
            {
                return v1.Value > v2.Value ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }

        [Function(Const.LessThanOrEqualFunc)]
        public static IObject LessThanOrEqual(IObject[] args)
        {
            if (args == null || args.Length != 2)
            {
                return Boolean.False;
            }

            if (args[0] == args[1])
            {
                return Boolean.True;
            }

            if (args[0] is Number v1 && args[1] is Number v2)
            {
                return v1.Value <= v2.Value ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }

        [Function(Const.GtreaterThanOrEqualFunc)]
        public static IObject GtreaterThanOrEqual(IObject[] args)
        {
            if (args == null || args.Length != 2)
            {
                return Boolean.False;
            }

            if (args[0] == args[1])
            {
                return Boolean.True;
            }

            if (args[0] is Number v1 && args[1] is Number v2)
            {
                return v1.Value >= v2.Value ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }

        [Function(Const.LeftShiftFunc)]
        public static IObject LeftShfit(IObject[] args)
        {
            if (args == null || args.Length != 2)
            {
                return new Number(double.NaN);
            }

            if (args[0] == args[1])
            {
                return new Number(double.NaN);
            }

            if (args[1] is Number v2)
            {
                return new Number((Int64)(((Number)args[0]).Value) << (int)(v2.Value));
            }

            return new Number(double.NaN);
        }

        [Function(Const.RightShiftFunc)]
        public static IObject RightShift(IObject[] args)
        {
            if (args == null || args.Length != 2)
            {
                return new Number(double.NaN);
            }

            if (args[0] == args[1])
            {
                return new Number(double.NaN);
            }

            if (IsNumber(args[1]))
            {
                return new Number((Int64)(((Number)args[0]).Value) >> (int)((Number)args[1]).Value);
            }

            return new Number(double.NaN);
        }

        [Function(Const.XOrFunc)]
        public static IObject Xor(IObject[] arguments)
        {
            if (arguments == null || arguments.Length != 2)
            {
                return new Number(double.NaN);
            }

            if (arguments[0] == arguments[1])
            {
                return new Number(double.NaN);
            }

            if (IsNumber(arguments[1]))
            {
                return new Number((Int64)(((Number)arguments[0]).Value) ^ (Int64)((Number)arguments[1]).Value);
            }

            return new Number(double.NaN);
        }

        [Function(Const.OrFunc)]
        public static IObject Or(IObject[] arguments)
        {
            if (arguments == null || arguments.Length != 2)
            {
                return new Number(double.NaN);
            }

            if (arguments[0] == arguments[1])
            {
                return new Number(double.NaN);
            }

            if (IsNumber(arguments[1]))
            {
                return new Number((Int64)(((Number)arguments[0]).Value) | (Int64)((Number)arguments[1]).Value);
            }

            return new Number(double.NaN);
        }

        [Function(Const.NotFunc)]
        public static IObject Not(IObject[] arguments)
        {
            if (arguments == null || arguments.Length != 1)
            {
                return new Number(double.NaN);
            }

            return new Number(~(Int64)(((Number)arguments[0]).Value));
        }

        [Function(Const.AndFunc)]
        public static IObject And(IObject[] arguments)
        {
            if (arguments == null || arguments.Length != 2)
            {
                return new Number(double.NaN);
            }

            if (arguments[0] == arguments[1])
            {
                return new Number(double.NaN);
            }

            if (IsNumber(arguments[1]))
            {
                return new Number((Int64)(((Number)arguments[0]).Value) & (Int64)((Number)arguments[1]).Value);
            }

            return new Number(double.NaN);
        }

        [Function(Const.AddFunc)]
        public static IObject Add(IObject[] arguments)
        {
            if (arguments == null || arguments.Length != 2)
            {
                return new Number(double.NaN);
            }

            if (arguments[0] == arguments[1])
            {
                return new Number(double.NaN);
            }

            if (IsNumber(arguments[1]))
            {
                return new Number(((Number)arguments[0]).Value + ((Number)arguments[1]).Value);
            }

            return new Number(double.NaN);
        }
        public static IObject Add(IObject v1, IObject v2)
        {
            return new Number(((Number)v1).Value + ((Number)v2).Value);
        }

        [Function(Const.SubFunc)]
        public static IObject Sub(IObject[] arguments)
        {
            if (arguments == null || arguments.Length != 2)
            {
                return new Number(double.NaN);
            }

            if (arguments[0] == arguments[1])
            {
                return new Number(double.NaN);
            }

            if (IsNumber(arguments[1]))
            {
                return new Number(((Number)arguments[0]).Value - ((Number)arguments[1]).Value);
            }

            return new Number(double.NaN);
        }

        [Function(Const.MulFunc)]
        public static IObject Mul(IObject[] arguments)
        {
            if (arguments == null || arguments.Length != 2)
            {
                return new Number(double.NaN);
            }

            if (arguments[0] == arguments[1])
            {
                return new Number(double.NaN);
            }

            if (IsNumber(arguments[1]))
            {
                return new Number(((Number)arguments[0]).Value * ((Number)arguments[1]).Value);
            }

            return new Number(double.NaN);
        }

        [Function(Const.ModFunc)]
        public static IObject Mod(IObject[] arguments)
        {
            if (arguments == null || arguments.Length != 2)
            {
                return new Number(double.NaN);
            }

            if (arguments[0] == arguments[1])
            {
                return new Number(double.NaN);
            }

            if (IsNumber(arguments[1]))
            {
                return new Number(((Number)arguments[0]).Value % ((Number)arguments[1]).Value);
            }

            return new Number(double.NaN);
        }

        [Function(Const.DivFunc)]
        public static IObject Div(IObject[] arguments)
        {
            if (arguments == null || arguments.Length != 2)
            {
                return new Number(double.NaN);
            }

            if (arguments[0] == arguments[1])
            {
                return new Number(double.NaN);
            }

            if (IsNumber(arguments[1]))
            {
                return new Number(((Number)arguments[0]).Value / ((Number)arguments[1]).Value);
            }

            return new Number(double.NaN);
        }

        [Function(Const.CallFunc)]
        public static IObject Call(IObject[] arguments)
        {
            throw new Exception();
        }
    }
}