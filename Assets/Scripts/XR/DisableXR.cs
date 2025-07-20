using UnityEngine;
using UnityEngine.XR.Management;
using System.Collections;

public class DisableXR : MonoBehaviour
{
    IEnumerator Start()
    {
        if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
        {
            XRManagerSettings xrManager = XRGeneralSettings.Instance.Manager;

            if (xrManager.activeLoader != null)
            {
                xrManager.StopSubsystems();
                xrManager.DeinitializeLoader();
                Debug.Log("XR successfully stopped and deinitialized.");
            }
            else
            {
                Debug.Log("XR Loader not active, nothing to stop.");
            }
        }
        else
        {
            Debug.LogWarning("XRGeneralSettings or Manager is null. Make sure XR is enabled in this project.");
        }

        yield return null;
    }
}
