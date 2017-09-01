using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class SetTeam : NetworkBehaviour {

	private bool isFFA = false;
	private bool is2Team = true;	
	private GameObject[] players;
	[SyncVar(hook = "SetTeamNum")] public int team; // When local player determines correct team, link to RPC tp update existing remote clients

	void Start()
	{
		// If not local player, other player should have correct team already
		if (!isLocalPlayer)
			return;
		
		// If local player...
		if (isLocalPlayer)
		{
			players = GameObject.FindGameObjectsWithTag("Player"); // Get number of players

			// If it's Free For All...
			if (isFFA)
			{
				team = players.Length; // Update local players team...
				// Move player to correct spawn position
				if (team == 1)
					gameObject.transform.position = GameObject.Find("SpawnPoint1").transform.position;
				else
					gameObject.transform.position = GameObject.Find("SpawnPoint2").transform.position; 
				CmdSetTeam(players.Length); // Update server...
			}

			// Else if only 2 teams...
			else if (is2Team)
			{
				if (players.Length % 2 == 1) // Odd players get team 1...
				{
					team = 1;
					gameObject.transform.position = GameObject.Find("SpawnPoint1").transform.position;
					CmdSetTeam(1);
				} else // Even players are team two
				{
					team = 2;
					gameObject.transform.position = GameObject.Find("SpawnPoint2").transform.position;
					CmdSetTeam(2);
				}
			}
		}
	}

	[Command] private void CmdSetTeam(int teamNum)
	{
		team = teamNum;
	}

	private void SetTeamNum(int teamNum)
	{
		if (!isServer)
			return;

		RpcSetTeam(teamNum);
	}

	[ClientRpc] private void RpcSetTeam(int teamNum)
	{
		// If not local player, update team of other player on your client
		if (!isLocalPlayer)
		{
			team = teamNum;
		}
	}
}
