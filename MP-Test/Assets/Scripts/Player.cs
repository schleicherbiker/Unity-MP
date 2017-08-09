using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	
	// Character Attributes
	[SerializeField] private float MOVEMENT_SPEED;
	[SerializeField] private float JUMP_SPEED;

	[SyncVar(hook = "SetDir")] public bool facingRight;
	private float horizontal;

	// Character References
	private Rigidbody2D myRB; 
	private Animator myAnimator;
	private BoxCollider2D myCollider;

	// Jumping stuff?
	private bool attack;
	[SerializeField] private Transform[] groundCollider;
	[SerializeField] private float groundRadius;
	[SerializeField] private LayerMask whatIsGround;
	private bool isGrounded;
	private bool jump;
	private bool canDoubleJump;


	//public GameObject bulletPrefab;
	
	public override void OnStartLocalPlayer()
	{
		// Reference Components
		myRB = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D>();
		myAnimator = GetComponent<Animator>();

		// Setup Camera
		Vector3 playerPos = this.transform.position;
		playerPos.z = -1;
		Camera.main.transform.position = playerPos;
		Camera.main.transform.parent = this.transform;
	}

	void Update() {

		// Return If Not Local Player
		if (!isLocalPlayer)
            return;

		// Handle Input Every Frame
		HandleInput();
		Debug.Log(isGrounded);
	}

	void FixedUpdate()
	{
		// Return If Not Local Player
		if (!isLocalPlayer)
            return;

		// Get Horizontal Input and Handle Movement
		horizontal = Input.GetAxis("Horizontal");
		HandleMovement(horizontal);
		
		// TODO
		isGrounded = IsGrounded();
		HandleAttacks();
		ResetVars();
	}

	private void HandleMovement(float horizontal)
	{
		// If Attacking, Don't Move
		if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
			myRB.velocity = new Vector2(0, myRB.velocity.y);
		} 
		// Else Set Normal Movespeed
		else {
			myRB.velocity = new Vector2(horizontal * MOVEMENT_SPEED, myRB.velocity.y);
		}

		// Set Animator Speed
		myAnimator.SetFloat("speed", Mathf.Abs(horizontal));

		// Check for H Flips
		if (horizontal > 0)
		{
			if (transform.localScale.x == -1)
			{
				// Correct Direction Locally
				Vector3 playerScale = transform.localScale;
				playerScale.x = 1;
				transform.localScale = playerScale;

				// Update Server with [Command] Call
				CmdUpdateServerDir(true);
			}
		}
		else if (horizontal < 0)
		{
			if (transform.localScale.x == 1)
			{
				// Correct Direction Locally
				Vector3 playerScale = transform.localScale;
				playerScale.x = -1;
				transform.localScale = playerScale;

				// Update Server with [Command] Call
				CmdUpdateServerDir(false);
			}
		}

		if (Input.GetKey(KeyCode.Space)) {
			Jump();
		}
	}

	private void HandleAttacks() {
		if (attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
			myAnimator.SetTrigger("attack");
		}
	}

	private void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			attack = true;
		}

		if (Input.GetKey(KeyCode.Space)) {
			Jump();
		}
	}

	private void SetDir(bool facingRight)
	{
		// Return if Not Server
		if (!isServer)
			return;
		RpcSetDir(facingRight);
	}

	[ClientRpc] private void RpcSetDir(bool facingRight)
	{
		if (isLocalPlayer)
		// If Not Local Player, Update Direction On Client...
		if (!isLocalPlayer)
		{
			if (facingRight)
			{
				Vector3 playerScale = transform.localScale;
				playerScale.x = 1;
				transform.localScale = playerScale;
			}
			else
			{
				Vector3 playerScale = transform.localScale;
				playerScale.x = -1;
				transform.localScale = playerScale;
			}
		}
	}

	[Command] private void CmdUpdateServerDir(bool faceRight)
	{
		facingRight = faceRight;
	}

	private void ResetVars() {
		attack = false;
		jump = false;
	}

	private bool IsGrounded()
	{
		if (myRB.velocity.y <= 0)
		{
			Vector2 topLeft, bottomRight;


			if (transform.localScale.x == 1) {
				topLeft = new Vector2(myCollider.transform.position.x + myCollider.offset.x - (myCollider.size.x / 2f) + .1f, myCollider.transform.position.y + myCollider.offset.y - (myCollider.size.y / 2f));
				bottomRight = new Vector2(myCollider.transform.position.x + myCollider.offset.x + (myCollider.size.x / 2f) -.1f, myCollider.transform.position.y + myCollider.offset.y - (myCollider.size.y / 2f) - .01f);
				Debug.DrawLine(topLeft, bottomRight, Color.yellow);
			} else {
				topLeft = new Vector2(myCollider.transform.position.x - myCollider.offset.x - (myCollider.size.x / 2f) + .1f, myCollider.transform.position.y + myCollider.offset.y - (myCollider.size.y / 2f));
				bottomRight = new Vector2(myCollider.transform.position.x - myCollider.offset.x + (myCollider.size.x / 2f) -.1f, myCollider.transform.position.y + myCollider.offset.y - (myCollider.size.y / 2f) - .01f);
				Debug.DrawLine(topLeft, bottomRight, Color.yellow);
			}
			

			Collider2D[] colliders = Physics2D.OverlapAreaAll(topLeft, bottomRight, whatIsGround);
			for (int i=0; i<colliders.Length; i++) {
				Debug.Log(colliders[i]);
			 	if (colliders[i].gameObject != gameObject) { // If the item im colliding with isnt the player...
					return true;
			 	}
			}
		}
		return false;
	}

	private void Jump()
	{
		if (isGrounded)
		{
			myRB.AddForce(new Vector2(0, JUMP_SPEED));
			isGrounded = false;
		}
			
		// else if (canDoubleJump)
		// {
		// 	myRB.AddForce(new Vector2(0, JUMP_SPEED));
		// 	canDoubleJump = false;
		// }
	}
}
