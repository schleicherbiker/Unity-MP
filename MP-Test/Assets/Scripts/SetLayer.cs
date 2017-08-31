using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class SetLayer : NetworkBehaviour {

	[SyncVar(hook = "SetNewLayer")] private int layer; // When local player determines correct layer, link to RPC tp update existing remote clients

	void Start ()
	{
		// If not local player, just assign layer based on the servers value for the other player
		if (!isLocalPlayer)
		{
			gameObject.layer = layer;
		}

		// If local player...
		if (isLocalPlayer)
		{
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // Determine correct layer
			layer = LayerMask.NameToLayer("Player" + players.Length.ToString()); 
			gameObject.layer = layer; // Assign correct layer
			CmdSetLayer(layer); // Update server to have correct value
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
		// If not local player, update layer of other player on your client
		if (!isLocalPlayer)
		{
			gameObject.layer = layer;
		}
	}
}
