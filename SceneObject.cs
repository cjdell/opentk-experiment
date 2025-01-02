using LearnOpenTK.Common;
using OpenTK.Mathematics;

namespace opentk_experiment
{
    public abstract class SceneObject : IDisposable
    {
        // Not properties so we can directly access the components
        public Vector3 Translation;
        public Vector3 Rotation;

        protected readonly Shader _shader;

        public SceneObject(Shader shader)
        {
            _shader = shader;

            Setup();
        }

        protected abstract void Setup();

        public void Draw(Matrix4 parentModelMatrix)
        {
            var modelMatrix = Matrix4.Identity *
                Matrix4.CreateRotationX(Rotation.X) *
                Matrix4.CreateRotationY(Rotation.Y) *
                Matrix4.CreateRotationZ(Rotation.Z) *
                Matrix4.CreateTranslation(Translation) *
                parentModelMatrix;  // Multiply the parent matrix last

            InternalDraw(modelMatrix);
        }

        protected abstract void InternalDraw(Matrix4 modelMatrix);

        public virtual void Dispose()
        {

        }
    }
}
