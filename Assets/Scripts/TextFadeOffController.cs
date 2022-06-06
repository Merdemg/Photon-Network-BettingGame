using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFadeOffController : MonoBehaviour
{
    TMP_Text text = null;

    Coroutine fadeOffCoroutine = null;
    Coroutine showTextCoroutine = null;

    const float FADE_OFF_TIME = 1.5f;
    float fadeOffCounter = 0;

    const float TEXT_STAY_TIME = 3.0f;
    float textStayCounter = 0;

    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Calling this method when text is already visible is safe.
    /// It will reset the timer
    /// </summary>
    public void ShowTextThenFadeOff() {
        if (fadeOffCoroutine != null) {
            StopCoroutine(fadeOffCoroutine);
            fadeOffCoroutine = null;
        }

        textStayCounter = 0;
        if (showTextCoroutine == null) {
            showTextCoroutine = StartCoroutine(ShowTextCoroutine());
        }
    }

    /// <summary>
    /// Calling this method while text is visible will start the fade off process early.
    /// </summary>
    public void FadeOffOverTime() {
        if (fadeOffCoroutine == null) {
            if (showTextCoroutine != null) {
                StopCoroutine(showTextCoroutine);
                showTextCoroutine = null;
            }

            fadeOffCoroutine = StartCoroutine(FadeOffRoutine());
        }
    }

    /// <summary>
    /// Can be used to stop all show / fade off coroutines early and make the text disappear immediately
    /// </summary>
    public void FadeOffImmediate() {
        if (showTextCoroutine != null) {
            StopCoroutine(showTextCoroutine);
            showTextCoroutine = null;
        }
        if (fadeOffCoroutine != null) {
            StopCoroutine(fadeOffCoroutine);
            fadeOffCoroutine = null;
        }

        Color color = text.color;
        color.a = 0;
        text.color = color;
    }

    IEnumerator ShowTextCoroutine() {
        Color color = text.color;
        color.a = 1;
        text.color = color;

        while (textStayCounter < TEXT_STAY_TIME) {
            textStayCounter += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        showTextCoroutine = null;
        FadeOffOverTime();
    }

    IEnumerator FadeOffRoutine() {
        fadeOffCounter = 0;
        float startingAlpha = text.color.a;

        while (fadeOffCounter < FADE_OFF_TIME) {
            Color color = text.color;
            color.a = startingAlpha - (fadeOffCounter / FADE_OFF_TIME);
            text.color = color;

            fadeOffCounter += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        FadeOffImmediate();
    }
}
