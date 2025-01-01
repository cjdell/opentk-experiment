#version 330

uniform vec4 ourColor;

in vec4 vertexColor;

out vec4 outputColor;

void main()
{
    outputColor = ourColor + vertexColor;
}
