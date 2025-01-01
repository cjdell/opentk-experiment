using LearnOpenTK.Common;

namespace dotnet_console_1
{
    public class Plane(Shader shader, float width, float height) : SceneObject(shader)
    {
        public float Width => width;
        public float Height => height;

        protected override GeometryHelper.MeshData GetMeshData()
        {
            return GeometryHelper.CreatePlane(width, height, 2);
        }
    }
}
