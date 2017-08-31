using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Rogue_Abilities : NetworkBehaviour {

	// Serialized Vars
	

	// Movement Vars
	private Rigidbody2D myRB; 

	// Ability Vars
	public bool isBusy;

	// Use this for initialization	
	void Start ()
	{
		isBusy = false;
		myRB = GetComponent<Rigidbody2D>();
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
		
	}

	

		

	
}
