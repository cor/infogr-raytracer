using OpenTK;

namespace infogr_raytracer
{
    public interface IGameObject
    {
        void Move(Vector2 position);
        bool Intersects(Ray ray);
        
        // TODO:
        // Vector3 Trace(Ray ray);
    }
}