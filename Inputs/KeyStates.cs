using System;

namespace WebmilioCommons.Inputs
{
    [Flags]
    public enum KeyStates : byte
    {
        NotPressed = 1 << 0,
        Pressed = 1 << 1,
        JustPressed = Pressed | 1 << 2,
        JustReleased = NotPressed | 1 << 4
    }
}