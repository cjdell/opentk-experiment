using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System.Diagnostics;
using OpenTK.Mathematics;

namespace opentk_experiment
{
    public class Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : GameWindow(gameWindowSettings, nativeWindowSettings)
    {
        private Stopwatch _timer;
        private Shader _shader;
        private readonly List<SceneObject> _sceneObjects = [];
        private Camera _camera;

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            // Initialize the shader
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.SetInt("texture0", 0);
            _shader.Use();

            var plane = new Plane(_shader, 1.0f, 1.0f)
            {
                Texture = Texture.LoadFromFile("Resources/texture.png")
            };

            var cube = new Cube(_shader, 1.0f)
            {
                Texture = Texture.LoadFromFile("Resources/cube.png")
            };

            plane.Translation.Y = 2.0f;

            _sceneObjects.Add(plane);
            _sceneObjects.Add(cube);
            _sceneObjects.Add(new Sphere(_shader, 0.5f, 24, 24));

            var group = new SceneGroup(_shader);

            var sphere1 = new Sphere(_shader, 0.5f, 24, 24);
            sphere1.Translation.X = -2.0f;

            var sphere2 = new Sphere(_shader, 0.5f, 24, 24);
            sphere2.Translation.X = 2.0f;

            group.Children.Add(sphere1);
            group.Children.Add(sphere2);

            _sceneObjects.Add(group);

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

            _sceneObjects[0].Rotation.X = (float)MathHelper.DegreesToRadians(timeValue * 90.0f);
            _sceneObjects[1].Rotation.Y = (float)MathHelper.DegreesToRadians(timeValue * 90.0f);
            _sceneObjects[1].Rotation.X = (float)MathHelper.DegreesToRadians(timeValue * 30.0f);
            _sceneObjects[2].Translation.Y = (float)Math.Sin(timeValue) * 2.0f;

            _sceneObjects[3].Rotation.Y = (float)MathHelper.DegreesToRadians(timeValue * 90.0f);
            _sceneObjects[3].Translation.Y = (float)Math.Sin(timeValue / 4.0f);

            _shader.SetMatrix4("uView", _camera.GetViewMatrix());
            _shader.SetMatrix4("uProjection", _camera.GetProjectionMatrix());

            _shader.SetVector3("uLightColour", new Vector3(1.0f, 1.0f, 1.0f));
            _shader.SetVector3("uLightPos", new Vector3(1.2f, 1.0f, 2.0f));
            _shader.SetVector3("uViewPos", _camera.Position);

            // Draw the scene object
            _sceneObjects.ForEach(o => o.Draw(Matrix4.Identity));

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

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position -= Vector3.UnitZ * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position += Vector3.UnitZ * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= Vector3.UnitX * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += Vector3.UnitX * (float)e.Time;
            }

            if (input.IsKeyPressed(Keys.O))
            {
                _camera.Orthographic = true;
            }
            if (input.IsKeyPressed(Keys.P))
            {
                _camera.Orthographic = false;
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
