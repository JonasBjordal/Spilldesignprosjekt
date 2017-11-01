using System;
using UnityEngine;


public class PlatformerCharacter2D : MonoBehaviour
{
	[SerializeField] private float m_MoveSpeed = 15f;

    [SerializeField] private float m_JumpForce = 8f;                  // Amount of force added when the player jumps.
    
	[SerializeField] private float m_fallMultiplier = 2.5f;
	[SerializeField] private float m_lowJumpMultiplier = 2f;


	[SerializeField] private float m_doubleJumpForce = 600f;

	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
	public bool m_inWater = false;
    

	public bool canDoubleJump = false;	// Double jump

	private bool jumpSpent = false;


	public float delayBeforeDoubleJump = .05f; // Double jump

	public float jump1PrefabOffset;
	public float jump2PrefabOffset;
	public GameObject jump1Prefab; 
	public GameObject jump2Prefab;




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



    public void Move(float move, bool crouch, bool jump)
    {

        // If crouching, check to see if the character can stand up
        if (!crouch && m_Anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        m_Anim.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move*m_CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));


			m_Rigidbody2D.velocity = new Vector2 (move * m_MoveSpeed, m_Rigidbody2D.velocity.y);


            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
                // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
		if(jump) {
			if (m_Grounded && m_Anim.GetBool ("Ground")) {
				// Add a vertical force to the player.
				m_Grounded = false;
				canDoubleJump = false;
				m_Anim.SetBool ("Ground", false);
				m_Rigidbody2D.velocity = Vector2.up * m_JumpForce;
				instantiateJump1PrefabPos (new Vector3 (transform.position.x, transform.position.y + jump1PrefabOffset, transform.position.z));
			
	

			} else if (canDoubleJump) {
				canDoubleJump = false;
				m_Rigidbody2D.velocity = new Vector2 (m_Rigidbody2D.velocity.x, 0f);
				m_Rigidbody2D.velocity = Vector2.up * m_doubleJumpForce;
				instantiateJump2PrefabPos (new Vector3 (transform.position.x, transform.position.y + jump2PrefabOffset, transform.position.z));
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
			print ("You're in the water!");
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
			print("You're out of the water!");
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
