#include "ParticleSystem.h"
#include <vector>
#include <cmath>
#include <cstdlib>

std::vector<Particle> particles;
int particleCount = 0;

extern "C" {
    void InitParticles(int count) {
        particles.clear();
        particleCount = count;
        for (int i = 0; i < count; ++i) {
            Particle p;
            p.angle = ((float)rand() / RAND_MAX) * 2.0f * 3.14159f;
            p.radius = ((float)rand() / RAND_MAX) * 1.5f + 0.5f;
            p.rotationSpeed = 1.0f + ((float)rand() / RAND_MAX) * 2.0f;
            p.vy = 0.5f + ((float)rand() / RAND_MAX) * 1.0f;
            
            // IMPORTANTE: Vida aleatoria inicial para que la espiral esté "llena" desde el inicio
            p.life = ((float)rand() / RAND_MAX) * 5.0f;
            
            // Calculamos posición inicial basada en esa vida
            float timePassed = 5.0f - p.life;
            p.y = p.vy * timePassed;
            float currentAngle = p.angle + (p.rotationSpeed * timePassed);
            p.x = p.radius * cosf(currentAngle);
            p.z = p.radius * sinf(currentAngle);

            particles.push_back(p);
        }
    }

    void UpdateParticles(float deltaTime, float speed) {
        for (auto& p : particles) {
            p.life -= deltaTime * speed;

            if (p.life <= 0) {
                p.life = 5.0f;
                p.y = 0;
                // Opcional: cambiar radio o ángulo al reiniciar
            }

            // Movimiento
            p.angle += p.rotationSpeed * deltaTime * speed;
            p.y += p.vy * deltaTime * speed;
            
            // Aplicar trigonometría
            p.x = p.radius * cosf(p.angle);
            p.z = p.radius * sinf(p.angle);
        }
    }

    Particle* GetParticles() { return particles.data(); }
    int GetParticleCount() { return particleCount; }
}