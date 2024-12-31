#version 330

uniform vec4 ourColor; 

out vec4 outputColor;

// This is where the color variable we declared and assigned in vertex shader 
// Gets pass to, this is enabled by using the in keyword 
// Keep in mind the vec type must match in order for this to work

in vec4 vertexColor;

void main()
{
    //outputColor = vec4(1.0, 1.0, 0.0, 1.0);
    //outputColor = vertexColor;
    outputColor = ourColor;
}
