using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System.Diagnostics;
using OpenTK.Mathematics;

namespace dotnet_console_1
{
    public class Window : GameWindow
    {
        private Stopwatch _timer;
        private Shader _shader;
        private List<SceneObject> _sceneObjects = new List<SceneObject>();
        private Camera _camera;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            // Initialize the shader
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            // Create a SceneObject with the vertices and indices
            _sceneObjects.Add(new SceneObject(_shader, [
                 0.5f,  0.5f, 0.0f, // top right
                 0.5f, -0.5f, 0.0f, // bottom right
                -0.5f, -0.5f, 0.0f, // bottom left
                -0.5f,  0.5f, 0.0f, // top left
            ], [
                0, 1, 3, // First triangle
                1, 2, 3  // Second triangle
            ]));

            _sceneObjects.Add(new SceneObject(_shader, [
                 1.0f,  0.5f, 1.0f, // top right
                 1.0f, -0.5f, 1.0f, // bottom right
                 0.0f, -0.5f, 1.0f, // bottom left
                 0.0f,  0.5f, 1.0f, // top left
            ], [
                0, 1, 3, // First triangle
                1, 2, 3  // Second triangle
            ]));

            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            // Start the timer
            _timer = new Stopwatch();
            _timer.Start();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            double timeValue = _timer.Elapsed.TotalSeconds;
            float greenValue = (float)Math.Sin(timeValue) / 2.0f + 0.5f;

            int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
            GL.Uniform4(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);

            var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(timeValue * 90.0f));

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            // Draw the scene object
            _sceneObjects.ForEach(o => o.Draw());

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
        }
    }
}
