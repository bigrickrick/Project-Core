using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class DamageRecieveScreenEffect : MonoBehaviour
{
    public float initialIntensity = 1.0f; // Initial intensity value
    private float intensity; // Current intensity value

    public PostProcessVolume volume;
    public Vignette vignette;

    private void Start()
    {
        volume.profile.TryGetSettings(out vignette);
        vignette.enabled.Override(false);
        intensity = initialIntensity; // Set the initial intensity
    }

    public void TakeDamageEffect()
    {
        StartCoroutine(ApplyDamageEffect());
    }

    private IEnumerator ApplyDamageEffect()
    {
        vignette.enabled.Override(true);

        yield return new WaitForSeconds(0.2f);

        while (intensity > 0)
        {
            intensity -= 0.1f;

            if (intensity < 0) intensity = 0;
            vignette.intensity.Override(intensity);

            yield return new WaitForSeconds(0.05f);
        }

        vignette.enabled.Override(false);
        intensity = initialIntensity; // Reset the intensity for next use
    }

}
