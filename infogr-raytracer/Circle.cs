using System;
using OpenTK;

namespace infogr_raytracer
{
    public struct Circle: IGameObject
    {
        public Vector2 Position;
        public float Radius;
        
        public void Move(Vector2 position)
        {
            Position = position;
        }

        public bool Intersects(Ray ray)
        {
            var end = ray.End();
            var xLength = end.X - ray.Origin.X;
            var yLength = (end.Y - ray.Origin.Y);
            var a =  xLength * xLength +
                     yLength * yLength;

            var xDiff = (ray.Origin.X - Position.X);
            var yDiff = (ray.Origin.Y - Position.Y);
            var b = 2 * xLength * xDiff + 2 * yLength * yDiff;
            var c = xDiff * xDiff + yDiff * yDiff - Radius * Radius;
            
            float d = b * b - 4 * a * c;
            return d >= 0f;
        }

        public Vector3 Trace(Ray ray)
        {
            Vector2 po = ray.Origin - Position;

            float a = Vector2.Dot(ray.Origin, ray.Origin);
            float b = Vector2.Dot(ray.Direction, po) * 2;
            float c = Vector2.Dot(po, po) - (Radius * Radius);
            
            float d = b * b - 4 * a * c;


            if (d > 0)
            {
                float dSqrt = (float)Math.Sqrt(d);
                float t1 = (-b + dSqrt) / (2 * a);
                float t2 = (-b - dSqrt) / (2 * a);

                Vector2 intersection1 = ray.Origin + ray.Direction * t1;
                Vector2 intersection2 = ray.Origin + ray.Direction * t2;
            }
            else // d = 0
            {
                float t = -b / (2 * a);
                Vector2 intersection = ray.Origin + ray.Direction * t;
            }
            
            
            // TODO: Return result
            return Vector3.One;
        }
    }
}