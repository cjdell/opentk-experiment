using LearnOpenTK.Common;

namespace opentk_experiment
{
    public class Cube(Shader shader, float size) : Mesh(shader)
    {
        public float Size => size;

        protected override GeometryHelper.MeshData GetMeshData()
        {
            return GeometryHelper.CreateCube(size);
        }
    }
}
