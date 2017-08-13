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

		var targetPos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var targetPos = new Vector2(targetPos3.x, targetPos3.y);

		myRB.velocity = (targetPos.normalized * DAGGER_SPEED);
	}

}
