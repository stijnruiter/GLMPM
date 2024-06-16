#version 330 core

in vec2 vertices;
in vec3 particle;

uniform mat4 projection;

out vec2 center;
out float radius;

void main()
{
    gl_Position = projection * vec4((vertices * particle.z + particle.xy), 0.0, 1.0);
    center = 0.5 * (projection * vec4(particle.xy, 0.0, 1.0)).xy + vec2(0.5, 0.5); // Transforom from [-1, 1] to [0, 1]
    radius = 0.25 * particle.z * projection[0][0]; // particle.z is diameter, and transform [-1, 1] to [0, 1] => radius = diam / 2 / 2
}