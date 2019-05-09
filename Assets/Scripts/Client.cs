using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;

public class Client : MonoBehaviour {
    public string clientName;
    public bool isHost = false;

    bool socketReady;
    TcpClient socket;
    NetworkStream stream;
    StreamWriter writer;
    StreamReader reader;

    List<GameClient> players = new List<GameClient>();
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool ConnectToServer(string host, int port)
    {
        if (socketReady)
            return false;

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        }
        catch(Exception e)
        {
            Debug.Log("Socket error: " + e.Message);
        }

        return socketReady;
    }
    private void Update()
    {
        if(socketReady)
        {
            if(stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }
        }
    }
    private void OnIncomingData(string data)
    {
        string[] parsedData = data.Split('|');
        switch(parsedData[0])
        {
            case "SWHO":
                for(int i=1; i<parsedData.Length-1; i++)
                {
                    UserConnected(parsedData[i], false);
                }
                Send("CWHO|" + clientName+"|"+((isHost)?1:0).ToString());
                break;
            case "SCNN":
                UserConnected(parsedData[1], false);
                break;
            case "SMOV":
                CheckerBoard.instance.TryMovePiece(int.Parse(parsedData[1]), int.Parse(parsedData[2]), int.Parse(parsedData[3]), int.Parse(parsedData[4]));
                break;
        }
    }

    private void UserConnected(string name, bool isHost)
    {
        GameClient c = new GameClient();
        c.name = name;
        c.isHost = isHost;
        players.Add(c);

        if (players.Count == 2)
            GameManager.instance.StartGame();
    }

    public void Send(string data)
    {
        if (!socketReady)
            return;
        writer.WriteLine(data);
        writer.Flush();
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }

    private void CloseSocket()
    {
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
}

public class GameClient
{
    public string name;
    public bool isHost;
}