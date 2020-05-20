using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace infogr_raytracer
{
    public class Window: GameWindow
    {
        private Game _game;

        /// <summary>
        /// Constructs a new Window with the specified attributes.
        /// </summary>
        /// <param name="width">The width of the Window's window</param>
        /// <param name="height">The height of the Window's window</param>
        /// <param name="title">The title of the Window's window</param>
        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title,
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
            
            _game = new Game { Screen = new Surface(Width, Height) };
            _game.OnLoad();

            base.OnLoad(e);
        }

        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Clear the screen
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Let our Game draw stuff to it's Screen
            _game.OnRenderFrame();
            _game.Screen.Draw();
            
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
            
            _game.Screen.Unload();
            _game.Screen = new Surface(Width, Height);
            
            _game.OnResize();
            base.OnResize(e);
        }

        /// <summary>
        /// Called after GameWindow.Exit was called, but before destroying the OpenGL context.
        /// Unloads our screen.
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnUnload(EventArgs e)
        {
            _game.Screen.Unload();
            base.OnUnload(e);
        }
    }
}