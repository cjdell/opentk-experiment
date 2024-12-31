#version 330 core

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

in vec3 aPosition;

out vec4 vertexColor;

void main(void)
{
    // gl_Position = vec4(aPosition, 1.0);
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;

    float depth = aPosition.z / 2.0f;
    vertexColor = vec4(depth, depth, depth, 1.0);
}
