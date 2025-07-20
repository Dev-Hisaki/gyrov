using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    [Header("Here lies the help page")]
    [SerializeField] private GameObject mainMenuCanvas;

    [Header("and here is the private things")]
    private CanvasGroup _mainMenuCG;

    private void Start()
    {
        if (mainMenuCanvas == null)
        {
            Debug.Log("HelpButton: something is not assigned yet");
            return;
        }

        _mainMenuCG = mainMenuCanvas.GetComponent<CanvasGroup>();
    }

    public void ShowHelp(GameObject help)
    {
        CanvasGroup helpcg = help.GetComponent<CanvasGroup>();

        // Tambahkan CanvasGroup jika tidak ada
        if (helpcg == null)
        {
            helpcg = help.AddComponent<CanvasGroup>();
            Debug.LogWarning($"{help.name} tidak punya CanvasGroup. CanvasGroup otomatis ditambahkan.");
        }

        // Start showing help
        StartCoroutine(ShowHelpImage(help, helpcg));
    }

    private IEnumerator ShowHelpImage(GameObject help, CanvasGroup helpcg)
    {
        // Fade out startup canvas
        LeanTweenFadeHelper.FadeCanvasGroup(mainMenuCanvas, 1f, 0f, 1f, LeanTweenType.easeOutCubic, () =>
        {
            Debug.Log("Main Menu Canvas faded out.");
        });

        // Aktifkan Help Page
        helpcg.alpha = 0f;
        help.SetActive(true);

        // Ini kunci: tunggu 1 frame!
        yield return null;

        LeanTweenFadeHelper.FadeCanvasGroup(help, 0f, 1f, 1f, LeanTweenType.easeInCubic, () =>
        {
            mainMenuCanvas.SetActive(false);
            Debug.Log($"{help} Appeared");
        });
    }
}
