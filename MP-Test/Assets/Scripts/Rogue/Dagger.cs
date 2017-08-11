using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour {

	[SerializeField] private float DAGGER_SPEED;
	private Rigidbody2D myRB; 
	private BoxCollider2D myCollider;

	void Start ()
	{
		myRB = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D>();

		myRB.velocity = new Vector2(DAGGER_SPEED, 0);
	}

}
