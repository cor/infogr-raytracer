using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;

namespace infogr_raytracer
{
    public class Game
    {
        public Surface Screen;
        private Scene _scene;
        private Camera _camera = new Camera()
        {
            Position = new Vector2(1, 1), 
            ViewPort = new Vector2(10, 10)
        };

        /// <summary>
        /// Called when the Game is loaded.
        /// Should be used for one-time setup.
        /// </summary>
        public void OnLoad()
        {
            _scene = new Scene(this)
            {
                Lights = new List<Light>()
                {
                    new Light() { Color = new Vector3(1, 1, 1), Position = new Vector2(2f, 2f) },
                    new Light() { Color = new Vector3(3, 2, 1), Position = new Vector2(3f, 4f) },
                    new Light() { Color = new Vector3(3, 4, 5), Position = new Vector2(3f, 8f) },
                    new Light() { Color = new Vector3(1, 0, 0), Position = new Vector2(7f, 8f) },
                    new Light() { Color = new Vector3(0, 0, 1), Position = new Vector2(7.5f, 8f) }
                }
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
                    Vector3 colorForPixel = Trace(new Vector2(_camera.WorldSpaceX(x), _camera.WorldSpaceY(y)));
                    Screen.Plot(x, y, ToScreenColor(colorForPixel));
                }
            }
            Screen.Print("Look on my Works, ye Mighty, and despair!", 10, 10, 0xFFFFFF);
        }


        public void OnResize()
        {
            float pixelsPerWorldSpaceUnit = 100;
            _camera.ScreenResolution = new Vector2(Screen.Width, Screen.Height);
            _camera.ViewPort = new Vector2((float) Screen.Width / pixelsPerWorldSpaceUnit, 
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
            Vector3 colorAtPoint = new Vector3(0, 0, 0);
            
            foreach (Light light in _scene.Lights)
            {
                Vector2 pointToLight = light.Position - point;
                float distanceToLight = pointToLight.Length;
                float intensity = 1f / (4f * (float) Math.PI * distanceToLight * distanceToLight);

                colorAtPoint += light.Color * intensity;
            }
            
            return colorAtPoint;
        }
    }
}