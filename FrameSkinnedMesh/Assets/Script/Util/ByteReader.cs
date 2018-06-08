#region Copyright © 2016-2018 RenGuiYou. All rights reserved.
//=====================================================
// Filename:    ByteReader.cs
// Author:      RenGuiyou
// Feedback: 	mailto:750539605@qq.com
//=====================================================
#endregion

using UnityEngine;

public class ByteReader
{
    private byte[] data;
    private int m_Length;
    private int index;
    public int Length
    {
        get { return m_Length; }
    }

    public ByteReader(byte[] bytes)
    {
        data = bytes;
        m_Length = bytes.Length;
        index = 0;
    }

    public byte ReadByte()
    {
        byte result = data[index];
        index++;
        return result;
    }

    public int ReadShort()
    {
        int result = (short) (((data[index + 1] & 0x00FF) << 8) | (0x00FF & data[index]));
        index += 2;
        return result;
    }

    public int ReadInt()
    {
        int result = (int)data[index + 3] << 24 | (int)data[index + 2] << 16 | (int)data[index + 1] << 8 | (int)data[index];
        index += 4;
        return result;
    }

    public byte[] ReadBytes(int num)
    {
        byte[] result = new byte[num];
        for (int i = 0; i < num; i++)
        {
            result[i] = data[index];
            index++;
        }
        return result;
    }

    public void ReadNext(int num = 1)
    {
        index += num;
    }
}
