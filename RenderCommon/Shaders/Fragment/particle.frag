#version 330 core
in vec2 center;
in float radius;

uniform vec2 resolution;
uniform mat4 view;

out vec4 color;

void main()
{
    vec2 uv = gl_FragCoord.xy / resolution;
    vec2 size = (uv - center) * vec2(1.0, resolution.y / resolution.x);

    if (dot(size, size) < radius * radius)
    {
        color = vec4(0.1, 0.0, 1.0, 1.0);
    }
    else 
    {
        discard;
    }
}