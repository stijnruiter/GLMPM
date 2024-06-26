#version 330 core
in vec2 vertices;
uniform mat4 model;
uniform mat4 projection;

void main()
{
    gl_Position = projection * model * vec4(vertices, 0.0, 1.0);
}