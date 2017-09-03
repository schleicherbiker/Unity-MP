using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

	public const int maxHealth = 150;
	[SyncVar(hook="OnChangeHealth")] public int currentHealth = maxHealth;
	public int numLives = 3;
	public RectTransform healthBar;
	private float healthBarSize;
	private Vector3 healthBarScale;

	public void Start()
	{
		healthBarSize = healthBar.sizeDelta.x;
		healthBarScale = new Vector3(1, 1, 1);
	}

	void Update()
	{
		healthBar.transform.localScale = healthBarScale;
	}

	public void TakeDamage(int amount)
	{
		// Check if server...
		if (isServer)
		{	
			// Take Damage
			currentHealth -= amount;

			// Check health/lives
			if (currentHealth <= 0)
			{
				currentHealth = maxHealth; // Subtract health...
				RpcRespawn(); // Respawn...
				numLives--; // Subtract life...
				if (numLives == 0)
				{
					Debug.Log("Somebody wins!");
					// TODO Game reset code here...
				}
			}
		}


	}
	void OnChangeHealth(int currentHealth)
	{
		healthBar.sizeDelta = new Vector2(((float)currentHealth/(float)maxHealth)*healthBarSize, healthBar.sizeDelta.y);
	}

	[ClientRpc] void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			gameObject.transform.position = GameObject.Find("SpawnPoint" + gameObject.GetComponent<SetTeam>().team).transform.position;
		}
	}
}
