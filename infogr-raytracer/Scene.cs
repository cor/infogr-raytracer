using System.Collections.Generic;
using OpenTK;

namespace infogr_raytracer
{
    public class Scene
    {
        private Game _game;
        public List<Light> Lights;
        private Vector2 _worldSpaceSize = new Vector2(10f, 10f);

        public Scene(Game game)
        {
            _game = game;
        }
    }
}