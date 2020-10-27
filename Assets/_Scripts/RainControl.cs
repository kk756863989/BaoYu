using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainControl : MonoBehaviour
{
    ParticleSystem particle;
    ParticleSystem.EmissionModule emission;
    ParticleSystem.SizeOverLifetimeModule size;
    ParticleSystem.MainModule main;
    
    void Start()
    {
        particle = transform.GetComponent<ParticleSystem>();
        emission = particle.emission;
        size = particle.sizeOverLifetime;
        main = particle.main;
    }

    
    void Update()
    {
        
    }

    public void setRainS()
    {
        emission.rateOverTime = 100;
        size.size = new ParticleSystem.MinMaxCurve(1, 1);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(225, 225, 225, 50));
    }

    public void setRainM()
    {
        emission.rateOverTime = 300;
        size.size = new ParticleSystem.MinMaxCurve(1, 2);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(225, 225, 225, 100));

    }

    public void setRainL()
    {
        emission.rateOverTime = 500;
        size.size = new ParticleSystem.MinMaxCurve(1, 3);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(225, 225, 225, 140));

    }
}
