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
                new Circle() { Position = new Vector2(4, 4.5f), Radius = 0.1f },
                new Circle() { Position = new Vector2(6.5f,6.5f), Radius = 1f },
                new Circle() { Position = new Vector2(4f,4), Radius = 0.3f },
                new Circle() { Position = new Vector2(3f, 3f), Radius = 0.1f}
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

            // Test case that demonstrates our bug
            Light light = new Light()
            {
                Position = new Vector2(0, 4),
                Color = new Vector3(1, 1, 1)
            };
            
            Vector2 pointThatShouldBeBlack = new Vector2(10, 4);
            
            Vector2 pointToLight = light.Position - pointThatShouldBeBlack;
            
            Ray ray = new Ray() { Origin = pointThatShouldBeBlack, Direction = pointToLight};
            
            Circle testCircle = new Circle()
            {
                Position = new Vector2(4f, 4), 
                Radius = 2f
            };

            Console.WriteLine(testCircle.Intersects(ray));
            
            
            
            
            Ray aRay = new Ray()
            {
                Origin = new Vector2(0, 1), 
                Direction = new Vector2(1, 0)
            };
        
            Circle aCircle = new Circle()
            {
                Position = new Vector2(5, 1),
                Radius = 1f
            };
            
            // Console.WriteLine(aCircle.Intersects(aRay));

        }


        


        //
        // if (aCircle.Intersects(aRay))
        //     aCircle.Trace(aRay);
        //
        // Ray bRay = new Ray()
        // {
        //     Origin = new Vector2(0, 1), 
        //     Direction = new Vector2(1, 0)
        // };
        //
        // Circle bCircle = new Circle()
        // {
        //     Position = new Vector2(5, 10),
        //     Radius = 2f
        // };
        //
        // if (bCircle.Intersects(bRay))
        //     bCircle.Trace(bRay);
        //
        //
        // Ray cRay = new Ray()
        // {
        //     Origin = new Vector2(0, 1), 
        //     Direction = new Vector2(1, 0)
        // };
        //
        // Circle cCircle = new Circle()
        // {
        //     Position = new Vector2(2, 2f),
        //     Radius = 1f
        // };
        //
        // if (cCircle.Intersects(cRay))
        //     cCircle.Trace(cRay);

    

        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        public void OnRenderFrame()
        {

            MoveCamera();
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

        private void MoveCamera()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            float speed = 0.2f;

            if (keyboardState[Key.Right])
                _camera.Position.X += speed;
            if (keyboardState[Key.Left])
                _camera.Position.X -= speed;
            if (keyboardState[Key.Up])
                _camera.Position.Y += speed;
            if (keyboardState[Key.Down])
                _camera.Position.Y -= speed;
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
                // check for occlusions
                Vector2 pointToLight = light.Position - point;
                Ray ray = new Ray() { Origin = point, Direction = pointToLight, T = pointToLight.Length};
                
                if (_scene.GameObjects.Exists(gameObject => gameObject.Intersects(ray))) continue;
                
                float distanceToLight = pointToLight.Length;
                float intensity = 1f / (4f * (float) Math.PI * distanceToLight * distanceToLight);

                colorAtPoint += light.Color * intensity;
            }
            
            return colorAtPoint;
        }
    }
}