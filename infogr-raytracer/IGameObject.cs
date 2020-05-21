using OpenTK;

namespace infogr_raytracer
{
    public interface IGameObject
    {
        void Move(Vector2 position);
        bool Intersects(Ray ray);
        Vector3 Trace(Ray ray);
    }
}