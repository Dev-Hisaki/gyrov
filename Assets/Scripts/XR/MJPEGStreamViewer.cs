using System.Collections;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MJPEGStreamViewer : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private InputField raspiIpInputField;
    [SerializeField, Tooltip("Assigned Raspberry PI IP Address Here")] private string raspiIp;
    private string mjpegUrl;

    private Thread thread;
    private byte[] latestImageData;
    private Texture2D texture;
    private bool running = false;
    private readonly object dataLock = new object();

    private float lastUpdateTime = 0f;
    private float updateInterval = 0.1f; // 0.1s = 10 FPS

    public void StartStream()
    {
        if (raspiIpInputField != null)
        {
            raspiIp = raspiIpInputField.text;
        }

        mjpegUrl = "http://" + raspiIp + ":8080/?action=stream";

        if (rawImage == null)
        {
            Debug.LogError("RawImage is not assigned.");
            return;
        }

        texture = new Texture2D(2, 2);
        running = true;
        thread = new Thread(ReadStream);
        thread.Start();
    }

    void Update()
    {
        if (Time.time - lastUpdateTime < updateInterval) return;

        byte[] imageDataCopy = null;

        lock (dataLock)
        {
            if (latestImageData != null)
            {
                imageDataCopy = latestImageData;
                latestImageData = null;
            }
        }

        if (imageDataCopy != null)
        {
            texture.LoadImage(imageDataCopy);
            rawImage.texture = texture;
            lastUpdateTime = Time.time;
        }
    }

    void OnApplicationQuit()
    {
        running = false;
        if (thread != null && thread.IsAlive)
            thread.Abort();
    }

    void ReadStream()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mjpegUrl);
        request.Timeout = 10000;

        using (WebResponse response = request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        {
            var reader = new BinaryReader(stream);
            MemoryStream imageBuffer = new MemoryStream();

            byte[] jpegStart = new byte[] { 0xFF, 0xD8 };
            byte[] jpegEnd = new byte[] { 0xFF, 0xD9 };

            while (running)
            {
                try
                {
                    // Cari awal JPEG
                    byte prevByte = 0;
                    while (reader.ReadByte() != jpegStart[0] || reader.ReadByte() != jpegStart[1])
                    {
                        if (!running) return;
                    }
                    imageBuffer.WriteByte(jpegStart[0]);
                    imageBuffer.WriteByte(jpegStart[1]);

                    // Salin sampai akhir JPEG
                    while (true)
                    {
                        byte b = reader.ReadByte();
                        imageBuffer.WriteByte(b);

                        if (prevByte == jpegEnd[0] && b == jpegEnd[1])
                            break;

                        prevByte = b;
                    }

                    // Kirim ke main thread
                    lock (dataLock)
                    {
                        latestImageData = imageBuffer.ToArray();
                    }

                    imageBuffer.SetLength(0);
                }
                catch
                {
                    Debug.Log("There's something wrong...");
                    // Bisa tambahkan log di sini kalau mau debug
                }
            }
        }
    }
}
