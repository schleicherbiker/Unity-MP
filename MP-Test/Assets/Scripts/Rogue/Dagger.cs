using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour {

	public float SPIN_SPEED;
    public int team;
    
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
                //Destroy(gameObject);
            }
        }
        else
            Destroy(gameObject);
        

        // Else if colliding with terrain...
    }

	void FixedUpdate()
	{
		 	transform.Rotate(0, 0, SPIN_SPEED * Time.deltaTime);
	}
}
