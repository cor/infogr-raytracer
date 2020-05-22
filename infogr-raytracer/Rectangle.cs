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

        public bool Intersects(Ray ray)
        {
            return true;
        }
    }
}