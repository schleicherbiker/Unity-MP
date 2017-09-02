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

	public void Start()
	{
		healthBarSize = healthBar.sizeDelta.x;
	}

	public void TakeDamage(int amount)
	{
		// Check if server...
		if (!isServer)
			return;

		// Take Damage
		currentHealth -= amount;

		// Check health/lives
		if (currentHealth <= 0)
		{
			gameObject.transform.position = GameObject.Find("SpawnPoint" + gameObject.GetComponent<SetTeam>().team).transform.position;
			currentHealth = 0;
			numLives--;
			if (numLives == 0)
			{
				Debug.Log("Somebody wins!");
			}
		}
	}
	
	void OnChangeHealth(int currentHealth)
	{
		healthBar.sizeDelta = new Vector2(((float)currentHealth/(float)maxHealth)*healthBarSize, healthBar.sizeDelta.y);
	}
}
