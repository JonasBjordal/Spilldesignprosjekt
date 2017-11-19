using System;
using UnityEngine;

    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
		
		public float xOffset;

        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
		public float yPosRestriction = -1;
		public float yPosRestriction2 = 2;

		public float xPosRestriction = -10;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
		private float nextTimeToSearch = 0;

		private Vector3 targetPosition;
		

        // Use this for initialization
        private void Start()
        {
		targetPosition = new Vector3 (target.position.x + xOffset, target.position.y, target.position.z);

		m_LastTargetPosition = targetPosition;
		m_OffsetZ = (transform.position - targetPosition).z;
            transform.parent = null;

			
        }


        // Update is called once per frame
        private void Update()
        {
		

			if (target == null) {
				FindPlayer ();
				return;

			}
		targetPosition = new Vector3 (target.position.x + xOffset, target.position.y, target.position.z);


            // only update lookahead pos if accelerating or changed direction
		float xMoveDelta = (targetPosition - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }


		Vector3 aheadTargetPos = targetPosition + m_LookAheadPos + Vector3.forward*m_OffsetZ;
		Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

		newPos = new Vector3 (Mathf.Clamp (newPos.x, xPosRestriction, Mathf.Infinity), Mathf.Clamp (newPos.y, yPosRestriction, yPosRestriction2), newPos.z);

            transform.position = newPos;

		m_LastTargetPosition = targetPosition;
        }

		void FindPlayer () {
			if (nextTimeToSearch <= Time.time) {
				GameObject searchResult = GameObject.FindGameObjectWithTag ("Player");
			if (searchResult != null)
				target = searchResult.transform;
				nextTimeToSearch = Time.time + 0.5f;
			}

		}

    }

