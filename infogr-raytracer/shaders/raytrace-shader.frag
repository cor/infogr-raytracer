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


uniform Light[] lights = Light[] (
    Light(vec2(0, 0), vec3(0.5, 0.5, 0.5)),
    Light(vec2(1, 0), vec3(0.5, 0.5, 0.5)),
    Light(vec2(2, 0), vec3(0.5, 0.5, 0.5)),
    Light(vec2(3, 0), vec3(0.5, 0.5, 0.5)),
    Light(vec2(4, 0), vec3(0.5, 0.5, 0.5)),
    Light(vec2(5, 0), vec3(0.5, 0.5, 0.5)),
    Light(vec2(6, 0), vec3(0.5, 0.5, 0.5)),
    Light(vec2(7, 0), vec3(0.5, 0.5, 0.5)),
    Light(vec2(8, 0), vec3(0.5, 0.5, 0.5)),
    Light(vec2(9, 0), vec3(0.5, 0.5, 0.5)),
    Light(vec2(0, 1), vec3(0.5, 0.5, 0.5)),
    Light(vec2(1, 1), vec3(0.5, 0.5, 0.5)),
    Light(vec2(2, 1), vec3(0.5, 0.5, 0.5)),
    Light(vec2(3, 1), vec3(0.5, 0.5, 0.5)),
    Light(vec2(4, 1), vec3(0.5, 0.5, 0.5)),
    Light(vec2(5, 1), vec3(0.5, 0.5, 0.5)),
    Light(vec2(6, 1), vec3(0.5, 0.5, 0.5)),
    Light(vec2(7, 1), vec3(0.5, 0.5, 0.5)),
    Light(vec2(8, 1), vec3(0.5, 0.5, 0.5)),
    Light(vec2(9, 1), vec3(0.5, 0.5, 0.5)),
    Light(vec2(0, 2), vec3(0.5, 0.5, 0.5)),
    Light(vec2(1, 2), vec3(0.5, 0.5, 0.5)),
    Light(vec2(2, 2), vec3(0.5, 0.5, 0.5)),
    Light(vec2(3, 2), vec3(0.5, 0.5, 0.5)),
    Light(vec2(4, 2), vec3(0.5, 0.5, 0.5)),
    Light(vec2(5, 2), vec3(0.5, 0.5, 0.5)),
    Light(vec2(6, 2), vec3(0.5, 0.5, 0.5)),
    Light(vec2(7, 2), vec3(0.5, 0.5, 0.5)),
    Light(vec2(8, 2), vec3(0.5, 0.5, 0.5)),
    Light(vec2(9, 2), vec3(0.5, 0.5, 0.5)),
    Light(vec2(0, 3), vec3(0.5, 0.5, 0.5)),
    Light(vec2(1, 3), vec3(0.5, 0.5, 0.5)),
    Light(vec2(2, 3), vec3(0.5, 0.5, 0.5)),
    Light(vec2(3, 3), vec3(0.5, 0.5, 0.5)),
    Light(vec2(4, 3), vec3(0.5, 0.5, 0.5)),
    Light(vec2(5, 3), vec3(0.5, 0.5, 0.5)),
    Light(vec2(6, 3), vec3(0.5, 0.5, 0.5)),
    Light(vec2(7, 3), vec3(0.5, 0.5, 0.5)),
    Light(vec2(8, 3), vec3(0.5, 0.5, 0.5)),
    Light(vec2(9, 3), vec3(0.5, 0.5, 0.5)),
    Light(vec2(0, 4), vec3(0.5, 0.5, 0.5)),
    Light(vec2(1, 4), vec3(0.5, 0.5, 0.5)),
    Light(vec2(2, 4), vec3(0.5, 0.5, 0.5)),
    Light(vec2(3, 4), vec3(0.5, 0.5, 0.5)),
    Light(vec2(4, 4), vec3(0.5, 0.5, 0.5)),
    Light(vec2(5, 4), vec3(0.5, 0.5, 0.5)),
    Light(vec2(6, 4), vec3(0.5, 0.5, 0.5)),
    Light(vec2(7, 4), vec3(0.5, 0.5, 0.5)),
    Light(vec2(8, 4), vec3(0.5, 0.5, 0.5)),
    Light(vec2(9, 4), vec3(0.5, 0.5, 0.5)),
    Light(vec2(0, 5), vec3(0.5, 0.5, 0.5)),
    Light(vec2(1, 5), vec3(0.5, 0.5, 0.5)),
    Light(vec2(2, 5), vec3(0.5, 0.5, 0.5)),
    Light(vec2(3, 5), vec3(0.5, 0.5, 0.5)),
    Light(vec2(4, 5), vec3(0.5, 0.5, 0.5)),
    Light(vec2(5, 5), vec3(0.5, 0.5, 0.5)),
    Light(vec2(6, 5), vec3(0.5, 0.5, 0.5)),
    Light(vec2(7, 5), vec3(0.5, 0.5, 0.5)),
    Light(vec2(8, 5), vec3(0.5, 0.5, 0.5)),
    Light(vec2(9, 5), vec3(0.5, 0.5, 0.5)),
    Light(vec2(0, 6), vec3(0.5, 0.5, 0.5)),
    Light(vec2(1, 6), vec3(0.5, 0.5, 0.5)),
    Light(vec2(2, 6), vec3(0.5, 0.5, 0.5)),
    Light(vec2(3, 6), vec3(0.5, 0.5, 0.5)),
    Light(vec2(4, 6), vec3(0.5, 0.5, 0.5)),
    Light(vec2(5, 6), vec3(0.5, 0.5, 0.5)),
    Light(vec2(6, 6), vec3(0.5, 0.5, 0.5)),
    Light(vec2(7, 6), vec3(0.5, 0.5, 0.5)),
    Light(vec2(8, 6), vec3(0.5, 0.5, 0.5)),
    Light(vec2(9, 6), vec3(0.5, 0.5, 0.5)),
    Light(vec2(0, 7), vec3(0.5, 0.5, 0.5)),
    Light(vec2(1, 7), vec3(0.5, 0.5, 0.5)),
    Light(vec2(2, 7), vec3(0.5, 0.5, 0.5)),
    Light(vec2(3, 7), vec3(0.5, 0.5, 0.5)),
    Light(vec2(4, 7), vec3(0.5, 0.5, 0.5)),
    Light(vec2(5, 7), vec3(0.5, 0.5, 0.5)),
    Light(vec2(6, 7), vec3(0.5, 0.5, 0.5)),
    Light(vec2(7, 7), vec3(0.5, 0.5, 0.5)),
    Light(vec2(8, 7), vec3(0.5, 0.5, 0.5)),
    Light(vec2(9, 7), vec3(0.5, 0.5, 0.5)),
    Light(vec2(0, 8), vec3(0.5, 0.5, 0.5)),
    Light(vec2(1, 8), vec3(0.5, 0.5, 0.5)),
    Light(vec2(2, 8), vec3(0.5, 0.5, 0.5)),
    Light(vec2(3, 8), vec3(0.5, 0.5, 0.5)),
    Light(vec2(4, 8), vec3(0.5, 0.5, 0.5)),
    Light(vec2(5, 8), vec3(0.5, 0.5, 0.5)),
    Light(vec2(6, 8), vec3(0.5, 0.5, 0.5)),
    Light(vec2(7, 8), vec3(0.5, 0.5, 0.5)),
    Light(vec2(8, 8), vec3(0.5, 0.5, 0.5)),
    Light(vec2(9, 8), vec3(0.5, 0.5, 0.5)),
    Light(vec2(0, 9), vec3(0.5, 0.5, 0.5)),
    Light(vec2(1, 9), vec3(0.5, 0.5, 0.5)),
    Light(vec2(2, 9), vec3(0.5, 0.5, 0.5)),
    Light(vec2(3, 9), vec3(0.5, 0.5, 0.5)),
    Light(vec2(4, 9), vec3(0.5, 0.5, 0.5)),
    Light(vec2(5, 9), vec3(0.5, 0.5, 0.5)),
    Light(vec2(6, 9), vec3(0.5, 0.5, 0.5)),
    Light(vec2(7, 9), vec3(0.5, 0.5, 0.5)),
    Light(vec2(8, 9), vec3(0.5, 0.5, 0.5)),
    Light(vec2(9, 9), vec3(0.5, 0.5, 0.5))
);

uniform Circle[] circles = Circle[] (
    Circle(vec2(1, 2), 0.1),
    Circle(vec2(2, 2), 0.2),
    Circle(vec2(3, 2), 0.3),
    Circle(vec2(4, 2), 0.2),
    Circle(vec2(5, 2), 0.1)
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
        float intensity = 1.0 / (4 * M_PI * distanceToLight);

        colorAtPixel += lights[i].color * intensity;
    }
    
    return colorAtPixel;
}

void main()
{
    camera.position = vec2 (0, time * 4);
    
    vec3 colorAtPixel = Trace(ToWorldSpace(screenPosition) + camera.position);
    FragColor = vec4(colorAtPixel, 1);
}