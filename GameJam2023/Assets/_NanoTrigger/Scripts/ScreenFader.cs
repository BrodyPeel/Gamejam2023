using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image image;
    public float fadeDuration = 1f;

    private void Start()
    {
        // Ensure the fadeObject's shader _FullAlphaDissolveFade property is set to 1 at the start
        image.material.SetFloat("_FullAlphaDissolveFade", 1f);
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeTo(1f));
    }

    public void FadeFromBlack()
    {
        StartCoroutine(FadeTo(0f));
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = image.material.GetFloat("_FullAlphaDissolveFade");
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            image.material.SetFloat("_FullAlphaDissolveFade", alpha);
            yield return null;
        }
    }
}
