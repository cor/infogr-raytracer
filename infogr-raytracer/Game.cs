using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;

namespace infogr_raytracer
{
    public class Game
    {
        public Surface Screen;

        private List<Light> _lights;
        private Vector2 _worldSpaceSize = new Vector2(10f, 10f);

        /// <summary>
        /// Called when the Game is loaded.
        /// Should be used for one-time setup.
        /// </summary>
        public void OnLoad()
        {
            _lights = new List<Light>()
            {
                new Light() { Color = new Vector3(1, 1, 1), Position = new Vector2(2f, 2f) },
                new Light() { Color = new Vector3(3, 2, 1), Position = new Vector2(3f, 4f) },
                new Light() { Color = new Vector3(3, 4, 5), Position = new Vector2(3f, 8f) },
                new Light() { Color = new Vector3(1, 0, 1), Position = new Vector2(7f, 8f) }
            };
        }

        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        public void OnRenderFrame()
        {
            // Clear the screen
            Screen.Clear(255);

            for (int x = 0; x < Screen.Width; x++)
            {
                for (int y = 0; y < Screen.Height; y++)
                {
                    Vector3 colorForPixel = Trace(new Vector2(WorldSpaceX(x), WorldSpaceY(y)));
                    Screen.Plot(x, y, ToScreenColor(colorForPixel));
                }
            }
            Screen.Print("hi", 10, 10, 0xFFFFFF);
        }


        public void OnResize()
        {
            float pixelsPerWorldSpaceUnit = 100;
            _worldSpaceSize = new Vector2((float) Screen.Width / pixelsPerWorldSpaceUnit, 
                                          (float) Screen.Height / pixelsPerWorldSpaceUnit);
        }

        private int ToScreenColor(Vector3 worldColor)
        {
            return ((int) (Math.Min(worldColor.X * 255, 255)) << 16) + 
                   ((int) (Math.Min(worldColor.Y * 255, 255)) << 8) +
                   (int) (Math.Min(worldColor.Z * 255, 255)); 
        }

        private Vector3 Trace(Vector2 point)
        {
            Vector3 colorAtPixel = new Vector3(0, 0, 0);
            
            foreach (Light light in _lights)
            {
                Vector2 pointToLight = light.Position - point;
                float r = pointToLight.Length;
                float intensityForLight = 1f / (4f * r * r);

                colorAtPixel += light.Color * intensityForLight;
            }
            
            return colorAtPixel;
        }

        private float WorldSpaceX(int screenSpaceX)
        {
            float ratio = ((float) _worldSpaceSize.X / (float) Screen.Width);
            return (float) screenSpaceX * ratio;
        }

        private float WorldSpaceY(int screenSpaceY)
        {
            float ratio = ((float) _worldSpaceSize.Y / (float) Screen.Height);
            return _worldSpaceSize.Y - screenSpaceY * ratio;
        }
    }
}