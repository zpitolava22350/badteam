using System;
using System.IO;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Program {
    public static void Main(string[] args) {
        var nativeWindowSettings = new NativeWindowSettings() {
            Size = new Vector2i(800, 600),
            Title = "OpenTK Cube Example",
            Flags = ContextFlags.ForwardCompatible,
        };

        using (var window = new Game(nativeWindowSettings)) {
            window.Run();
        }
    }
}

public class Game: GameWindow {
    private int _vertexArrayObject;
    private int _vertexBufferObject;
    private int _elementBufferObject;
    private int _shaderProgram;

    private readonly float[] _vertices =
    {
        // Positions          // Colors
        -0.5f, -0.5f, -0.5f,  1.0f, 0.0f, 0.0f,
         0.5f, -0.5f, -0.5f,  0.0f, 1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  0.0f, 0.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,  1.0f, 1.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 1.0f,
         0.5f, -0.5f,  0.5f,  0.0f, 1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 0.0f, 0.0f
    };

    private readonly uint[] _indices =
    {
        0, 1, 2, 2, 3, 0,  // Back face
        4, 5, 6, 6, 7, 4,  // Front face
        0, 1, 5, 5, 4, 0,  // Bottom face
        3, 2, 6, 6, 7, 3,  // Top face
        0, 4, 7, 7, 3, 0,  // Left face
        1, 5, 6, 6, 2, 1   // Right face
    };

    private Matrix4 _model;
    private Matrix4 _view;
    private Matrix4 _projection;

    public Game(NativeWindowSettings settings)
        : base(GameWindowSettings.Default, settings) {
    }

    protected override void OnLoad() {
        base.OnLoad();

        GL.ClearColor(0.1f, 0.2f, 0.3f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        var vertexShaderSource = File.ReadAllText("vertexShader.glsl");
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);

        var fragmentShaderSource = File.ReadAllText("fragmentShader.glsl");
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(fragmentShader);

        _shaderProgram = GL.CreateProgram();
        GL.AttachShader(_shaderProgram, vertexShader);
        GL.AttachShader(_shaderProgram, fragmentShader);
        GL.LinkProgram(_shaderProgram);

        GL.DetachShader(_shaderProgram, vertexShader);
        GL.DetachShader(_shaderProgram, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        _model = Matrix4.Identity;
        _view = Matrix4.LookAt(new Vector3(1.5f, 1.5f, 1.5f), Vector3.Zero, Vector3.UnitY);
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100.0f);
    }

    protected override void OnRenderFrame(FrameEventArgs e) {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _model = Matrix4.CreateRotationY((float)GLFW.GetTime());
        Console.WriteLine(GLFW.GetTime());

        GL.UseProgram(_shaderProgram);

        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "model"), false, ref _model);
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "view"), false, ref _view);
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "projection"), false, ref _projection);

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

        Context.SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e) {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape)) {
            Close();
        }
    }

    protected override void OnResize(ResizeEventArgs e) {
        base.OnResize(e);
        GL.Viewport(0, 0, Size.X, Size.Y);
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100.0f);
    }

    protected override void OnUnload() {
        base.OnUnload();

        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteBuffer(_elementBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);
        GL.DeleteProgram(_shaderProgram);
    }
}