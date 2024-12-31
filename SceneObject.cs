using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;

public class SceneObject
{
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
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        var vertexLocation = _shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        // Unbind the VAO to prevent accidental modifications
        GL.BindVertexArray(0);
    }

    public void Draw()
    {
        // Bind the VAO and draw the elements
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    public void Dispose()
    {
        // Clean up resources
        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteBuffer(_elementBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);
    }
}
