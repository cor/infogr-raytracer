using System;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace infogr_raytracer
{
    public class ShaderGame
    {
        
        private readonly float[] _vertices =
        {
             1.0f,  1.0f, 0.0f,
             1.0f, -1.0f, 0.0f,
            -1.0f, -1.0f, 0.0f,
            -1.0f,  1.0f, 0.0f
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
        
        /// <summary>
        /// Called when the Game is loaded.
        /// Should be used for one-time setup.
        /// </summary>
        public void OnLoad()
        {
            VaoSetup();
            
        }

        private void VaoSetup()
        {
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
			
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            
            
            _shader = new Shader("shaders/raytrace-shader.vert", "shaders/raytrace-shader.frag");
            _shader.Use();
            
            int vertexLocation = _shader.GetAttribLocation("vertexPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        public void OnRenderFrame()
        {
            GL.BindVertexArray(_vertexArrayObject);
            
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            
            stopwatch.Stop();
            
            var fps = stopwatch.ElapsedMilliseconds == 0 ? 
                "Infinite" :  
                $"{1_000 / stopwatch.ElapsedMilliseconds}";
            Console.WriteLine($"FPS: {fps}");
        }

        public void OnUnload()
        {
            _shader.Dispose();
        }
    }
}