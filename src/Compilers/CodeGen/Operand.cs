using System.Runtime.CompilerServices;
namespace Pain.Compilers.CodeGen;

public abstract class Operand
{
    public abstract int Size { get; }

    public abstract void WriteTo(Stream stream);

    public abstract object GetValue();
}

public class Operand<TValue> : Operand where TValue : struct
{
    public override int Size => ValueSize;

    public virtual TValue Value { get; set;}

    protected virtual int ValueSize { get; }

    public Operand(TValue value, int valueSize)
    {
        Value = value;
        ValueSize = valueSize;
    }

    public override unsafe void WriteTo(Stream stream)
    {
        stream.Write(Value, ValueSize);
    }

    public override object GetValue()
    {
        return Value;
    }
}

  public static class StreamExtensions
    {
        public unsafe static void Write<TValue>(this Stream stream, TValue value, int valueSize) where TValue : struct
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (value is int i)
            {
                stream.Write(BitConverter.GetBytes(i));
                return;
            }

            stream.Write(new Span<byte>(Unsafe.AsPointer(ref value), valueSize));
        }
    }