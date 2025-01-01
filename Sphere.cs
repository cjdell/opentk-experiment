using LearnOpenTK.Common;

namespace opentk_experiment
{
    public class Sphere(Shader shader, float radius, int latitudeSegments, int longitudeSegments) : SceneObject(shader)
    {
        public float Radius => radius;
        public int LatitudeSegments => latitudeSegments;
        public int LongitudeSegments => longitudeSegments;

        protected override GeometryHelper.MeshData GetMeshData()
        {
            return GeometryHelper.CreateSphere(radius, latitudeSegments, longitudeSegments);
        }
    }
}
