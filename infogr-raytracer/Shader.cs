using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace infogr_raytracer
{
    public class Shader: IDisposable
    {
        public int Handle;

        public Shader(string vertexPath, string fragmentPath)
        {
            // Get the Vertex Shader Source from the file located at vertexPath
            // and the Fragment Shader Source from the file located at fragmentPath
            string vertexShaderSource;
            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
                vertexShaderSource = reader.ReadToEnd();

            string fragmentShaderSource;
            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
                fragmentShaderSource = reader.ReadToEnd();

            
            // Create OpenGL shaders and bind the source code to the shaders
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            
            
            // Compile the shaders and check for errors
            GL.CompileShader(vertexShader);
            string infoLogVert = GL.GetShaderInfoLog(vertexShader);
            if (infoLogVert != String.Empty)
                Console.WriteLine(infoLogVert);
            
            GL.CompileShader(fragmentShader);
            string infoLogFrag = GL.GetShaderInfoLog(fragmentShader);
            if (infoLogFrag != String.Empty)
                Console.WriteLine(infoLogFrag);
            
            
            // Link our compiled shaders together into a program that can run on the GPU.
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            
            GL.LinkProgram(Handle); // Handle is now a usable shader program
            
            
            // Cleanup. Vertex and fragment shaders are useless now that they've been linked. We detach and delete them.
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void SetUniform4(string name, float v0, float v1, float v2, float v3)
        {
            Use();
            GL.Uniform4(GL.GetUniformLocation(Handle, name), v0, v1, v2, v3);
        }

        public void SetFloat(string name, float value)
        {
            Use();
            GL.Uniform1(GL.GetUniformLocation(Handle, name), value);
        }

        public void SetInt(string name, int value)
        {
            Use();
            GL.Uniform1(GL.GetUniformLocation(Handle, name), value);
        }
        
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }
        
        
        // Make our shader disposable
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                GL.DeleteProgram(Handle);

                _disposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
    }
}