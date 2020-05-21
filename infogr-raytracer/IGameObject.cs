using OpenTK;

namespace infogr_raytracer
{
    public interface IGameObject
    {
        bool Intersects(Ray ray);
        Vector3 Trace(Ray ray);
    }
}