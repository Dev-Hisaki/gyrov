using UnityEngine;
using UnityEngine.UI;

public class SingleToggleActived : MonoBehaviour
{
    [Header("Reference to Toggles")]
    [SerializeField] private Toggle stationaryToggle;
    [SerializeField] private Toggle vRToggle;

    private void Start()
    {
        if (stationaryToggle == null || vRToggle == null)
        {
            Debug.LogError("SingleToggleActived: Ada Toggle yang belum diset di Inspector.");
            return;
        }

        // Tambahkan listener dari script (bisa juga dari Inspector)
        stationaryToggle.onValueChanged.AddListener(OnStationaryToggleChanged);
        vRToggle.onValueChanged.AddListener(OnVRToggleChanged);
    }

    private void OnStationaryToggleChanged(bool isOn)
    {
        if (isOn)
        {
            vRToggle.isOn = false;
        }
    }

    private void OnVRToggleChanged(bool isOn)
    {
        if (isOn)
        {
            stationaryToggle.isOn = false;
        }
    }
}
