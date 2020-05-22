using OpenTK;

namespace infogr_raytracer
{
    public interface IGameObject
    {
        Vector2 Position { get; }
        void Move(Vector2 position);

        void MoveBy(Vector2 translation);

        bool Intersects(Ray ray);
        
        // TODO:
        // Vector3 Trace(Ray ray);
    }
}