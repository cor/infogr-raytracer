using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

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
                new Circle() { Position = new Vector2(4.0f,4.5f), Radius = 0.1f },
                new Circle() { Position = new Vector2(6.5f,6.5f), Radius = 1.0f },
                new Circle() { Position = new Vector2(4.0f,4.0f), Radius = 0.3f },
                new Circle() { Position = new Vector2(3.0f,3.0f), Radius = 0.1f }
            }
        };
        
        private Camera _camera = new Camera()
        {
            Position = new Vector2(-1, -1)
        };

        /// <summary>
        /// Called when the Game is loaded.
        /// Should be used for one-time setup.
        /// </summary>
        public void OnLoad()
        {
        }

        
        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        public void OnRenderFrame()
        {
            MoveCamera();
            RenderScene();
        }

        /// <summary>
        /// Render our scene.
        /// </summary>
        private void RenderScene()
        {
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
        /// Moves the Camera using the arrow keys.
        /// </summary>
        private void MoveCamera()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            float speed = 0.2f;

            if (keyboardState[Key.Right]) _camera.Position.X += speed;
            if (keyboardState[Key.Left])  _camera.Position.X -= speed;
            if (keyboardState[Key.Up])    _camera.Position.Y += speed;
            if (keyboardState[Key.Down])  _camera.Position.Y -= speed;
        }


        /// <summary>
        /// On Window resize call the camera's resize method.
        /// </summary>
        public void OnResize()
        {
           _camera.Resize(Screen.Width, Screen.Height);
        }

        /// <summary>
        /// Convert a world color (float, [0-1], and [1, ->) for HDR), to a screen color (int, [0-255]).
        /// </summary>
        /// <param name="worldColor">The world color of type float, [0-1], and [1, ->).</param>
        /// <returns>A screen color of type int, [0-255].</returns>
        private int ToScreenColor(Vector3 worldColor)
        {
            return ((int) (Math.Min(worldColor.X * 255, 255)) << 16) + 
                   ((int) (Math.Min(worldColor.Y * 255, 255)) << 8) +
                    (int) (Math.Min(worldColor.Z * 255, 255)); 
        }

        /// <summary>
        /// Calculate which color a world space point should have.
        /// </summary>
        /// <param name="point">A world space point for which the color should be determined.</param>
        /// <returns>The determined color</returns>
        private Vector3 Trace(Vector2 point)
        {
            Vector3 colorAtPoint = new Vector3(0, 0, 0);
            
            foreach (Light light in _scene.Lights)
            {
                // Construct ray from point to the light source
                Vector2 vectorToLight = light.Position - point;
                Ray rayToLight = new Ray() { Origin = point, Direction = vectorToLight, Magnitude = vectorToLight.Length};
                
                // Check if the light is occluded (intersected) by a Game Object, continue if it is occluded
                if (_scene.GameObjects.Exists(gameObject => gameObject.Intersects(rayToLight))) continue;
                
                // If the light from the light source is not occluded by any game object,
                // Calculate the light intensity form that non-occluded light source to our point
                float distanceToLight = vectorToLight.Length;
                float intensity = 1f / (4f * (float) Math.PI * distanceToLight * distanceToLight);
                
                // Add the color from the light to our final color
                colorAtPoint += light.Color * intensity;
            }
            
            return colorAtPoint;
        }
    }
}