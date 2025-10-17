using System;

namespace WebCom;

public struct FloatBuffer
{
    private readonly object _lock = new();
    public float value = 0;

    public FloatBuffer(float value)
    {
        this.value = value;
    }

    public int TakeInts()
    {
        int delta;

        lock (_lock)
        {
            delta = (int)value;
            value -= delta;
        }

        return delta;
    }

    public static FloatBuffer operator +(FloatBuffer a, FloatBuffer b)
    {
        a.value += b.value;
        return a;
    }

    public static FloatBuffer operator -(FloatBuffer a, FloatBuffer b)
    {
        a.value -= b.value;
        return a;
    }

    public static FloatBuffer operator /(FloatBuffer a, FloatBuffer b)
    {
        a.value /= b.value;
        return a;
    }

    public static FloatBuffer operator *(FloatBuffer a, FloatBuffer b)
    {
        a.value *= b.value;
        return a;
    }

    public static implicit operator int(FloatBuffer buffer) => (int) buffer.value;
    public static implicit operator float(FloatBuffer buffer) => buffer.value;
}