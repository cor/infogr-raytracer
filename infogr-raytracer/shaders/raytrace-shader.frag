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


//uniform Light[] lights = Light[] (
//    Light(vec2(0, 0), 4 * normalize(vec3(0, 0.75, 0.75))),
//    Light(vec2(4, 0), 4 * normalize(vec3(0, 1, 0))),
//    Light(vec2(8, 0), 4 * normalize(vec3(0.75, 1, 0))),
//    Light(vec2(0, 4), 4 * normalize(vec3(0, 0, 1))),
//    Light(vec2(4, 4), 4 * normalize(vec3(1, 1, 1))),
//    Light(vec2(8, 4), 4 * normalize(vec3(1, 1, 0))),
//    Light(vec2(0, 8), 4 * normalize(vec3(1, 0, 1))),
//    Light(vec2(4, 8), 4 * normalize(vec3(1, 0, 0))),
//    Light(vec2(8, 8), 4 * normalize(vec3(1, 0.75, 0)))
//);

uniform Light[] lights = Light[] (
    Light(vec2(0, 0), 4 * vec3(0, 0.75, 0.75)),
    Light(vec2(4, 0), 4 * vec3(0, 1, 0)),
    Light(vec2(8, 0), 4 * vec3(0.75, 1, 0)),
    Light(vec2(0, 4), 4 * vec3(0, 0, 1)),
    Light(vec2(4, 4), 4 * vec3(1, 1, 1)),
    Light(vec2(8, 4), 4 * vec3(1, 1, 0)),
    Light(vec2(0, 8), 4 * vec3(1, 0, 1)),
    Light(vec2(4, 8), 4 * vec3(1, 0, 0)),
    Light(vec2(8, 8), 4 * vec3(1, 0.75, 0))
);

Circle[] circles = Circle[] (
    Circle(vec2(2, 2), 0.1),
    Circle(vec2(3, 2), 0.2),
    Circle(vec2(4, 2), 0.3),
    Circle(vec2(5, 2), 0.2),
    Circle(vec2(6, 2), 0.1)
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

struct Rectangle {
    vec2[4] corners;
};

Rectangle newRectangle(in vec2 position, float width, float height, float angle) {
    vec2[4] corners = vec2[4](
        vec2(-width * 0.5, height * 0.5),
        vec2(width * 0.5, height * 0.5),
        vec2(width * 0.5, -height * 0.5),
        vec2(-width * 0.5, -height * 0.5)
    );
    
    if (angle != 0) {
        for (int i = 0; i < corners.length(); i++) {
            vec2 corner = corners[i];
            corners[i] = vec2(
                corner.x * cos(angle) - corner.y * sin(angle),
                corner.xy * sin(angle) + corner.y * cos(angle)
            );
        }
    }

    for (int i = 0; i < corners.length(); i++) {
        corners[i] += position;
    }
    
    return Rectangle(corners);
}

bool lineIntersects(Ray ray, vec2 lineStart, vec2 lineEnd) {
    vec2 rayStart = ray.origin;
    vec2 rayEnd = ray.origin + ray.direction;
    
    vec2 rayStoLineS = lineStart - rayStart;
    vec2 r = ray.direction * ray.magnitude;
    vec2 s = lineEnd - lineStart;
    
    float crossR = (rayStoLineS.x * r.y) - (rayStoLineS.y * r.x);
    float crossS = (rayStoLineS.x * s.y) - (rayStoLineS.y * s.x);
    float rCrossS = r.x * s.y - r.y * s.x;
    
    if (crossR == 0) {
        return ((lineStart.x - rayStart.x < 0) != (lineStart.x - rayEnd.x < 0)) || 
        ((lineStart.y - rayStart.y < 0) != (lineStart.y - rayEnd.y < 0));
    }
    
    if (rCrossS == 0) {return false;}
    
    float t = crossS / rCrossS;
    float u = crossR / rCrossS;
    
    return (t >= 0) && (t <= 1) && (u >= 0) && (u <= 1);
}

bool rectangleIntersect(Ray ray, Rectangle rect) {
    vec2[4] corners = rect.corners;
    return lineIntersects(ray, corners[0], corners[1]) ||
           lineIntersects(ray, corners[1], corners[2]) ||
           lineIntersects(ray, corners[2], corners[3]) ||
           lineIntersects(ray, corners[3], corners[0]);
}

Rectangle[] rectangles = Rectangle[](
    newRectangle(vec2(3,3), 1, 1, 0.4)
);

// Convert point from screen space to world space
vec2 ToWorldSpace(vec2 screenSpacePoint)
{
    float ratio = 100; // screen pixels per world space unit
    
    return screenSpacePoint * vec2(SCREEN_WIDTH, SCREEN_HEIGHT) / ratio; 
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
        
        for (int r = 0; r < rectangles.length(); r++) {
            Rectangle rect = rectangles[r];
            if (rectangleIntersect(ray, rect)) {
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
    
    for (int i = 0; i < circles.length(); i++)
    {
        circles[i].position += vec2(time, 0);
    }
    
    camera.position = vec2 (4, 4 + time);
    vec3 colorAtPixel = Trace(ToWorldSpace(screenPosition) + camera.position);
    FragColor = vec4(colorAtPixel, 1);
}