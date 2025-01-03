#version 330

uniform vec4 ourColor;
uniform sampler2D texture0;

in vec3 normal;
in vec2 texCoord;
in float face;

out vec4 outputColor;

void main()
{
    outputColor = texture(texture0, texCoord);
}
