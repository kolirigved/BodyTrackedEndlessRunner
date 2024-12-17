using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class MyListener : MonoBehaviour
{
    private Thread thread;
    private UdpClient client; // UDP client
    private bool isRunning = true;  // Flag to control thread termination
    public int connectionPort = 25001;
    private IPEndPoint remoteEndPoint; // End point of the client (from which we receive data)
    private string data;
    
    public float xpos;
    public float ypos;
    public float jumpstate;

    void Start()
    {
        thread = new Thread(new ThreadStart(GetData));
        thread.IsBackground = true; // Close the thread when the application is closed
        thread.Start();
    }

    void GetData()
    {
        try{
            client = new UdpClient(connectionPort); // Create a UDP client
            client.Client.ReceiveTimeout = 1000; // Set the client receive timeout to 1 second
            remoteEndPoint = new IPEndPoint(IPAddress.Any, connectionPort); // Any IP, same port
            Debug.Log("UDP server started, waiting for data...");

            while (isRunning)
            {
                try
                {
                    if(client.Available > 0) { // Check if there is data available
                        byte[] data = client.Receive(ref remoteEndPoint); // Receive data from the client
                        string text = Encoding.UTF8.GetString(data);
                        ParseData(text);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error in receiving data: " + e.Message);
                }

                Thread.Sleep(1); // Reduce CPU usage
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Server error: " + e.Message);
        }
        finally
        {
            Cleanup();
        }
    }

    void ParseData(string data)
    {
        // Input data coordinates are separated by spaces
        string[] values = data.Split(' ');
        float[] floatValues = new float[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            floatValues[i] = float.Parse(values[i]);
        }
        // we converted them to float values
        xpos = floatValues[0]-0.5f;
        ypos = floatValues[1]-0.5f;
        jumpstate = floatValues[2]-0.5f;
        
    }

    void OnApplicationQuit() // Stop the thread when the application is closing
    {
        isRunning = false;  // Stop the thread loop gracefully
        thread.Join();      // Wait for the thread to finish
        Cleanup();
    }

    void Cleanup() // Close the UDP server connection properly
    {
        if (client != null)
        {
            client.Close();
            client = null;
        }
    }
}
