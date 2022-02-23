using Microsoft.Xna.Framework;

namespace WebmilioCommons.Helpers;

public static class MathHelpers
{
    public static float ToNormalRadians(float angle)
    {
        if (angle > 0)
            angle = 2 * MathHelper.Pi - angle;

        if (angle < 0)
            angle = angle * -1;

        return angle;
    }

    public static void ToNormalRadians(ref float angle)
    {
        if (angle > 0)
            angle = 2 * MathHelper.Pi - angle;

        if (angle < 0)
            angle = angle * -1;
    }
}