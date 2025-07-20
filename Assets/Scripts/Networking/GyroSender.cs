using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;

public class GyroSender : MonoBehaviour
{
    private UdpClient client;
    private NetworkStream stream;
    private Thread sendThread;
    private bool isRunning = true;

    public string esp32IP = "192.168.4.1"; // Ganti dengan IP ESP32
    public int port = 5000; // Sesuaikan dengan port di ESP32

    void Start()
    {
        // Mulai koneksi UDP dalam thread terpisah
        sendThread = new Thread(SendData);
        sendThread.IsBackground = true;
        sendThread.Start();
    }

    void SendData()
    {
        try
        {
            Debug.Log($"Connecting to {esp32IP} with {port} port");
            client = new UdpClient(esp32IP, port);
            // stream = client.GetStream();
            Debug.Log("Connected to ESP32");

            while (isRunning)
            {
                Vector3 rotationRate = GyroscopeReader.rotationRate;

                // Buat string JSON untuk dikirim
                string jsonData = string.Format(CultureInfo.InvariantCulture, "{{\"x\":{0:0.00},\"y\":{1:0.00}}}\n", rotationRate.x, rotationRate.y);

                // Ubah string ke byte array dan kirim ke ESP32
                byte[] data = Encoding.UTF8.GetBytes(jsonData);
                // stream.Send(data, 0, data.Length);

                Thread.Sleep(50); // Kirim data setiap 50ms
            }
        }
        catch (SocketException ex)
        {
            Debug.LogError("Socket Exception: " + ex.Message);
        }
        finally
        {
            CloseConnection();
        }
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        CloseConnection();
    }

    void CloseConnection()
    {
        if (stream != null) stream.Close();
        if (client != null) client.Close();
        Debug.Log("Connection closed");
    }
}
