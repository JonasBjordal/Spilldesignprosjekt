using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraX : MonoBehaviour {

	private Transform cameraTransform;

	// Use this for initialization
	void Awake () {
		cameraTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = new Vector3 (cameraTransform.position.x, transform.position.y, transform.position.z);

	}
}
