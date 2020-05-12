#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;

out vec2 texCoord;

void main(void)
{
    // pass the input texture coordinate to the fragment shader
    texCoord = aTexCoord;
    
    gl_Position = vec4(aPosition, 1.0);
}