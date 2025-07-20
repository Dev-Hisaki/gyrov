using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationPanelAppearance : MonoBehaviour
{
    [SerializeField] private GameObject configurationPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void HideConfigurationPanel()
    {
        if (configurationPanel != null)
        {
            configurationPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Configuration panel is not assigned or found.");
        }
    }

    public void ShowConfigurationPanel()
    {
        if (configurationPanel != null)
        {
            configurationPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Configuration panel is not assigned or found.");
        }
    }
}
