using System.Collections;
using UnityEngine;

public class ShowMainMenu : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField] private GameObject startupCanvas;
    [SerializeField] private GameObject mainMenuCanvas;

    [Header("Image References")]
    [SerializeField] private GameObject mainTitleImage;
    [SerializeField] private GameObject bg1Image;
    [SerializeField] private GameObject bg2Image;

    private CanvasGroup _startupCanvasGroup;
    private CanvasGroup _mainMenuCanvasGroup;
    private Transform _mainTitle;
    private Transform _bg1;
    private Transform _bg2;
    private bool _canTouch = true;

    private void Awake()
    {
        if (startupCanvas == null || mainMenuCanvas == null || mainTitleImage == null || bg1Image == null || bg2Image == null)
        {
            Debug.LogError("ShowMainMenu: Ada reference yang belum di-assign di Inspector.");
        }
    }

    private void Start()
    {
        SetupCanvasGroups();
        CacheTransforms();
    }

    private void SetupCanvasGroups()
    {
        _startupCanvasGroup = GetOrAddCanvasGroup(startupCanvas);
        _mainMenuCanvasGroup = GetOrAddCanvasGroup(mainMenuCanvas);
        _mainMenuCanvasGroup.alpha = 0f; // Pastikan main menu invisible saat awal
        mainMenuCanvas.SetActive(false);
    }

    private void CacheTransforms()
    {
        _mainTitle = mainTitleImage.transform;
        _bg1 = bg1Image.transform;
        _bg2 = bg2Image.transform;
    }

    private CanvasGroup GetOrAddCanvasGroup(GameObject obj)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = obj.AddComponent<CanvasGroup>();
            Debug.LogWarning($"{obj.name} tidak punya CanvasGroup. CanvasGroup otomatis ditambahkan.");
        }
        return cg;
    }

    public void ShowMainMenuUI()
    {
        if (_canTouch)
        {
            StartCoroutine(TransitionToMainMenu());
        }
    }

    private IEnumerator TransitionToMainMenu()
    {
        _canTouch = false;

        // Fade out startup canvas
        LeanTweenFadeHelper.FadeOutAndDisable(startupCanvas, 1f, LeanTweenType.easeOutCubic, () =>
        {
            Debug.Log("Startup Canvas faded out and disabled.");
        });

        // Animasi gerak/zoom
        AnimateTransition(_mainTitle);
        AnimateTransition(_bg1);
        AnimateTransition(_bg2);

        // Aktifkan Main Menu
        mainMenuCanvas.SetActive(true);

        // Ini kunci: tunggu 1 frame!
        yield return null;

        _mainMenuCanvasGroup.alpha = 0f;

        LeanTweenFadeHelper.FadeCanvasGroup(mainMenuCanvas, 0f, 1f, 1f, LeanTweenType.easeInCubic, () =>
        {
            Debug.Log("Main Menu Appeared");
        });
    }

    private void AnimateTransition(Transform target)
    {
        LeanTween.moveLocalY(target.gameObject, target.localPosition.y + 100f, 1f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.scale(target.gameObject, target.localScale * 1f, 1f).setEase(LeanTweenType.easeOutCubic);
    }
}
