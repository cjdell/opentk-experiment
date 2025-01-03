#version 330

uniform vec4 ourColor;
uniform sampler2D texture0;

in vec4 vertexColor;
in vec2 texCoord;

out vec4 outputColor;

void main()
{
    outputColor = texture(texture0, texCoord);
}
