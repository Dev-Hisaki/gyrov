using System;
using UnityEngine;

public static class LeanTweenFadeHelper
{
    /// <summary>
    /// Fade alpha CanvasGroup dari 'from' ke 'to' dalam 'duration' detik, lalu panggil onComplete.
    /// </summary>
    public static void FadeCanvasGroup(GameObject target, float from, float to, float duration, LeanTweenType easeType, Action onComplete = null)
    {
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = target.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = from;
        LeanTween.value(target, from, to, duration)
            .setEase(easeType)
            .setOnUpdate((float value) =>
            {
                canvasGroup.alpha = value;
            })
            .setOnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    /// <summary>
    /// Fade out GameObject lalu disable setelah selesai.
    /// </summary>
    public static void FadeOutAndDisable(GameObject target, float duration, LeanTweenType easeType, Action onComplete = null)
    {
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = target.AddComponent<CanvasGroup>();
        }

        LeanTween.value(target, canvasGroup.alpha, 0f, duration)
            .setEase(easeType)
            .setOnUpdate((float value) =>
            {
                canvasGroup.alpha = value;
            })
            .setOnComplete(() =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                onComplete?.Invoke();
                target.SetActive(false);
            });
    }
}
