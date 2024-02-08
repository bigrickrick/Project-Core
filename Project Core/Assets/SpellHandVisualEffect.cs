using System.Collections;
using UnityEngine;

public class SpellHandVisualEffect : MonoBehaviour
{
    public ParticleSystem Particules;
    private ParticleSystem.MainModule mainModule;

    private void Start()
    {
        mainModule = Particules.main;
    }

    private IEnumerator AdjustParticles(bool increase)
    {
        while (increase)
        {
            mainModule.maxParticles = Mathf.Clamp(mainModule.maxParticles + (increase ? 10 : -10), 0, 1000);
            yield return null;
        }
    }

    public void StartIncreasingParticles()
    {
        StartCoroutine(AdjustParticles(true));
    }

    public void StartDecreasingParticles()
    {
        StartCoroutine(AdjustParticles(false));
    }

    public void StopAdjustingParticles()
    {
        StopAllCoroutines();
    }
}