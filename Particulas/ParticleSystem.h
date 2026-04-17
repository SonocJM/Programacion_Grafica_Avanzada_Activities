#pragma once
#define DLLEXPORT __declspec(dllexport)

extern "C" {
    struct Particle {
        float x, y, z;          // Posición
        float vy;               // Velocidad vertical
        float life;             // Vida
        float angle;            // Giro
        float radius;           // Radio
        float rotationSpeed;    // Velocidad de giro
    };

    DLLEXPORT void InitParticles(int count);
    DLLEXPORT void UpdateParticles(float deltaTime, float speed);
    DLLEXPORT Particle* GetParticles();
    DLLEXPORT int GetParticleCount();
}