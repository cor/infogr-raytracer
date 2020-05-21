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
            Vector2 po = ray.Origin - Position;

            float a = Vector2.Dot(ray.Origin, ray.Origin);
            float b = 2 * Vector2.Dot(ray.Direction, po);
            float c = Vector2.Dot(po, po) - (Radius * Radius);
            
            float d = b * b - 4 * a * c;
            
            // Console.WriteLine("");
            // Console.WriteLine($"a: {a}");
            // Console.WriteLine($"b: {b}");
            // Console.WriteLine($"c: {c}");
            // Console.WriteLine($"d: {d}");
            
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