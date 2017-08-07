using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
 
public class NetworkManagerNew : NetworkManager {
 
	void Start()
	{
        SetupButtons();
        NetworkManager.singleton.networkAddress = "2602:304:6824:c0e0:994f:cfae:f45e:1e8a";//"70.130.76.14";
        NetworkManager.singleton.networkPort = 4040;
	}


    void StartHost()
    {
        Debug.Log(Network.player.ipAddress);
        Debug.Log(Network.player.externalIP);
        Debug.Log(Network.player.port);
        Debug.Log(Network.player.externalPort);
        NetworkManager.singleton.StartHost();
    }

    void JoinGame()
    {  
        Debug.Log("Address: " + NetworkManager.singleton.networkAddress);
        Debug.Log("Port: " + NetworkManager.singleton.networkPort);
        NetworkManager.singleton.StartClient();
    }
    void StartServer(){}
 
    /*public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }
 
    public void SetIPAddress()
    {
        string ipAddress = GameObject.Find("InputFieldIPAddress").transform.FindChild("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }
 
    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }
 
    void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
            //SetupMenuSceneButtons();
            StartCoroutine(SetupMenuSceneButtons());
        }else
        {
            SetupOtherSceneButtons();
        }
	}*/
 
    void SetupButtons()
    {
        GameObject.Find("ServerButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ServerButton").GetComponent<Button>().onClick.AddListener(StartServer);

        GameObject.Find("HostButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("HostButton").GetComponent<Button>().onClick.AddListener(StartHost);

        GameObject.Find("JoinButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("JoinButton").GetComponent<Button>().onClick.AddListener(JoinGame);
    }
 
    /*void SetupOtherSceneButtons()
   	{
        GameObject.Find("DisconnectButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("DisconnectButton").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }*/
}