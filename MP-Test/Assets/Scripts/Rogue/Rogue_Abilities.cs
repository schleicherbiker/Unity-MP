﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Rogue_Abilities : NetworkBehaviour {

	// Serialized Vars
	[SerializeField] private GameObject Dagger;
	[SerializeField] private float DASH_TIME;
	[SerializeField] private float DASH_SPEED;

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
		isBusy = true;
		myRB.gravityScale = 0;
		myRB.velocity = new Vector2(10f, 10f);
		StartCoroutine(StopDash(DASH_TIME));
	}

	IEnumerator StopDash(float time)
	{
		yield return new WaitForSeconds(2f);
		isBusy = false;
		myRB.gravityScale = 2.5f;
	} 	

	void ThrowDagger()
	{
		Instantiate(Dagger, transform);
	}
}
