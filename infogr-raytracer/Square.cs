using OpenTK;

namespace infogr_raytracer
{
    public struct Square: IGameObject
    {
        public Vector2 Position;
        
        public void Move(Vector2 position)
        {
            Position = position;
        }

        public bool Intersects(Ray ray)
        {
            throw new System.NotImplementedException();
        }

        public Vector3 Trace(Ray ray)
        {
            throw new System.NotImplementedException();
        }
    }
}