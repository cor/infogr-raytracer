#version 330 core

in vec2 screenPosition;
out vec4 FragColor;

#define M_PI 3.1415926535897932384626433832795

uniform int SCREEN_WIDTH;
uniform int SCREEN_HEIGHT;

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

struct Camera
{
    vec2 position;
    vec2 viewport;
    int screenSizeWorldSizeRatio;
} camera;


uniform Light[3] lights = Light[3] (
    Light(vec2(0, 0), vec3(1, 0.5, 0.5)),
    Light(vec2(-1, 0), vec3(0.5, 0.5, 1)),
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



// Convert point from screen space to world space
vec2 ToWorldSpace(vec2 screenSpacePoint)
{
    float ratio = 100; // screen pixels per world space unit
    
    return ((screenSpacePoint + vec2(1, 1)) / 2) * vec2(SCREEN_WIDTH, SCREEN_HEIGHT) / ratio; 
}


vec3 Trace(vec2 worldPoint)
{
    vec3 colorAtPixel = vec3(0, 0, 0);

    for (int i = 0; i < lights.length(); i++)
    {
        vec2 vector2Light = lights[i].position - worldPoint;
        
        // Don't forget to normalize the ray's direction
        Ray ray = Ray(worldPoint, vector2Light, length(vector2Light));
        ray.direction = normalize(ray.direction);

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
    
    return colorAtPixel;
}

void main()
{
    camera.position = vec2 (-5, time - 5);
    
    vec3 colorAtPixel = Trace(ToWorldSpace(screenPosition) + camera.position);
    FragColor = vec4(colorAtPixel, 1);
}