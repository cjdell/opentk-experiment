#version 330 core

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

in vec3 aPosition;
in vec3 aNormal;
in vec2 aTexCoord;
in float aFace;

out vec3 fragPos;
out vec3 normal;
out vec2 texCoord;
out float face;

void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * uModel * uView * uProjection;

    fragPos = vec3(vec4(aPosition, 1.0) * uModel);
    normal = aNormal * mat3(transpose(inverse(uModel)));
    texCoord = aTexCoord;
    face = aFace;
}
