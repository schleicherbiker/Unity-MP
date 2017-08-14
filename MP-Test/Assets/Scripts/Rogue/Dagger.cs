using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour {

	public float SPIN_SPEED;
	private bool facingRight;

	void Start ()
	{
		// if (transform.parent != null)
		// {
		// 	GameObject player = transform.parent.gameObject;
		// 	Debug.Log(player.GetComponent<Rogue_Movement>().facingRight);
		// } else {
		// 	Debug.Log(transform.parent);
		// }

	}

	void FixedUpdate()
	{
		 	transform.Rotate(0, 0, SPIN_SPEED * Time.deltaTime);
	}
}
