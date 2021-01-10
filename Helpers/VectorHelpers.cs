using System;
using Microsoft.Xna.Framework;

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


        public static bool PointInTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
        {
            // Compute vectors        
            Vector2 v0 = c - a;
            Vector2 v1 = b - a;
            Vector2 v2 = p - a;

            // Compute dot products
            float dot00 = Vector2.Dot(v0, v0);
            float dot01 = Vector2.Dot(v0, v1);
            float dot02 = Vector2.Dot(v0, v2);
            float dot11 = Vector2.Dot(v1, v1);
            float dot12 = Vector2.Dot(v1, v2);

            // Compute barycentric coordinates
            float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            // Check if point is in triangle
            if (u >= 0 && v >= 0 && (u + v) < 1)
            { return true; }
            else { return false; }
        }

        public static bool PointInRectangle(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 p)
        {
            return PointInTriangle(a, b, c, p) || PointInTriangle(a, c, d, p);
        }
    }
}