#version 330 core

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

in vec3 aPosition;
in vec3 aNormal;

out vec4 vertexColor;

void main(void)
{
    // gl_Position = vec4(aPosition, 1.0);
    gl_Position = vec4(aPosition, 1.0) * uModel * uView * uProjection;

    vertexColor = vec4(aNormal, 1.0);
}
