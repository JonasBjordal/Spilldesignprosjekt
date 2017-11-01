using UnityEngine;
using System.Collections;

public class UIAppear : MonoBehaviour 
{
	private GameObject hidey;

	void Start() {
		hidey = GameObject.Find ("hidey_Goal");
		hidey.gameObject.SetActive (false);


	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		//Telling game to actiavte boolean
		if(other.gameObject.tag == "Player") 
		{
			countdownTimer.goal = true; 
			hidey.gameObject.SetActive (true);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag ("Player")) {
			hidey.gameObject.SetActive (false);

		}
	}
}
