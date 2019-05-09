using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;

public class Server : MonoBehaviour {
    public int port = 6321;

    List<ServerClient> clients;
    List<ServerClient> disconnectList;

    TcpListener server;
    bool serverStarted;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();
        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
        }
        catch(Exception e)
        {
            Debug.Log("Error :"+e.Message);
        }
    }

    private void Update()
    {
        if (!serverStarted)
            return;
        foreach(ServerClient c in clients)
        {
            if(!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if(s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if(data!=null)
                    {
                        OnIncomingData(c,data);
                    }
                }
            }
        }

        int disconnectedPlayers = disconnectList.Count;
        for(int i=disconnectedPlayers-1; i>=0; i--)
        {
            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }
    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    void AcceptTcpClient(IAsyncResult ar)
    {
        string info = "SWHO|";
        foreach (ServerClient c in clients)
        {
            info += c.clientName + "|";
        }
        TcpListener listener = (TcpListener)ar.AsyncState;
        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);
        StartListening();
        Broadcast(info, clients[clients.Count - 1]);
    }

    bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    void Broadcast(string data, List<ServerClient> cl)
    {
        foreach( ServerClient c in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch(Exception e)
            {
                Debug.Log("Write Error: " + e.Message);
            }
        }
    }

    void Broadcast(string data, ServerClient c)
    {
        try
        {
            StreamWriter writer = new StreamWriter(c.tcp.GetStream());
            writer.WriteLine(data);
            writer.Flush();
        }
        catch (Exception e)
        {
            Debug.Log("Write Error: " + e.Message);
        }
    }

    void OnIncomingData(ServerClient c, string d)
    {
        string[] parsedData = d.Split('|');
        switch (parsedData[0])
        {
            case "CWHO":
                c.clientName = parsedData[1];
                c.isHost = (parsedData[2] == "0") ? false : true;
                Broadcast("SCNN|" + c.clientName, clients);
                break;
            case "CMOV":
                string msg= d.Replace('C', 'S');
                Broadcast(msg, clients);
                break;
        }
    }
}

public class ServerClient
{
    public string clientName;
    public bool isHost;
    public TcpClient tcp;

    public ServerClient(TcpClient tcp)
    {
        this.tcp = tcp;
    }
}
