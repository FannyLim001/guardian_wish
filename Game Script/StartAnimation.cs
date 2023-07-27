using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartAnimation : MonoBehaviour
{
    TextMeshProUGUI text;
    public float fadeDuration = 1f; // Duration for fading in and out
    // Start is called before the first frame update
    void Start()
    {
        // Fade in the text.
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        while (true)
        {
            // Fade in the text
            text.CrossFadeAlpha(1f, fadeDuration, true);
            yield return new WaitForSeconds(fadeDuration);

            // Fade out the text
            text.CrossFadeAlpha(0f, fadeDuration, true);
            yield return new WaitForSeconds(fadeDuration);
        }
    }
}
