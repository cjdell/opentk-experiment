#version 330

uniform vec4 ourColor;

in vec4 vertexColor;

out vec4 outputColor;

void main()
{
    //outputColor = vec4(1.0, 1.0, 0.0, 1.0);
    //outputColor = vertexColor;
    outputColor = ourColor + vertexColor;
}
