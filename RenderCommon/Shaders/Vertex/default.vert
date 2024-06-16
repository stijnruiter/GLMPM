#version 330 core
in vec2 vertices;
uniform mat4 model;
uniform mat4 view;

void main()
{
    gl_Position = view * model * vec4(vertices, 0.0, 1.0);
}