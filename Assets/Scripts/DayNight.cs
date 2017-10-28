using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public float scalingFactor = 1f; // Bigger for slower

	void Update()
	{
		
		// Rotate the object around its local X axis at 1 degree per second
		transform.Rotate(Vector3.forward * ((Time.deltaTime)/scalingFactor) * -1);

	}

}
