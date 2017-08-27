using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Match;

public class Rogue_Movement : NetworkBehaviour {

	// Movement Vars
	[SerializeField] private float MOVEMENT_SPEED;
	[SerializeField] private float JUMP_SPEED;
	[SerializeField] private LayerMask whatIsGround;
	private float horizontal;
	private bool isGrounded;
	private bool canDoubleJump;
	private Rigidbody2D myRB; 
	private BoxCollider2D myCollider;
	private bool canCast = true;
	private bool canMove = true;
	
	// Animation Vars
	[SyncVar(hook = "SetDir")] public bool facingRight;
	private Animator myAnimator;

	// Ability Vars
	[SerializeField] private float DASH_TIME;
	[SerializeField] private float DASH_SPEED;
	[SerializeField] private float DASH_CD;
	[SerializeField] private float DAGGER_SPEED;
	[SerializeField] private float DAGGER_CD;
	[SerializeField] private GameObject daggerPrefab;
	private int dashCharges = 3;



	void Start()
	{
		if (!facingRight)
		{
			Vector3 playerScale = transform.localScale;
			playerScale.x = -1;
			transform.localScale = playerScale;
		}		
	}

	public override void OnStartLocalPlayer()
	{
		// Reference Components
		myRB = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D>();
		myAnimator = GetComponent<Animator>();

		// Setup Camera and player Z pos
		Vector3 playerPos = transform.position;
		playerPos.z = -20;
		Camera.main.transform.position = playerPos;
		Camera.main.transform.parent = transform;
		playerPos.z = -10;
		transform.position = playerPos;
	}

	void Update()
	{
		Debug.Log(dashCharges);
		// Return If Not Local Player
		if (!isLocalPlayer)
            return;
		
		// Handle Input Every Frame
		HandleInput();
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
		if (!canMove)
			return;
 
		// Set Normal Movespeed
		myRB.velocity = new Vector2(horizontal * MOVEMENT_SPEED, myRB.velocity.y);

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
	// 		
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
			Jump();
		}

		// Auto Attack
		if (Input.GetMouseButtonDown(0) && canCast)
		{
			AutoAttack();
		}

		// First Move
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
		}

		// Second Move
		if (Input.GetKeyDown(KeyCode.Alpha2) && canCast)
		{
			if (dashCharges > 0)
				Dash();
		}

		// Third Move
		if (Input.GetKeyDown(KeyCode.Alpha3)) 
		{
			ThrowDagger();
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
	}

	[ClientRpc] private void RpcSetDir(bool facingRight)
	{
		// If Not Local Player, Update Direction On Client...
		if (isLocalPlayer)
			return;

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

	[Command] private void CmdUpdateServerDir(bool faceRight)
	{
		facingRight = faceRight;
	}

	private void ResetVars() {
		// attack = false;
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
			myRB.velocity = new Vector2(myRB.velocity.x, JUMP_SPEED);
			isGrounded = false;
		}	
		else if (canDoubleJump)
		{
			myRB.velocity = new Vector2(myRB.velocity.x, JUMP_SPEED);
			canDoubleJump = false;
		}
	}

	// ======================================================================================================================================================================================================================================\\
	// ============================================================================================================= ABILITIES ==============================================================================================================\\
	// ======================================================================================================================================================================================================================================\\

	void AutoAttack()
	{
		canCast = false;
		myAnimator.SetTrigger("attack");
		canCast = true;
	}

	void Dash()
	{
		// Recharge dashes if < 3
		if (dashCharges == 3)
		{
			StartCoroutine(AddDash());
		}
			
		canCast = false;
		canMove = false;
		dashCharges--;

		myRB.gravityScale = 0;
		if (this.facingRight)
			myRB.velocity = new Vector2(DASH_SPEED, 0);
		else
			myRB.velocity = new Vector2(-DASH_SPEED, 0);
		StartCoroutine(StopDash());
		// TODO StopCOroutine
	}

	IEnumerator StopDash()
	{
		yield return new WaitForSeconds(DASH_TIME);
		canMove = true;
		canCast = true;
		myRB.gravityScale = 2.5f;
	} 

	IEnumerator AddDash()
	{
		yield return new WaitForSeconds(DASH_CD);
		if (dashCharges < 3)
		{
			dashCharges++;
			if (dashCharges < 3)
				StartCoroutine(AddDash());
		}
			
	} 

	void ThrowDagger()
	{
		// Get Correct Direction
		Vector3 dir3 = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		Vector2 dir = new Vector2(dir3.x, dir3.y);
		dir = dir.normalized * DAGGER_SPEED;

		// Spawn Dagger with Correct Spin Direction
		if (facingRight)
			CmdThrowDagger(dir, -1000f);
		else
			CmdThrowDagger(dir, 1000f);
		
	}

	[Command] void CmdThrowDagger(Vector3 dir, float spin)
	{
		var dagger = (GameObject) Instantiate(daggerPrefab, transform.position, transform.rotation);
		dagger.GetComponent<Rigidbody2D>().velocity = dir;
		dagger.GetComponent<Dagger>().SPIN_SPEED = spin;
		Destroy(dagger, 2.0f);
		NetworkServer.Spawn(dagger);
	}
}
