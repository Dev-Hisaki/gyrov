using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;

public class GyroSenderUDP : MonoBehaviour
{
    private UdpClient udpClient;
    private Thread sendThread;
    private bool isRunning = false;

    private string esp32IP;
    private int port;

    public void Initialize(string ip, int p)
    {
        esp32IP = ip;
        port = p;

        udpClient = new UdpClient();
        isRunning = true;

        sendThread = new Thread(SendData);
        sendThread.IsBackground = true;
        sendThread.Start();
    }

    void SendData()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(esp32IP), port);

        while (isRunning)
        {
            Vector3 rotationRate = GyroscopeReader.rotationRate;
            if (rotationRate == null) return;
            string jsonData = $"{{\"x\":{rotationRate.x.ToString("F2", CultureInfo.InvariantCulture)},\"y\":{rotationRate.y.ToString("F2", CultureInfo.InvariantCulture)}}}\n";
            byte[] data = Encoding.ASCII.GetBytes(jsonData);

            try
            {
                udpClient.Send(data, data.Length, endPoint);
            }
            catch (SocketException ex)
            {
                Debug.LogError("UDP Send Error: " + ex.Message);
            }

            Thread.Sleep(50);
        }
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        udpClient?.Close();
    }
}
