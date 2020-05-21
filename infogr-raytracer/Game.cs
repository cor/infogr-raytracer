using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;

namespace infogr_raytracer
{
    public class Game
    {
        public Surface Screen;
        
        private Scene _scene = new Scene()
        {
            Lights = new List<Light>()
            {
                new Light() { Color = new Vector3(1, 1, 1), Position = new Vector2(2f, 2f) },
                new Light() { Color = new Vector3(3, 2, 1), Position = new Vector2(3f, 4f) },
                new Light() { Color = new Vector3(3, 4, 5), Position = new Vector2(3f, 8f) },
                new Light() { Color = new Vector3(1, 0, 0), Position = new Vector2(7f, 8f) },
                new Light() { Color = new Vector3(0, 0, 1), Position = new Vector2(7.5f, 8f) }
            },
            GameObjects = new List<IGameObject>()
            {
                new Circle(),
                new Square()
            }
        };
        
        private Camera _camera = new Camera()
        {
            Position = new Vector2(0, 0)
        };

        /// <summary>
        /// Called when the Game is loaded.
        /// Should be used for one-time setup.
        /// </summary>
        public void OnLoad()
        {
            Ray aRay = new Ray()
            {
                Origin = new Vector2(0, 1), 
                Direction = new Vector2(1, 0)
            };

            Circle aCircle = new Circle()
            {
                Position = new Vector2(5, 1),
                Radius = 2f
            };

            if (aCircle.Intersects(aRay))
                aCircle.Trace(aRay);
            
            Ray bRay = new Ray()
            {
                Origin = new Vector2(0, 1), 
                Direction = new Vector2(1, 0)
            };

            Circle bCircle = new Circle()
            {
                Position = new Vector2(5, 10),
                Radius = 2f
            };

            if (bCircle.Intersects(bRay))
                bCircle.Trace(bRay);


            Ray cRay = new Ray()
            {
                Origin = new Vector2(0, 1), 
                Direction = new Vector2(1, 0)
            };

            Circle cCircle = new Circle()
            {
                Position = new Vector2(2, 2f),
                Radius = 1f
            };

            if (cCircle.Intersects(cRay))
                cCircle.Trace(cRay);

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
                    Vector3 colorForPixel = Trace(_camera.WorldSpace(x, y));
                    Screen.Plot(x, y, ToScreenColor(colorForPixel));
                }
            }
            Screen.Print("Look on my Works, ye Mighty, and despair!", 10, 10, 0xFFFFFF);
        }


        /// <summary>
        /// On Window resize call the camera's resize method.
        /// </summary>
        public void OnResize()
        {
           _camera.Resize(Screen.Width, Screen.Height);
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