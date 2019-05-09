using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public GameObject menu;
    public GameObject serverMenu;
    public GameObject waitingMenu;
    public InputField hostIpField;
    public InputField clientNameField;

    public GameObject serverPrefab;
    public GameObject clientPrefab;

    string userId;

	// Use this for initialization
	void Start () {
        instance = this;
        serverMenu.SetActive(false);
        waitingMenu.SetActive(false);
        Application.runInBackground = true;
        DontDestroyOnLoad(gameObject);
        userId = PlayerPrefsManager.GetUserName();
        if (userId != "")
            clientNameField.text = userId;
	}

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            SceneManager.LoadScene(0);
    }

    public void ConnectButton()
    {
        menu.SetActive(false);
        serverMenu.SetActive(true);
    }

    public void HostButton()
    {
        menu.SetActive(false);
        waitingMenu.SetActive(true);

        try
        {
            Server server = Instantiate(serverPrefab).GetComponent<Server>();
            server.Init();
            Client client = Instantiate(clientPrefab).GetComponent<Client>();
            client.clientName = clientNameField.text;
            client.isHost = true;
            if (client.clientName == "")
                client.clientName = "Host";
            client.ConnectToServer("localhost", 6321);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void ConnectToServer()
    {
        string hostAddress = hostIpField.text;
        if (hostAddress == "")
            hostAddress = "localhost";

        try
        {
            Client client = Instantiate(clientPrefab).GetComponent<Client>();
            client.clientName = clientNameField.text;
            if (client.clientName == "")
                client.clientName = "Client";
            client.ConnectToServer(hostAddress, 6321);
            serverMenu.SetActive(false);
        }catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void BackButton()
    {
        Server server=GameObject.FindObjectOfType<Server>();
        if (server != null)
            Destroy(server.gameObject);
        Client client = GameObject.FindObjectOfType<Client>();
        if (client != null)
            Destroy(client.gameObject);
        waitingMenu.SetActive(false);
        serverMenu.SetActive(false);
        menu.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("OnlineMain");
    }

    public void UpdateUserName()
    {
        PlayerPrefsManager.SetUserName(clientNameField.text);
    }
}
