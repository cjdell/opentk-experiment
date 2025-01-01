using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

public class SceneObject
{
    // Not properties so we can directly access the components
    public Vector3 Translation;
    public Vector3 Rotation;

    private int _vertexArrayObject;
    private int _vertexBufferObject;
    private int _elementBufferObject;

    private readonly Shader _shader;
    private readonly float[] _vertices;
    private readonly uint[] _indices;

    public SceneObject(Shader shader, float[] vertices, uint[] indices)
    {
        _shader = shader;
        _vertices = vertices;
        _indices = indices;

        Setup();
    }

    private void Setup()
    {
        // Generate and bind VAO
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        // Generate and bind VBO
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        // Generate and bind EBO
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        // Set up vertex attribute pointers (position data)
        var vertexLocation = _shader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(vertexLocation);

        var normalLocation = _shader.GetAttribLocation("aNormal");
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(normalLocation);

        // Unbind the VAO to prevent accidental modifications
        GL.BindVertexArray(0);
    }

    public void Draw()
    {
        var model = Matrix4.Identity *
            Matrix4.CreateRotationX(Rotation.X) *
            Matrix4.CreateRotationY(Rotation.Y) *
            Matrix4.CreateRotationZ(Rotation.Z) *
            Matrix4.CreateTranslation(Translation);

        // Each object sets its own model matrix
        _shader.SetMatrix4("model", model);

        // Bind the VAO and draw the elements
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        // GL.DrawElements(PrimitiveType.LineLoop, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    public void Dispose()
    {
        // Clean up resources
        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteBuffer(_elementBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);
    }
}
