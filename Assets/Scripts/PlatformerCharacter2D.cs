using System;
using UnityEngine;


public class PlatformerCharacter2D : MonoBehaviour
{
	[SerializeField] private float m_MoveSpeed = 6f;

	[SerializeField] private float m_RunSpeedMultiplier = 2f;

	[SerializeField] private float m_SpeedLimit = 10f;

	[SerializeField] private float slideSpeed = 4f;
	[SerializeField] private float slideFriction = 1.6f;

	[SerializeField] private float m_AirMovement = 15f;

    [SerializeField] private float m_JumpForce = 16f;                  // Amount of force added when the player jumps.
    
	[SerializeField] private float m_fallMultiplier = 2f;
	[SerializeField] private float m_lowJumpMultiplier = 8f;


	[SerializeField] private float m_doubleJumpForce = 14f;

	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
	public bool m_inWater = false;
    

	private bool canDoubleJump = false;	// Double jump

	private bool jumpSpent = false;

	private bool sliding = false;

	public float delayBeforeDoubleJump = .05f; // Double jump

	public float jump1PrefabOffset;
	public float jump2PrefabOffset;
	public GameObject jump1Prefab; 
	public GameObject jump2Prefab;

	public GameObject splashPrefab;
	public GameObject splashOutPrefab; 
	public float splashPrefabYOffset;



	private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

	public bool goal = false;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {

        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }

