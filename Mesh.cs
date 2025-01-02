using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace opentk_experiment
{
    public abstract class Mesh(Shader shader) : SceneObject(shader)
    {
        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private int _vertexCount;

        protected abstract GeometryHelper.MeshData GetMeshData();

        protected override void Setup()
        {
            var meshData = GetMeshData();

            _vertexCount = meshData.Indices.Length;

            // Generate and bind VAO
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            // Generate and bind VBO
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, meshData.Attributes.Length * sizeof(float), meshData.Attributes, BufferUsageHint.StaticDraw);

            // Generate and bind EBO
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, meshData.Indices.Length * sizeof(uint), meshData.Indices, BufferUsageHint.StaticDraw);

            var stride = 9 * sizeof(float);

            // Set up vertex attribute pointers (position data)
            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(vertexLocation);

            var normalLocation = _shader.GetAttribLocation("aNormal");
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(normalLocation);

            var textureLocation = _shader.GetAttribLocation("aUv");
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
            GL.EnableVertexAttribArray(textureLocation);

            var faceLocation = _shader.GetAttribLocation("aFace");
            GL.VertexAttribPointer(faceLocation, 1, VertexAttribPointerType.Float, false, stride, 8 * sizeof(float));
            GL.EnableVertexAttribArray(faceLocation);

            // Unbind the VAO to prevent accidental modifications
            GL.BindVertexArray(0);
        }

        protected override void InternalDraw(Matrix4 modelMatrix)
        {
            // Each object sets its own model matrix
            _shader.SetMatrix4("uModel", modelMatrix);

            // Bind the VAO and draw the elements
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _vertexCount, DrawElementsType.UnsignedInt, 0);
            // GL.DrawElements(PrimitiveType.LineLoop, _vertexCount, DrawElementsType.UnsignedInt, 0);
        }

        public override void Dispose()
        {
            base.Dispose();

            // Clean up resources
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
        }
    }
}
