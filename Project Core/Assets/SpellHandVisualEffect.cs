using System.Collections;
using UnityEngine;

public class SpellHandVisualEffect : MonoBehaviour
{
    public ParticleSystem Particules;
    private ParticleSystem.MainModule mainModule;
    public float emissionRate;
    public float OriginalEmission;

    private void Start()
    {
        mainModule = Particules.main;
       
    }

    

    private IEnumerator AdjustParticles(bool increase)
    {
        var emission = Particules.emission;

        if (increase)
        {
            emission.rateOverTime = emissionRate;
        }
        else
        {
            emission.rateOverTime = OriginalEmission;
        }

        yield return null; 
        Debug.Log("Emission rate: " + emission.rateOverTime.constant);
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
