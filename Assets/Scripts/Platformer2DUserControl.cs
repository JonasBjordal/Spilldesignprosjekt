using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

    [RequireComponent(typeof (PlatformerCharacter2D))]


    public class Platformer2DUserControl : MonoBehaviour
    {
        


		private PlatformerCharacter2D m_Character;
        private bool m_Jump;
		private bool m_Run;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
			// Read the jump input in Update so button presses aren't missed.
			if(CrossPlatformInputManager.GetButtonDown ("Jump")) {
				m_Jump = true;
			}
			if (CrossPlatformInputManager.GetButton ("Run")) {
				m_Run = true;
			}

		}

        private void FixedUpdate()
        {
            // Read the inputs.
		bool crouch = false;
		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.LeftControl)) {
			crouch = true;

		}
	
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
		m_Character.Move(h, crouch, m_Jump, m_Run);
            m_Jump = false;
			m_Run = false;
        }			
    }
