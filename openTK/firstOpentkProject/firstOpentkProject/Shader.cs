using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace YourNamespaceHere {
    public class Shader: IDisposable {
        int Handle;

        public Shader(string vertexPath, string fragmentPath) {
            string VertexShaderSource = File.ReadAllText(vertexPath);
            string FragmentShaderSource = File.ReadAllText(fragmentPath);

            int VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);
            GL.CompileShader(VertexShader);

            int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);
            GL.CompileShader(FragmentShader);

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);
            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0) {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        public void Use() {
            GL.UseProgram(Handle);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                GL.DeleteProgram(Handle);
                disposedValue = true;
            }
        }

        ~Shader() {
            if (disposedValue == false) {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
