using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Concurrent;
using System;

public class ConnectionManager : MonoBehaviour
{
    private readonly ConcurrentQueue<Action> mainThreadActions = new ConcurrentQueue<Action>();

    public TMP_InputField inputIp;
    public TMP_InputField inputPort;
    public Button connectButton;
    public TMP_Text statusText;

    private string esp32IP;
    private int esp32Port;

    void Start()
    {
        connectButton.onClick.AddListener(OnConnectClicked);
    }

    void Update()
    {
        while (mainThreadActions.TryDequeue(out var action))
        {
            action?.Invoke();
        }
    }

    void OnConnectClicked()
    {
        esp32IP = inputIp.text;
        if (!int.TryParse(inputPort.text, out esp32Port) || string.IsNullOrWhiteSpace(esp32IP))
        {
            ShowStatus("IP atau Port tidak valid", Color.red);
            return;
        }

        connectButton.interactable = false;
        inputIp.interactable = false;
        inputPort.interactable = false;

        ShowStatus("Mengatur koneksi UDP...", Color.yellow);
        Time.timeScale = 0f;

        GetComponent<GyroSenderUDP>().Initialize(esp32IP, esp32Port);

        ShowStatus("Koneksi UDP disiapkan!", Color.green);
        Time.timeScale = 1f;

        connectButton.interactable = true;
        inputIp.interactable = true;
        inputPort.interactable = true;
    }

    void ShowStatus(string message, Color color)
    {
        statusText.text = message;
        statusText.color = color;
    }

    void InvokeOnMainThread(Action action)
    {
        mainThreadActions.Enqueue(action);
    }
}
