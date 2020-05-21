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
            float b = Vector2.Dot(ray.Direction, po) * 2;
            float c = Vector2.Dot(po, po) - (Radius * Radius);
            
            float d = b * b - 4 * a * c;
            
            Console.WriteLine("");
            Console.WriteLine($"a: {a}");
            Console.WriteLine($"b: {b}");
            Console.WriteLine($"c: {c}");
            Console.WriteLine($"d: {d}");
            
            return (int) d >= 0;
        }

        public Vector3 Trace(Ray ray)
        {
            throw new System.NotImplementedException();
        }
    }
}