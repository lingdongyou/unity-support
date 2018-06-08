
using System.Collections.Generic;

public class ByteWrite{
    readonly List<byte> list = new List<byte>();

    public void WriteByte(byte value)
    {
        list.Add(value);
    }

    public void WriteShort(short value)
    {
        list.Add((byte)value);
        list.Add((byte)(value >> 8));
    }

    public void WriteInt(int value)
    {
        list.Add((byte)value);
        list.Add((byte)(value >> 8));
        list.Add((byte)(value >> 16));
        list.Add((byte)(value >> 24));
    }

    public byte[] ToByteArray()
    {
        return list.ToArray();
    }

    public void Clear()
    {
        list.Clear();
    }
}
