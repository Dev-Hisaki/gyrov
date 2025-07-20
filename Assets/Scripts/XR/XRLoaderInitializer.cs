using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

public class XRLoaderInitializer : MonoBehaviour
{
    IEnumerator Start()
    {
        if (XRGeneralSettings.Instance == null)
        {
            Debug.LogError("XRGeneralSettings.Instance is null. Is XR Plug-in Management enabled?");
            yield break;
        }

        if (XRGeneralSettings.Instance.Manager == null)
        {
            Debug.LogError("XRGeneralSettings.Instance.Manager is null. Check your XR configuration.");
            yield break;
        }

        var xrManager = XRGeneralSettings.Instance.Manager;

        if (xrManager.activeLoader == null)
        {
            Debug.Log("Initializing XR...");
            yield return xrManager.InitializeLoader();

            if (xrManager.activeLoader == null)
            {
                Debug.LogError("Initializing XR Failed.");
                yield break;
            }
        }

        xrManager.StartSubsystems();
        Debug.Log("XR Started.");
    }
}
