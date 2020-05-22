#version 330 core
out vec4 FragColor;

#define M_PI 3.1415926535897932384626433832795

uniform float time;

struct Light
{
    vec2 position;
    vec3 color;
};

uniform Light[2] lights = Light[2] (
    Light(vec2(0, 0), vec3(2, 1, 1)),
    Light(vec2(0.5, 0.5), vec3(0.25, 1, 0.25))
);

//const vec2 lightPosition = vec2(0, 0);
//const vec3 lightColor = vec3(2, 1, 1);

in vec2 screenPosition;

void main()
{
    vec3 colorAtPixel = vec3(0, 0, 0);
    
    for (int i = 0; i < lights.length(); i++)
    {
        vec2 vector2Light = screenPosition.xy - lights[i].position + vec2(0, time);
        float distanceToLight = length(vector2Light);
        float intensity = 1.0 / (4 * M_PI * distanceToLight * distanceToLight);
        
        colorAtPixel += lights[i].color * intensity;
    }
    
    FragColor = vec4(colorAtPixel, 1);
}