		if (!m_Grounded && !jumpSpent) {
			// Custom double jump:
			jumpSpent = true;
			Invoke ("EnableDoubleJump", delayBeforeDoubleJump);
		} else if (m_Grounded && jumpSpent) {
			jumpSpent = false;
		}


        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
    }

	// Regular jump particles/smoke:
	private void instantiateJump1PrefabPos(Vector2 pos) {
		GameObject.Instantiate (jump1Prefab, pos, Quaternion.identity);
	}
	// Double jump particles/smoke:
	private void instantiateJump2PrefabPos(Vector2 pos) {
		GameObject.Instantiate (jump2Prefab, pos, Quaternion.identity);
	}
	// Splashing particles:
	private void instantiateSplashPrefabPos(Vector2 pos) {
		GameObject.Instantiate (splashPrefab, pos, Quaternion.identity);
	}
	private void instantiateSplashOutPrefabPos(Vector2 pos) {
		GameObject.Instantiate (splashOutPrefab, pos, Quaternion.identity);
	}

	// Movement:
    public void Move(float move, bool crouch, bool jump, bool run)
    {

        // If crouching, check to see if the character can stand up
        if (!crouch && m_Anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
				jump = false;
				canDoubleJump = false;
            }
        }
		// No jump in tight passages
		if (crouch && m_Anim.GetBool("Crouch") && Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
		{
				crouch = true;
				jump = false;
				canDoubleJump = false;
		}

        // Set whether or not the character is crouching in the animator
        m_Anim.SetBool("Crouch", crouch);

		m_Anim.SetBool ("Sliding", sliding);

		if (Mathf.Abs (m_Rigidbody2D.velocity.x) > 0.5f && !crouch) {
			m_Anim.SetBool ("Run", run);
		} else {
			m_Anim.SetBool ("Run", false);
		}
		if (!crouch) {
			slideSpeed = 1f;
			sliding = false;
		}

        //only control the player if grounded or airControl is turned on
		if (m_Grounded /*| m_AirControl*/) {
			// Reduce the speed if crouching by the crouchSpeed multiplier

			//if running
			if (m_Grounded && Mathf.Abs (m_Rigidbody2D.velocity.x) > m_SpeedLimit - 0.5f) {
				if (crouch) {
					sliding = true;
					print ("sliding" + sliding);
				} else {
					sliding = false;
				}
			}

			//if sliding and not still.
			if (sliding) {
				print ("ADD SLIDE ANIMATION + SMOKE PREFAB");
				if (m_Rigidbody2D.velocity.x > 0.05f) {
					slideSpeed -= Time.deltaTime * slideFriction;
					move = (crouch ? slideSpeed : move);
				}
				if (m_Rigidbody2D.velocity.x < -0.05f) {
					slideSpeed -= Time.deltaTime * slideFriction;
					move = (crouch ? -slideSpeed : move);
				}
				if (slideSpeed < 1f) {
					sliding = false;
				}
			}


			if (m_Grounded) {
//				if (crouch && !run) {
//					move *= m_CrouchSpeed
				move = ((crouch && !run) ? move * m_CrouchSpeed : move);
				move = ((m_Anim.GetBool("Run")) ? move * m_RunSpeedMultiplier  : move);
			}

			m_Rigidbody2D.velocity = new Vector2 ((move * m_MoveSpeed), m_Rigidbody2D.velocity.y);



			// The Speed animator parameter is set to the absolute value of the horizontal input.
			m_Anim.SetFloat ("Speed", Mathf.Abs (move));

		


			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight) {
				// ... flip the player.
				Flip ();
			}
                // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight) {
				// ... flip the player.
				Flip ();
			}
		} else { // Air Control:
			if (move != 0) {
				m_Rigidbody2D.AddForce (Vector2.right * m_AirMovement * move);
				if (Mathf.Abs(m_Rigidbody2D.velocity.x) > m_SpeedLimit) { // Aerial Speed Limit
					if (m_Rigidbody2D.velocity.x > 0) {
						m_Rigidbody2D.velocity = new Vector2 ((m_SpeedLimit), m_Rigidbody2D.velocity.y);
					} else if (m_Rigidbody2D.velocity.x < 0) {
						m_Rigidbody2D.velocity = new Vector2 ((-m_SpeedLimit), m_Rigidbody2D.velocity.y);
					}
				}
			}
		}

        // Player jump:
		if(jump) {
			if (m_Grounded && m_Anim.GetBool ("Ground")) {
				// Add a vertical force to the player.
				m_Grounded = false;
				canDoubleJump = false;
				m_Anim.SetBool ("Ground", false);
				m_Rigidbody2D.AddForce (transform.up * m_JumpForce);
				if (!m_inWater) {
					instantiateJump1PrefabPos (new Vector3 (transform.position.x, transform.position.y + jump1PrefabOffset, transform.position.z));
				}
	

			} else if (canDoubleJump) {
				canDoubleJump = false;
				m_Rigidbody2D.velocity = new Vector2 (m_Rigidbody2D.velocity.x * 0.7f, 0f);
				//m_Rigidbody2D.velocity = Vector2.up * m_doubleJumpForce;
				m_Rigidbody2D.AddForce(Vector2.up * 500);
				if (!m_inWater) {
					instantiateJump2PrefabPos (new Vector3 (transform.position.x, transform.position.y + jump2PrefabOffset, transform.position.z));
				}
			}
		}
		if (!m_inWater) {
			if (m_Rigidbody2D.velocity.y < 0) {
				m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (m_fallMultiplier - 1) * Time.deltaTime;
			} else if (m_Rigidbody2D.velocity.y > 0 && !Input.GetButton ("Jump")) {
				m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (m_lowJumpMultiplier - 1) * Time.deltaTime;
			}
		}

	}

	// Custom double jump:
	public void EnableDoubleJump() 
	{
		canDoubleJump = true;
	}




    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }



	// Player in water -> default gravity

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Water") {
			m_inWater = true;
			print ("You're in the water! Add sound");

			instantiateSplashPrefabPos (new Vector3 (transform.position.x, transform.position.y + splashPrefabYOffset, transform.position.z));
		
		}

		// Inkrementere spawnPoint ved passering av checkpoint:
		else if (other.gameObject.tag == "Checkpoint") {
			GameMaster.gm.currentSpawnPoint += 1;
			Debug.Log ("Checkpoint passed");

			other.gameObject.SetActive (false);
		} else if (other.gameObject.tag == "GoalTrigger") {
			//GameMaster.gm.counter = false;
			Debug.Log ("Du er i mål!");
			goal = true;
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Water")
		{
			m_inWater = false;
			print("You're out of the water! Add sound");

			instantiateSplashOutPrefabPos (new Vector3 (transform.position.x, transform.position.y + splashPrefabYOffset, transform.position.z));

		}
	}



	// Player moves in relation to MovingPlatform:
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.tag == "MovingPlatform") {
			//platform = other.transform.parent.GetComponent<MovingPlatform> ();
			transform.parent = other.transform;

		}
	}
	void OnCollisionExit2D(Collision2D other)
	{
		if (other.transform.tag == "MovingPlatform") {
			//platform = null;
			transform.parent = null;

		}
	}


}
