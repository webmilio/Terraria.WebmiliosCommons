using Microsoft.Xna.Framework;

namespace WebmilioCommons.Helpers;

public static class ColorHelpers
{
    public static void Add(ref Color a, Color b)
    {
        a.R += b.R;
        a.G += b.G;
        a.B += b.B;
        a.A += b.A;
    }

    public static void Substract(ref Color a, Color b)
    {
        a.R -= b.R;
        a.G -= b.G;
        a.B -= b.B;
        a.A -= b.A;
    }
}