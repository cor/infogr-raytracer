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

        /// <summary>
        /// Return if this circle intersects a ray
        /// Uses the ABC formula
        /// </summary>
        /// <param name="ray">The ray for which the intersection should be checked</param>
        /// <returns>If the circle intersects the ray.</returns>
        public bool Intersects(Ray ray)
        {
            Vector2 positionToRayOrigin = ray.Origin - Position;
            
            // Apply ABC formula
            float a = Vector2.Dot(ray.Direction, ray.Direction);
            float b = Vector2.Dot(ray.Direction, positionToRayOrigin);
            float c = Vector2.Dot(positionToRayOrigin, positionToRayOrigin) - Radius * Radius;
            float d = b * b - a * c;
            
            // Early exit if there are no intersections
            if (d < 0) return false;
            
            // Calculate intersections based on d
            float sqrtD = (float) Math.Sqrt(d);
            float distance = Math.Max((-b - sqrtD) / a, (-b + sqrtD) / a);

            // Check if our result is within the ray's length
            return (distance > 0 && distance < ray.T);
        }
    }
}