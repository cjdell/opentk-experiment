using LearnOpenTK.Common;
using OpenTK.Mathematics;

namespace opentk_experiment
{
    public class SceneGroup(Shader shader) : SceneObject(shader)
    {
        public List<SceneObject> Children { get; private set; } = [];

        protected override void Setup() { }

        protected override void InternalDraw(Matrix4 modelMatrix)
        {
            foreach (var child in Children)
            {
                child.Draw(modelMatrix);
            }
        }
    }
}
