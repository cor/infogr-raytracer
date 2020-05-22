using OpenTK;

namespace infogr_raytracer
{
    public struct Rectangle: IGameObject
    {
        // Clockwise order, starting with top-left corner
        private Vector2[] Points;

        public Rectangle(Vector2 position, float width, float height)
        {
            Points = new[]
            {
                new Vector2(position.X - width * 0.5f, position.Y + height * 0.5f),
                new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f),
                new Vector2(position.X + width * 0.5f, position.Y - height * 0.5f),
                new Vector2(position.X - width * 0.5f, position.Y - height * 0.5f)
            };
        }

        public Vector2 Position
        {
            get => (Points[0] + Points[2]) / 2;
        }
        
        public void Move(Vector2 position)
        {
        }

        /// <summary>
        /// Return if this rectangle intersects a ray.
        /// </summary>
        /// <param name="ray">The ray for which the intersection should be checked</param>
        /// <returns>If the rectangle intersects the ray.</returns>
        public bool Intersects(Ray ray)
        {
            return _lineIntersects(ray, Points[0], Points[1]) ||
                   _lineIntersects(ray, Points[1], Points[2]) ||
                   _lineIntersects(ray, Points[2], Points[3]) ||
                   _lineIntersects(ray, Points[3], Points[0]);
        }

            
        /// <summary>
        /// Determine if our ray crosses the line from line start to line end.
        /// </summary>
        /// <param name="ray">The ray that needs to be checked.</param>
        /// <param name="lineStart">The start of the line.</param>
        /// <param name="lineEnd">The end of the line.</param>
        /// <returns></returns>
        private bool _lineIntersects(Ray ray, Vector2 lineStart, Vector2 lineEnd)
        {
            // Uses this https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
            // algorithm of finding out if two vectors intersect.
            
            Vector2 rayStart = ray.Origin;
            Vector2 rayEnd = ray.Origin + ray.Direction;

            Vector2 rayStoLineS = lineStart - rayStart;
            Vector2 r = ray.Direction * ray.Magnitude;
            Vector2 s = lineEnd - lineStart;

            float crossR = rayStoLineS.X * r.Y - rayStoLineS.Y * r.X;
            float crossS = rayStoLineS.X * s.Y - rayStoLineS.Y * s.X;
            float rCrossS = r.X * s.Y - r.Y * s.X;

            if (crossR == 0f) // Lines are collinear
                return ((lineStart.X - rayStart.X < 0f) != (lineStart.X - rayEnd.X < 0f)) || 
                       ((lineStart.Y - rayStart.Y < 0f) != (lineStart.Y - rayEnd.Y < 0f));

            if (rCrossS == 0f) // Lines are parallel
                return false;

            float t = crossS / rCrossS;
            float u = crossR / rCrossS;
            
            return (t >= 0f) && (t <= 1f) && (u >= 0f) && (u <= 1f);
        }
    }
}