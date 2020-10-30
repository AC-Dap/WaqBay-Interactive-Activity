using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPath : MonoBehaviour
{
    public ParticleSystem p;
    public LineRenderer line;

    private Vector3[] nodes;
    private Vector3[] pathVectors;
    private float lenOfPath;

    // Start is called before the first frame update
    void Start()
    {
        nodes = new Vector3[line.positionCount];
        pathVectors = new Vector3[nodes.Length - 1];
        line.GetPositions(nodes);

        for (int i = 0; i < pathVectors.Length; i++)
        {
            pathVectors[i] = nodes[i + 1] - nodes[i];
            lenOfPath += pathVectors[i].magnitude;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[p.particleCount];
        p.GetParticles(particles);

        for (int i = 0; i < particles.Length; i++)
        {
            // Find which part of the path this particle is on
            float magTraveled = (1 - particles[i].remainingLifetime / particles[i].startLifetime) * lenOfPath;
            float mag = 0f;

            for (int j = 0; j < pathVectors.Length && mag <= magTraveled; j++)
            {
                particles[i].position = nodes[j] + (magTraveled - mag) * pathVectors[j].normalized;
                mag += pathVectors[j].magnitude;
            }
        }
        p.SetParticles(particles);
    }
}