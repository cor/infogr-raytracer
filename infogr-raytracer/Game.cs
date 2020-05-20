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

        private int ToScreenColor(Vector3 worldColor)
        {
            return ((int) (Math.Min(worldColor.X * 255, 255)) << 16) + 
                   ((int) (Math.Min(worldColor.Y * 255, 255)) << 8) +
                   (int) (Math.Min(worldColor.Z * 255, 255)); 
        }

        private Vector3 Trace(Vector2 point)
        {
            return new Vector3(1, 0, 1);
        }

        private float WorldSpaceX(int screenSpaceX)
        {
            return (float) screenSpaceX / (float) Screen.Width;
        }

        private float WorldSpaceY(int screenSpaceY)
        {
            return (float) screenSpaceY /  (float) Screen.Height;
        }
    }
}