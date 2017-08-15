using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class SetLayer : NetworkBehaviour {

	[SyncVar(hook = "SetNewLayer")] private int layer;

	void Start ()
	{
		gameObject.layer = layer;

		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		layer = LayerMask.NameToLayer("Player" + players.Length.ToString());

		if (isLocalPlayer)
		{
			gameObject.layer = layer;
			CmdSetLayer(layer);
		}
			
	}

	[Command] private void CmdSetLayer(int layer)
	{
		gameObject.layer = layer;
	}

	private void SetNewLayer(int layer)
	{
		if (!isServer)
			return;

		RpcSetNewLayer(layer);
	}

	[ClientRpc] private void RpcSetNewLayer(int layer)
	{
		// If Not Local Player, Update Layer On Client...
		if (isLocalPlayer)
			return;

		Debug.Log("Changing player layer to " + layer);
		gameObject.layer = layer;
	}
}
