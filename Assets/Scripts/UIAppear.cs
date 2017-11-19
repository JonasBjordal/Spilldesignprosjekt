using UnityEngine;
using System.Collections;

public class UIAppear : MonoBehaviour 
{
	void Start() {
		
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		//Telling game to actiavte boolean
		if(other.gameObject.tag == "Player") 
		{
			countdownTimer.goal = true; 
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag ("Player")) {

		}
	}
}
