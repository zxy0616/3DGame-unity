using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSea : MonoBehaviour
{
    public ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particlesArray;

    public int seaResolution = 50;
    public float spacing = 0.5f;

    public float noiseScale = 0.2f;
    public float heightScale = 3f;

    private float perlinNoiseAnimX = 0.01f;
    private float perlinNoiseAnimY = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = ParticleSystem.Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/Halo"));
        particlesArray = new ParticleSystem.Particle[seaResolution * seaResolution];
        particleSystem.maxParticles = seaResolution * seaResolution;
        particleSystem.Emit(seaResolution * seaResolution);
        particleSystem.GetParticles(particlesArray);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < seaResolution; i++)
        {
            for (int j = 0; j < seaResolution; j++)
            {
                float zPos = Mathf.PerlinNoise(i * noiseScale + perlinNoiseAnimX, j * noiseScale + perlinNoiseAnimY) * heightScale;
                particlesArray[i * seaResolution + j].position = new Vector3(i * spacing, zPos, j * spacing);
            }
        }

        perlinNoiseAnimX += 0.01f;
        perlinNoiseAnimY += 0.01f;

        particleSystem.SetParticles(particlesArray, particlesArray.Length);
    }
}
