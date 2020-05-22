#version 330 core
layout (location = 0) in vec3 vertexPosition;

out vec2 screenPosition;

void main()
{
    gl_Position = vec4(vertexPosition, 1.0);
    
    screenPosition = vertexPosition.xy;
}