using System;

namespace WebmilioCommons.Helpers;

public static class PhysicsHelpers
{
    /// <summary>Calculates the initial velocity (v0) required to launch a projectile from point A to B.</summary>
    /// <param name="angle">Angle in degrees.</param>
    public static float PointABVelocity(float y0, float y1, float deltaX, float angle) =>
        PointABVelocity(y0, y1, deltaX, angle, Constants.GravityInMeters);

    public static float PointABVelocity(float y0, float y1, float deltaX, float angle, float gravity)
    {
        var raw = -gravity * Math.Pow(deltaX, 2) /
                  (2 * Math.Pow(Math.Cos(angle), 2) * (y0 - y1 - deltaX * Math.Tan(angle)));

        if (raw < 0)
            raw *= -1;

        return (float) Math.Sqrt(raw);
    }
}