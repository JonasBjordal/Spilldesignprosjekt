using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]


    public class Platformer2DUserControl : MonoBehaviour
    {
        


		// Double Jump custom code
		//public bool canDoubleJump = false;

		// Fetch variable:
		private PlatformerCharacter2D otherScriptToAccess;
		private bool booleanCanDoubleJump;


		void Start ()
		{
			otherScriptToAccess = GetComponent<PlatformerCharacter2D> ();

			booleanCanDoubleJump = otherScriptToAccess.canDoubleJump;

		}


		//



		private PlatformerCharacter2D m_Character;
        private bool m_Jump;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
			if (!m_Jump) {
				// Read the jump input in Update so button presses aren't missed.
				m_Jump = CrossPlatformInputManager.GetButtonDown ("Jump"); }


			if (booleanCanDoubleJump)
			{
				// Read the jump input in Update so button presses aren't missed.
				m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}


			// Stopper her

		}

        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
			m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }

		void OnCollisionEnter2D(Collision2D other)
		{
			if (other.transform.tag == "MovingPlatform") {
				transform.parent = other.transform;
			}
		}
		void OnCollisionExit2D(Collision2D other)
		{
			if (other.transform.tag == "MovingPlatform") {
				transform.parent = null;
			}
		}
				
    }
}
