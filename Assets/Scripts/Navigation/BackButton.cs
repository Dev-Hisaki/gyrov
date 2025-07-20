using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    [SerializeField] GameObject mainMenuCanvas, currentCanvasGroup;
    private CanvasGroup _mainMenuCG, _currentCG;

    private void Start()
    {
        _mainMenuCG = mainMenuCanvas.GetComponent<CanvasGroup>();
        _currentCG = gameObject.GetComponent<CanvasGroup>();
        if (_mainMenuCG == null) _mainMenuCG = mainMenuCanvas.AddComponent<CanvasGroup>();
        if (_currentCG == null) _currentCG = gameObject.AddComponent<CanvasGroup>();
    }

    public void StartBackToMainMenu()
    {
        StartCoroutine(BackToMainMenu());
    }

    private IEnumerator BackToMainMenu()
    {
        LeanTweenFadeHelper.FadeCanvasGroup(currentCanvasGroup, 1f, 0f, 1f, LeanTweenType.easeOutCubic, () =>
        {
            Debug.Log("Help dissappear.");
            _mainMenuCG.alpha = 0f;
            mainMenuCanvas.SetActive(true);
        });

        yield return new WaitForSeconds(1f);

        LeanTweenFadeHelper.FadeCanvasGroup(mainMenuCanvas, 0f, 1f, 1f, LeanTweenType.easeInCubic, () =>
        {
            Debug.Log("Main Menu Appear.");
        });
    }
}
