using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public float fadeTime = 2.0f;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Fade out the current scene audio.
        float volume = audioSource.volume;
        StartCoroutine(FadeOut(volume, fadeTime));
    }

    IEnumerator FadeOut(float volume, float fadeTime)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            volume -= 0.1f * Time.deltaTime;
            audioSource.volume = volume;
            yield return null;
        }
    }

    IEnumerator FadeIn(float volume, float fadeTime)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            volume += 0.1f * Time.deltaTime;
            audioSource.volume = volume;
            yield return null;
        }
    }
}
