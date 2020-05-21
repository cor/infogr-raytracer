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
            float a = Vector2.Dot(ray.Direction, ray.Direction);
            var delta = ray.Origin - Position;
            float b = Vector2.Dot(ray.Direction, delta);
            float c = Vector2.Dot(delta, delta) - Radius * Radius;
            float d = b * b - 4 * a * c;
            if (d < 0) return false;
            
            float sqrt = (float) Math.Sqrt(d);
            float distance = (-b - sqrt) / a;
            if (distance < 0) distance = (-b + sqrt) / a;

            return (distance > 0 && distance < ray.End().Length);

            // // var minLength = (Position - ray.Origin).Length - Radius;
            // // if (ray.T < minLength) return false;
            //
            // // var length = end - ray.Origin;
            // // var length = ray.Origin - Position;
            //
            // var length = ray.Direction * ray.T;
            //
            // var po = ray.Origin - Position;
            // // Console.WriteLine($"po: {po}, length {length}");
            // if (Vector2.Dot(po, length) > 0) return false;
            // var a = Vector2.Dot(length, length);
            // var b = Vector2.Dot(length, po) * 2;
            // var c = Vector2.Dot(po, po) - Radius * Radius;
            //
            // float d = b * b - 4 * a * c;
            //
            // // var t = 2 * c / (-b + (float) Math.Sqrt(b * b - 4 * a * c));
            // // var xy = length * t;
            // // Console.WriteLine($"xy: {xy}");
            // return d >= 0f;
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