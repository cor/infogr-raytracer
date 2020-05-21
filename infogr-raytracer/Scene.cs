using System.Collections.Generic;
using OpenTK;

namespace infogr_raytracer
{
    public class Scene
    {
        public List<Light> Lights;
        public List<IGameObject> GameObjects;
    }
}