using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class SetTeam : NetworkBehaviour {

	private bool isFFA = false;
	private bool is2Team = true;	
	private GameObject[] players;

	[SyncVar(hook = "SetTeamNum")] public int team;

	void Start()
	{
		// Return if not local player
		if (!isLocalPlayer)
			return;

		// Get number of players
		players = GameObject.FindGameObjectsWithTag("Player");		

		// If FFA...
		if (isFFA)
		{
			CmdSetTeam(players.Length);
			team = players.Length;
		}

		// If 2 Teams...
		else if (is2Team)
		{
			Debug.Log("sdfsdf" + players.Length % 2);
			if (players.Length % 2 == 1)
			{
				CmdSetTeam(1);
				team = 1;
			} else
			{
				CmdSetTeam(2);
				team = 2;
			}
				
		}
	}

	[Command] private void CmdSetTeam(int teamNum)
	{
		team = teamNum;
		Debug.Log("teamNum="+teamNum);
		Debug.Log("team="+team);
	}

	private void SetTeamNum(int teamNum)
	{
		if (!isServer)
			return;

		RpcSetTeam(teamNum);
	}

	[ClientRpc] private void RpcSetTeam(int teamNum)
	{
		// If Not Local Player, Update Team On Client...
		if (isLocalPlayer)
			return;

		Debug.Log("Changing remote player team to " + teamNum);
		team = teamNum;
	}
}
