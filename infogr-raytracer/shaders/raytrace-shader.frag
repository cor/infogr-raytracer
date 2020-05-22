#version 330 core
out vec4 FragColor;

#define M_PI 3.1415926535897932384626433832795

const vec2 lightPosition = vec2(0, 0);
const vec3 lightColor = vec3(2, 1, 1);

in vec2 screenPosition;

void main()
{
    vec2 vector2Light = screenPosition.xy - lightPosition;
    float distanceToLight = length(vector2Light);
    float intensity = 1.0 / (4 * M_PI * distanceToLight * distanceToLight);
    
    FragColor = vec4(lightColor * intensity, 1);
}