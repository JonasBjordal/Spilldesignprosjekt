using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public GameObject Platform;

	public float moveSpeed;

	public Transform currentPoint; 

	public Transform [] points;

	public int pointSelection;

	public Vector2 velocity;
	private Vector3 prePos; 

	// Use this for initialization
	void Start () {
		currentPoint = points [pointSelection];
	}
	
	// Update is called once per frame
	void Update () {

		if (Platform.transform.position == currentPoint.position) {
			pointSelection++;

			if (pointSelection == points.Length) {
				pointSelection = 0;
			}
			currentPoint = points [pointSelection];
		}
	
	}

	void FixedUpdate() {
		prePos = Platform.transform.position;
		Platform.transform.position = Vector3.MoveTowards (Platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);
		velocity = new Vector2 (Platform.transform.position.x - prePos.x, Platform.transform.position.y - prePos.y) / Time.deltaTime;
	}
}
