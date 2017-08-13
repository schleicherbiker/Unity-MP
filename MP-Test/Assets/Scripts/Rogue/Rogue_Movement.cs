using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Rogue_Movement : NetworkBehaviour {
	
	// Serialized Vars
	[SerializeField] private float MOVEMENT_SPEED;
	[SerializeField] private float JUMP_SPEED;
	[SerializeField] private float DASH_TIME;
	[SerializeField] private float DASH_SPEED;
	[SerializeField] private LayerMask whatIsGround;
	[SerializeField] private GameObject Dagger;
	
	// Movement Vars
	private float horizontal;
	private bool isGrounded;
	private bool canDoubleJump;
	private Rigidbody2D myRB; 
	private BoxCollider2D myCollider;
	private bool isBusy;
	
	// Animation Vars
	[SyncVar(hook = "SetDir")] public bool facingRight;
	private Animator myAnimator;

	// Ability Vars
	private bool attack;

	public override void OnStartLocalPlayer()
	{
		// Reference Components
		myRB = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D>();
		myAnimator = GetComponent<Animator>();

		// Setup Camera
		Vector3 playerPos = this.transform.position;
		playerPos.z = -20;
		Camera.main.transform.position = playerPos;
		Camera.main.transform.parent = this.transform;
	}

	void Update()
	{
		// Return If Not Local Player
		if (!isLocalPlayer)
            return;
		
		// Handle Input Every Frame
		HandleInput();
		Debug.Log(facingRight);
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
		//HandleAttacks();
		//ResetVars();
	}

	private void HandleMovement(float horizontal)
	{
		if (isBusy)
			return;

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
				facingRight = true;
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
				facingRight = false;
				CmdUpdateServerDir(false);
			}
		}
	}

	// private void HandleAttacks() {
	// 	if (attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
	// 		myAnimator.SetTrigger("attack");
	// 	}
	// }

	private void HandleInput()
	{
		// if (Input.GetKeyDown(KeyCode.LeftShift)) {
		// 	attack = true;
		// }

		// Jump
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("jump!");
			Jump();
		}

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

	private void SetDir(bool facingRight)
	{
		// Return if Not Server
		if (!isServer)
			return;
		RpcSetDir(facingRight);
		Debug.Log("Hello");
	}

	[ClientRpc] private void RpcSetDir(bool facingRight)
	{
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
		Debug.Log("1:" + facingRight);
		facingRight = faceRight;
		Debug.Log("2:" + facingRight);
	}

	private void ResetVars() {
		attack = false;
	}

	private bool IsGrounded()
	{
		if (myRB.velocity.y <= 0)
		{
			Vector2 topLeft, bottomRight;

			if (transform.localScale.x == 1)
			{
				topLeft = new Vector2(myCollider.transform.position.x + myCollider.offset.x - (myCollider.size.x / 2f) + .1f, myCollider.transform.position.y + myCollider.offset.y - (myCollider.size.y / 2f));
				bottomRight = new Vector2(myCollider.transform.position.x + myCollider.offset.x + (myCollider.size.x / 2f) -.1f, myCollider.transform.position.y + myCollider.offset.y - (myCollider.size.y / 2f) - .001f);
				Debug.DrawLine(topLeft, bottomRight, Color.yellow);
			} 
			else 
			{
				topLeft = new Vector2(myCollider.transform.position.x - myCollider.offset.x - (myCollider.size.x / 2f) + .1f, myCollider.transform.position.y + myCollider.offset.y - (myCollider.size.y / 2f));
				bottomRight = new Vector2(myCollider.transform.position.x - myCollider.offset.x + (myCollider.size.x / 2f) -.1f, myCollider.transform.position.y + myCollider.offset.y - (myCollider.size.y / 2f) - .001f);
				Debug.DrawLine(topLeft, bottomRight, Color.yellow);
			}
			
			Collider2D[] colliders = Physics2D.OverlapAreaAll(topLeft, bottomRight, whatIsGround);
			for (int i=0; i<colliders.Length; i++)
			{
			 	if (colliders[i].gameObject != gameObject) 
				{
					canDoubleJump = true;
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
			Debug.Log("Normal Jump!");
			myRB.velocity = new Vector2(myRB.velocity.x, JUMP_SPEED);
			isGrounded = false;
		}	
		else if (canDoubleJump)
		{
			Debug.Log("Double Jump!");
			myRB.velocity = new Vector2(myRB.velocity.x, JUMP_SPEED);
			canDoubleJump = false;
		}
	}

	// ======================================================================================================================================================================================================================================\\
	// ============================================================================================================= ABILITIES ==============================================================================================================\\
	// ======================================================================================================================================================================================================================================\\

	void Dash()
	{
		isBusy = true;
		myRB.gravityScale = 0;
		if (this.facingRight)
			myRB.velocity = new Vector2(DASH_SPEED, 0);
		else
			myRB.velocity = new Vector2(-DASH_SPEED, 0);
		StartCoroutine(StopDash(DASH_TIME));
	}

	IEnumerator StopDash(float time)
	{
		yield return new WaitForSeconds(DASH_TIME);
		isBusy = false;
		myRB.gravityScale = 2.5f;
	} 

	void ThrowDagger()
	{
		Instantiate(Dagger, transform);
	}
}
