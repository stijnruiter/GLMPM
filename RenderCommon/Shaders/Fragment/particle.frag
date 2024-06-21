#version 330 core
in vec2 center;
in float radius;

uniform vec2 resolution;
uniform vec4 color;

out vec4 FragColor;

void main()
{
    vec2 uv = gl_FragCoord.xy / resolution;
    vec2 size = (uv - center) * vec2(1.0, resolution.y / resolution.x);

    if (dot(size, size) < radius * radius)
    {
        FragColor = color;
    }
    else 
    {
        discard;
    }
}