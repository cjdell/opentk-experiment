#version 330

uniform vec3 uLightColour; // The color of the light.
uniform vec3 uLightPos; // The position of the light.
uniform vec3 uViewPos; // The position of the view and/or of the player.

uniform sampler2D texture0;

in vec3 fragPos;
in vec3 normal;
in vec2 texCoord;
in float face;

out vec4 outputColor;

vec2 mapCubeFaceTexCoords(vec2 texCoords, int faceIndex) {
    if (faceIndex < 10) {
        return texCoords;
    }

    faceIndex -= 10;

    // Determine the face's row (0 for top row, 1 for bottom row)
    int row = 2 - faceIndex / 3;
    // Determine the face's column (0, 1, or 2 within its row)
    int col = faceIndex % 3;

    // Calculate the offset for the face in the texture atlas
    vec2 offset = vec2(col / 3.0, row / 3.0);

    // Scale the input texture coordinates to the size of one face
    vec2 scaledCoords = texCoords / 3.0;

    // Add the offset to position the texture coordinates within the appropriate face
    return offset + scaledCoords;
}

void main()
{
    // vec3 objectColor = vec3(1.0, 1.0, 1.0); 
    vec3 objectColor = texture(texture0, mapCubeFaceTexCoords(texCoord, int(face))).rgb;

    //The ambient color is the color where the light does not directly hit the object.
    //You can think of it as an underlying tone throughout the object. Or the light coming from the scene/the sky (not the sun).
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * uLightColour;

    //We calculate the light direction, and make sure the normal is normalized.
    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(uLightPos - fragPos); //Note: The light is pointing from the light to the fragment

    //The diffuse part of the phong model.
    //This is the part of the light that gives the most, it is the color of the object where it is hit by light.
    float diff = max(dot(norm, lightDir), 0.0); //We make sure the value is non negative with the max function.
    vec3 diffuse = diff * uLightColour;


    //The specular light is the light that shines from the object, like light hitting metal.
    //The calculations are explained much more detailed in the web version of the tutorials.
    float specularStrength = 0.5;
    vec3 viewDir = normalize(uViewPos - fragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32); //The 32 is the shininess of the material.
    vec3 specular = specularStrength * spec * uLightColour;

    //At last we add all the light components together and multiply with the color of the object. Then we set the color
    //and makes sure the alpha value is 1
    vec3 result = (ambient + diffuse + specular) * objectColor;
    outputColor = vec4(result, 1.0);
    
    //Note we still use the light color * object color from the last tutorial.
    //This time the light values are in the phong model (ambient, diffuse and specular)
}
