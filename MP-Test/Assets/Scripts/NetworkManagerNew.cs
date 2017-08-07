using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
 
public class NetworkManager_Custom_V2 : NetworkManager {
 
	void Start()
	{
		SetupMenuSceneButtons();
	}

    public void StartupHost()
    {
		Debug.Log("Here");
        //SetPort();
        //Debug.Log(Network.player.ipAddress);
        //Debug.Log(Network.player.externalIP);
        //Debug.Log(Network.player.port);
        //Debug.Log(Network.player.externalPort);
        //NetworkManager.singleton.StartHost();
    }
 
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
 
    IEnumerator SetupMenuSceneButtons()
    {
        yield return new WaitForSeconds(0.15f);
        GameObject.Find("HostButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("HostButton").GetComponent<Button>().onClick.AddListener(StartupHost);
 
        //GameObject.Find("JoinButton").GetComponent<Button>().onClick.RemoveAllListeners();
        //GameObject.Find("JoinButton").GetComponent<Button>().onClick.AddListener(JoinGame);
    }
 
    /*void SetupOtherSceneButtons()
   	{
        GameObject.Find("DisconnectButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("DisconnectButton").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }*/
}