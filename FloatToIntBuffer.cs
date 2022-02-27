using System;

namespace WebmilioCommons;

public struct FloatToIntBuffer
{
    private readonly Action<int> _action;
    private float _value = 0;

    /// <summary></summary>
    /// <param name="action">Action to apply when Value >= 1.</param>
    public FloatToIntBuffer(Action<int> action)
    {
        _action = action;
    }

    public static implicit operator int(FloatToIntBuffer buffer) => (int) buffer.Value;
    public static implicit operator float(FloatToIntBuffer buffer) => buffer.Value;

    public float Value
    {
        get => _value;
        set
        {
            _value = value;

            if (_value >= 1)
            {
                int intgr = (int) _value;
                _value -= intgr;

                _action(intgr);
            }
        }
    }
}