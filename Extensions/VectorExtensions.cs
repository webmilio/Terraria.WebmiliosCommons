using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace WebmilioCommons.Extensions;

public static class VectorExtensions
{
    public static Vector2 VelocityVectorToMouse(this Vector2 start, float speed = 1.0f) => VelocityVector(start, Main.MouseWorld, speed);

    public static Vector2 VelocityVectorToMouse(this Vector2 start, float speed, Vector2 endOffset) => VelocityVector(start, Main.MouseWorld, speed, endOffset);

    public static Vector2 VelocityVector(this Vector2 start, Vector2 end, float speed = 1.0f) => (end - start).SafeNormalize(Vector2.UnitY) * speed;

    public static Vector2 VelocityVector(this Vector2 start, Vector2 end, float speed, Vector2 endOffset) => VelocityVector(start, end - endOffset, speed);


    public static Vector2 PerpendicularClockwise(this Vector2 v) => new Vector2(-v.Y, v.X);
    public static Vector2 PerpendicularCounterClockwise(this Vector2 v) => new Vector2(v.Y, -v.X);

    public static float TriangleArea(this IList<Vector2> points)
    {
        if (points.Count != 3)
            throw new ArgumentException("The provided IList must contain 3 elements.");

        return
            (points[0].X * (points[1].Y - points[2].Y) +
             points[1].X * (points[2].Y - points[0].Y) +
             points[2].X * (points[0].Y - points[1].Y)) / 2;
    }

    public static bool IsInTriangle(this Vector2 position, IList<Vector2> points)
    {
        float
            triangleArea = points.TriangleArea(),
            a1 = new[] { position, points[1], points[2] }.TriangleArea(),
            a2 = new[] { points[0], position, points[2] }.TriangleArea(),
            a3 = new[] { points[0], points[1], position }.TriangleArea();

        return Math.Abs(triangleArea - (a1 + a2 + a3)) < 0.1f;
    }

    public static Vector2 ToTileDrawPosition(this Vector2 position, int xOffset = 12, int yOffset = 12)
    {
        if (xOffset == 0 && yOffset == 0)
            return position * 16 - Main.screenPosition;

        return (new Vector2(position.X + xOffset, position.Y + yOffset) * 16) - Main.screenPosition;
    }
}