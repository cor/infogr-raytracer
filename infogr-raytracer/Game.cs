using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace infogr_raytracer
{
    public class Game: GameWindow
    {
        private Surface _screen;
        
        // OpenGL stuff for triangle
        private float[] _vertices =
        {
            -0.5f, -0.5f, 0.0f, // Bottom-left vertex
             0.5f, -0.5f, 0.0f, // Bottom-right vertex
             0.0f,  0.5f, 0.0f  // Top vertex
        };

        private int _vertexBufferObject;

        private Shader _shader;
        
        
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
            _screen = new Surface(width, height);
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
            
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            
            // Initialize triangle object
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            
            // Initialize our shader
            _shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
            
            base.OnLoad(e);
        }

        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            
            
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
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferObject);
            
            _shader.Dispose();
            
            base.OnUnload(e);
        }
    }
}