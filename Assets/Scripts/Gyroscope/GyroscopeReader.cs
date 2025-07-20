using UnityEngine;
using TMPro;

public class GyroscopeReader : MonoBehaviour
{
    public TextMeshProUGUI gyroText;
    private Gyroscope gyro;
    public static Vector3 rotationRate { get; private set; }

    void Start()
    {
        if (gyroText == null) { 
            Debug.LogWarning("GyroText is not assigned in the inspector.");
            gyroText = null;
        }

        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
        else
        {
            gyroText.text = "Gyroscope not supported";
            Debug.LogWarning("Gyroscope not supported on this device.");
        }
    }

    void Update()
    {
        if (gyro != null && gyro.enabled)
        {
            rotationRate = gyro.rotationRateUnbiased;
            gyroText.text = $"Gyro Data:\nX: {rotationRate.x:F2}\nY: {rotationRate.y:F2}\nZ: {rotationRate.z:F2}";
        }
    }
}