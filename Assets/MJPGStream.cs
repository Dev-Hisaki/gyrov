using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading;

public class MJPEGStream : MonoBehaviour
{
    public RawImage rawImage;
    [Tooltip("Ganti dengan IP Raspberry Pi")] public string streamURL = "http://192.168.1.100:8080/?action=stream";
    private Texture2D tex;
    private Thread streamThread;
    private bool running = true;

    void Start()
    {
        tex = new Texture2D(2, 2);
        rawImage.texture = tex;

        streamThread = new Thread(new ThreadStart(StreamMJPEG));
        streamThread.IsBackground = true;
        streamThread.Start();
    }

    void OnDestroy()
    {
        running = false;
        if (streamThread != null && streamThread.IsAlive)
            streamThread.Abort();
    }

    void StreamMJPEG()
    {
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(streamURL);
        req.Timeout = 10000;

        try
        {
            using (WebResponse resp = req.GetResponse())
            using (Stream stream = resp.GetResponseStream())
            {
                BinaryReader br = new BinaryReader(stream);
                while (running)
                {
                    // Cari header start JPEG
                    while (br.ReadByte() != 0xFF || br.ReadByte() != 0xD8) { }

                    MemoryStream imgStream = new MemoryStream();
                    imgStream.Write(new byte[] { 0xFF, 0xD8 }, 0, 2);

                    // Baca hingga akhir frame JPEG (FF D9)
                    while (true)
                    {
                        byte b = br.ReadByte();
                        imgStream.WriteByte(b);
                        if (b == 0xFF && br.PeekChar() == 0xD9)
                        {
                            imgStream.WriteByte(br.ReadByte());
                            break;
                        }
                    }

                    byte[] imageBytes = imgStream.ToArray();

                    // Update Texture di Unity Thread
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        tex.LoadImage(imageBytes);
                        rawImage.texture = tex;
                    });
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Stream Error: " + e.Message);
        }
    }
}
