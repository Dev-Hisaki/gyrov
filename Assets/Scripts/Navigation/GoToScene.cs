using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToScene : MonoBehaviour
{
    [SerializeField] private Toggle stationary, vr;

    private void GoTo(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void GoToMode()
    {
        if (stationary.isOn == true)
        {
            // Index for stationary Mode
            GoTo(1);
        }
        else if (vr.isOn == true)
        {
            // Index for VR Mode
            GoTo(2);
        }
        else
        {
            Debug.LogWarning("GoToScene: something was wrong.");
        }
    }
}
