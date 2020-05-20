using System;
using OpenTK;
using OpenTK.Graphics;

namespace infogr_raytracer
{
    public class Game
    {
        public Surface Screen;

        private Light _light;

        /// <summary>
        /// Called when the Game is loaded.
        /// Should be used for one-time setup.
        /// </summary>
        public void OnLoad()
        {
            _light = new Light() { Color = new Vector3(1, 1, 1), Position = new Vector2(1, 1) };
        }

        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        public void OnRenderFrame()
        {
            // Clear the screen
            Screen.Clear(255);
            
            // Draw stuff on our screen
            double size = 200 +  Math.Sin(DateTime.Now.Ticks / 10_000_000.0 * 2 * Math.PI) * 100;
            Screen.Bar(0, 0, (int) size, (int) size, 255 << 8);
            Screen.Print("hi", 10, 10, 0xFFFFFF);
        }
    }
}