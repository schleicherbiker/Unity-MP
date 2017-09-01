using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Dagger : NetworkBehaviour {

	[SyncVar] public float SPIN_SPEED;
    [SyncVar] public int team;
    
    void Start()
    {
        Debug.Log("testing");
    }

    void OnTriggerEnter2D(Collider2D col)
    {   
        // Test if colliding with a player...
        if (col.gameObject.GetComponent<Health>() != null)
        {
            // Check to see if colliding with player on opposite team
            if (team != col.gameObject.GetComponent<SetTeam>().team)
            {
                var health = col.gameObject.GetComponent<Health>();
                health.TakeDamage(10);
                Debug.Log(col.gameObject + " from team " + col.gameObject.GetComponent<SetTeam>().team + " has taken 10 damage from " + team);
                Destroy(gameObject);
            }
        }
        else // Else if colliding with terrain
            Destroy(gameObject);
    }

	void FixedUpdate()
	{
		transform.Rotate(0, 0, SPIN_SPEED * Time.deltaTime);
	}
}
