using LearnOpenTK.Common;

namespace opentk_experiment
{
    public class Plane(Shader shader, float width, float height) : Mesh(shader)
    {
        public float Width => width;
        public float Height => height;

        protected override GeometryHelper.MeshData GetMeshData()
        {
            return GeometryHelper.CreatePlane(width, height, 2);
        }
    }
}
