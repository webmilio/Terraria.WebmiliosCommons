using System;
using Microsoft.Xna.Framework;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.Helpers
{
    public static class VectorHelpers
    {
        /// <summary>Creates a point on a radius in a given direction. The point will always be on the circle, wether or not <paramref name="b">b</paramref> is inside or outside the radius.</summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="radius"></param>
        /// <param name="offsetRadius"></param>
        /// <returns></returns>
        public static Vector2 OnCircle(Vector2 a, Vector2 b, float radius, float offsetRadius = 0)
        {
            return OnCircle(a, b, radius, out _, offsetRadius);
        }

        public static Vector2 OnCircle(Vector2 a, Vector2 b, float radius, out Vector2 unitV, float offsetRadius = 0)
        {
            var displacement = b - a;
            var dist = Vector2.Distance(a, b);

            unitV = Vector2.Divide(displacement, dist) * radius;
            var pos = a + unitV;

            if (offsetRadius != 0)
                pos += unitV * offsetRadius;

            return pos;
        }


        public static float TriangleArea(Vector2 a, Vector2 b, Vector2 c)
        {
            return Math.Abs((b.X * a.Y - a.X * b.Y) + (c.X * b.Y - b.X * c.Y) + (a.X * c.Y - c.X * a.Y)) / 2;
        }


        // Taken from StackOverflow, forgot from where...
        public static bool PointInTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
        {
            // Compute vectors        
            Vector2 ca = c - a;
            Vector2 ba = b - a;
            Vector2 pa = p - a;

            // Compute dot products
            float dotCACA = Vector2.Dot(ca, ca);
            float dotCABA = Vector2.Dot(ca, ba);
            float dotCAPA = Vector2.Dot(ca, pa);
            float dotBABA = Vector2.Dot(ba, ba);
            float dotBAPA = Vector2.Dot(ba, pa);

            // Compute barycentric coordinates
            float inverseDominator = 1 / (dotCACA * dotBABA - dotCABA * dotCABA);
            float u = (dotBABA * dotCAPA - dotCABA * dotBAPA) * inverseDominator;
            float v = (dotCACA * dotBAPA - dotCABA * dotCAPA) * inverseDominator;

            // Check if point is in triangle
            return u >= 0 && v >= 0 && (u + v) < 1;
        }

        public static bool PointInRectangle(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 p)
        {
            return PointInTriangle(a, b, c, p) || PointInTriangle(a, c, d, p);
        }


        public static Vector2[] MakeRectanglePointsOnCircle(Vector2 start, Vector2 end, float circleRadius, float width, float length, float circleOffsetRadius = 0)
        {
            var pointCenter = VectorHelpers.OnCircle(start, end, circleRadius, out var unitV, circleOffsetRadius);

            var a = pointCenter + unitV.PerpendicularCounterClockwise() * width / 2;
            var b = a + unitV * length;
            var d = pointCenter + unitV.PerpendicularClockwise() * width / 2;
            var c = d + unitV * length;

            return new[] { a, b, c, d };
        }
    }
}