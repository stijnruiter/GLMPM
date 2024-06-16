#version 330 core

in vec2 vertices;
in vec3 particle;

uniform mat4 view;

void main()
{
    gl_Position = view * vec4((vertices * particle.z + particle.xy), 0.0, 1.0);
}