using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class JoystickValueReader : MonoBehaviour
{
    [Header("Utility")]
    [SerializeField] private TextMeshProUGUI forwardTMP;
    [SerializeField] private TextMeshProUGUI backwardTMP;
    [SerializeField] private TextMeshProUGUI resetTMP;

    [Header("Left Joystick")]
    [SerializeField] private TextMeshProUGUI xLeftTMP;
    [SerializeField] private TextMeshProUGUI yLeftTMP;

    [Header("Right Joystick")]
    [SerializeField] private TextMeshProUGUI xRightTMP;
    [SerializeField] private TextMeshProUGUI yRightTMP;

    [Header("D-Pad")]
    [SerializeField] private TextMeshProUGUI xDpadTMP;
    [SerializeField] private TextMeshProUGUI yDpadTMP;

    public void OnLeftStick(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        xLeftTMP.text = "X: " + value.x.ToString("F2");
        yLeftTMP.text = "Y: " + value.y.ToString("F2");
    }

    public void OnRightStick(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        xRightTMP.text = "X: " + value.x.ToString("F2");
        yRightTMP.text = "Y: " + value.y.ToString("F2");
    }

    public void OnDPad(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        xDpadTMP.text = "X: " + value.x.ToString("F2");
        yDpadTMP.text = "Y: " + value.y.ToString("F2");
    }

    public void OnForward(InputAction.CallbackContext context)
    {
        if (context.performed)
            forwardTMP.text = "Forward Button Pressed";
        if (context.canceled)
            forwardTMP.text = "Forward Button Released";
    }

    public void OnBackward(InputAction.CallbackContext context)
    {
        if (context.performed)
            backwardTMP.text = "Backward Button Pressed";
        if (context.canceled)
            backwardTMP.text = "Backward Button Released";
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        if (context.performed)
            resetTMP.text = "Reset Button Pressed";
        if (context.canceled)
            resetTMP.text = "Reset Button Released";
    }
}
