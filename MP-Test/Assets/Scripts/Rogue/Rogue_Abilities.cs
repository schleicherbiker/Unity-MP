using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Rogue_Abilities : NetworkBehaviour {

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Return If Not Local Player
		if (!isLocalPlayer)
            return;

		HandleInput();
	}

	void HandleInput()
	{
		// First Move
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Debug.Log("First move!");
		}

		// Second Move
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Dash();
			Debug.Log("Second move!");
		}

		// Third Move
		if (Input.GetKeyDown(KeyCode.Alpha3)) 
		{
			ThrowDagger();
			Debug.Log("Third move!");
		}

		// Ultimate Move
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			Debug.Log("Ultimate move!");
		}
	}

	void Dash()
	{

	}


	void ThrowDagger()
	{

	}
}
