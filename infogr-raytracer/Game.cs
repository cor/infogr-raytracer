using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace infogr_raytracer
{
    public class Game: GameWindow
    {
        private Surface _screen;

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
            
            _screen = new Surface(Width, Height);

            base.OnLoad(e);
        }

        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Clear the screen
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _screen.Clear(255);
            
            // Mutate the screen
            double size = 200 +  Math.Sin(DateTime.Now.Ticks / 10_000_000.0 * 2 * Math.PI) * 100;
            _screen.Bar(0, 0, (int) size, (int) size, 255 << 8);
            _screen.Print("hi", 10, 10, 0xFFFFFF);

            // Draw the screen
            _screen.Draw();
            
            // Show the new frame
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        /// <summary>
        /// Called when this window is resized.
        /// Updates viewport to match window size.
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            
            _screen.Unload();
            _screen = new Surface(Width, Height);
            
            base.OnResize(e);
        }

        /// <summary>
        /// Called after GameWindow.Exit was called, but before destroying the OpenGL context.
        /// Unloads our screen.
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnUnload(EventArgs e)
        {
            _screen.Unload();
            base.OnUnload(e);
        }
    }
}