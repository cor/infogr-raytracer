#version 330 core
out vec4 FragColor;

#define M_PI 3.1415926535897932384626433832795

uniform float time;

struct Ray {
    vec2 origin;
    vec2 direction;
    float magnitude;
};

struct Light
{
    vec2 position;
    vec3 color;
};

struct Circle
{
    vec2 position;
    float radius;
};

uniform Light[2] lights = Light[2] (
    Light(vec2(0, 0), vec3(2, 1, 1)),
    Light(vec2(0.5, 0.5), vec3(0.25, 1, 0.25))
);

uniform Circle[1] circles = Circle[1] (
    Circle(vec2(-0.5, -0.5), 0.1)
);

bool circle_collides(Circle circle, Ray ray) 
{
    vec2 posToOrigin = ray.origin - circle.position;
    float a = dot(ray.direction, ray.direction);
    float b = dot(ray.direction, posToOrigin);
    float c = dot(posToOrigin, posToOrigin) - (circle.radius * circle.radius);
    float d = (b * b) - (a * c);
    
    if (d < 0){return false;}
    
    float sqrtD = sqrt(d);
    float distance = (-b - sqrtD) / a;
    if (distance < 0) {distance = (-b + sqrtD) / a;}
    
    return (distance > 0 && distance < ray.magnitude);
}

in vec2 screenPosition;

void main()
{
    vec3 colorAtPixel = vec3(0, 0, 0);
    
    for (int i = 0; i < lights.length(); i++)
    {
        vec2 vector2Light = lights[i].position + vec2(0, time) - screenPosition;
        Ray ray = Ray(screenPosition, vector2Light, length(vector2Light));
        
        bool occluded = false;
        for (int c = 0; c < circles.length(); c++) {
            Circle circle = circles[c];
            if (circle_collides(circle, ray)) {
                occluded = true;
                break;
            }
        }
        if (occluded) {continue;}
        
        
        float distanceToLight = length(vector2Light);
        float intensity = 1.0 / (4 * M_PI * distanceToLight * distanceToLight);
        
        colorAtPixel += lights[i].color * intensity;
    }
    
    FragColor = vec4(colorAtPixel, 1);
}