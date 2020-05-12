using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace infogr_raytracer
{
    public class Game: GameWindow
    {
        /// <summary>
        /// Constructs a new Game with the specified attributes.
        /// </summary>
        /// <param name="width">The width of the Game's window</param>
        /// <param name="height">The height of the Game's window</param>
        /// <param name="title">The title of the Game's window</param>
        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title,
            GameWindowFlags.Default, DisplayDevice.Default,
            3, 3, GraphicsContextFlags.ForwardCompatible)
        {
            
        }
        
        /// <summary>
        /// Print OpenGL and Open GL Shading Language versions
        /// </summary>
        private static void PrintVersionInfo()
        {
            Console.WriteLine($"OpenGL Version: {GL.GetString(StringName.Version)}");
            Console.WriteLine($"OpenGL Shading Language Version: {GL.GetString(StringName.ShadingLanguageVersion)}");
            Console.WriteLine();
        }

        /// <summary>
        /// Called after an OpenGL context has been established, but before entering the main loop.
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            PrintVersionInfo();
            base.OnLoad(e);
        }
    }
}