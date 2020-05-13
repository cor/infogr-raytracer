using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace infogr_raytracer
{
    public class Game: GameWindow
    {
        private Surface _screen;
        private int _screenTexId;
        
        private float[] _vertices =
        {
            // Position         Texture coordinates
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f, // top right
             1.0f, -1.0f, 0.0f, 1.0f, 0.0f, // bottom right
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, // bottom left
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f  // top left
        };
        
        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };
        
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;

        private Shader _shader;
        private Texture _texture;

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
            // _screen = new Surface(width, height);
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
            
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            
            _shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
            _shader.Use();
            
            // Create a new Surface to draw on
            _screen = new Surface(Width, Height);
            _screenTexId = _screen.GenTexture();
            
            // _texture = new Texture("assets/container.png");
            // _texture.Use();
            
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BindTexture(TextureTarget.Texture2D, _screenTexId);


            int vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            int texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            
            base.OnLoad(e);
        }

        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _screen.Clear(255);
            
            // Draw stuff on the screen
            double size = 200 +  Math.Sin(DateTime.Now.Ticks / 10_000_000.0 * 2 * Math.PI) * 100;
            _screen.Bar(0, 0, (int) size, (int) size, 255 << 8);
            
            // Regenerate the screen's texture
            // TODO: Avoid doing this and instead update the texture's data.
            _screenTexId = _screen.GenTexture();
            
            
            GL.BindVertexArray(_vertexArrayObject);
            _shader.Use();
            
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            
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
            
            // TODO: Find out if this is necessary
            _screen = new Surface(Width, Height);
            
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            
            _shader.Dispose();
            _texture.Dispose();
            
            base.OnUnload(e);
        }
    }
}