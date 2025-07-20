using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamepadDetector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gamepadTMP;
    private bool connected = false;

    IEnumerator CheckForControllers()
    {
        while (true)
        {
            var controllers = Input.GetJoystickNames();

            if (!connected && controllers.Length > 0)
            {
                connected = true;
                gamepadTMP.text = "Joystick Status: Connected";
                Debug.Log("Connected");

            }
            else if (connected && controllers.Length == 0)
            {
                connected = false;
                gamepadTMP.text = "Joystick Status: Disconnected";
                Debug.Log("Disconnected");
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void Start()
    {
        StartCoroutine(CheckForControllers());
    }

}
