using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideFix : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<ParticleSystem>().collision.SetPlane(0, GameObject.Find("VannColliderTile").transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
