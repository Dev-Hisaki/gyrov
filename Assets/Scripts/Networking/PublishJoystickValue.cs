/*
SCRIPT REQUIRED!
- M2MqttUnityClient
COMPONENT REQUIRED!
- Player Input
NOTE:
The action of the player input must be assigned first. Then assigned this script to the same object with the player input.
Assign all of the public function to the corresponding action in the action map.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using M2MqttUnity.Examples;

public class PublishJoystickValue : M2MqttUnityClient
{
    private string message = "";
    private string clientIP = "127.0.0.1";
    private string streamUrl = "";

    // Variabel untuk menyimpan nilai ROV movement
    private int horizontalLR = 0;   // Left (-1), Neutral (0), Right (1)
    private int horizontalFB = 0;   // Backward (-1), Neutral (0), Forward (1)
    private int verticalUD = 0;     // Down (-1), Neutral (0), Up (1)
    private int yaw = 0;            // Left (-1), Neutral (0), Right (1)

    // Variabel untuk menyimpan nilai kamera
    private int pan = 0;            // Neutral (0), Left (-1), Right (1)
    private int tilt = 0;           // Neutral (0), Up (1), Down (-1)
    private int cameraReset = 0;    // Reset camera (0 or 1)

    private Coroutine publishCoroutine;

    [SerializeField] private MJPEGStreamDecoder streamDecoder;

    public void StartPublishing()
    {
        if (publishCoroutine == null)
        {
            clientIP = M2MqttUnityCustom.Instance.brokerAddress;
            streamUrl = "http://" + clientIP + ":8080/?action=stream";
            streamDecoder.StartStream(streamUrl);
            publishCoroutine = StartCoroutine(PublishMessages());
            Debug.Log("Start Stream");
        }
    }

    private IEnumerator PublishMessages()
    {
        while (true)
        {
            message =
                $"{{\"fb\":{horizontalFB}," +
                $"\"lr\":{horizontalLR}," +
                $"\"ud\":{verticalUD}," +
                $"\"yaw\":{yaw}," +
                $"\"pan\":{pan}," +
                $"\"tilt\":{tilt}," +
                $"\"reset\":{cameraReset}}}";

            Debug.Log(message);

            if (M2MqttUnityCustom.Instance != null &&
                M2MqttUnityCustom.Instance.GetClient() != null &&
                M2MqttUnityCustom.Instance.GetClient().IsConnected)
            {
                Debug.Log("Publishing message to MQTT: " + message);
                M2MqttUnityCustom.Instance.GetClient().Publish(
                    "rov/control",
                    System.Text.Encoding.UTF8.GetBytes(message),
                    MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                    false
                );
            }
            else
            {
                Debug.LogWarning("MQTT client belum terhubung.");
            }

            // Reset trigger
            cameraReset = 0;

            yield return new WaitForSeconds(0.05f);
        }
    }

    #region Input Actions
    public void Forward(bool isPressed)
    {
        horizontalFB = isPressed ? 1 : 0;
        Debug.Log("Forward: " + horizontalFB);
    }

    public void Backward(bool isPressed)
    {
        horizontalFB = isPressed ? -1 : 0;
    }

    public void Up(bool isPressed)
    {
        verticalUD = isPressed ? 1 : 0;
    }

    public void Down(bool isPressed)
    {
        verticalUD = isPressed ? -1 : 0;
    }

    public void Left(bool isPressed)
    {
        horizontalLR = isPressed ? -1 : 0;
    }

    public void Right(bool isPressed)
    {
        horizontalLR = isPressed ? 1 : 0;
    }

    public void YawLeft(bool isPressed)
    {
        yaw = isPressed ? -1 : 0;
    }

    public void YawRight(bool isPressed)
    {
        yaw = isPressed ? 1 : 0;
    }

    public void TiltUp(bool isPressed)
    {
        tilt = isPressed ? 1 : 0;
    }

    public void TiltDown(bool isPressed)
    {
        tilt = isPressed ? -1 : 0;
    }

    public void PanRight(bool isPressed)
    {
        pan = isPressed ? 1 : 0;
    }

    public void PanLeft(bool isPressed)
    {
        pan = isPressed ? -1 : 0;
    }

    public void ResetCam(bool isPressed)
    {
        cameraReset = isPressed ? 1 : 0;
    }
    #endregion
}
