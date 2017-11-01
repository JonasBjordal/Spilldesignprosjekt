using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countdownTimer : MonoBehaviour {

	public Text timerText;
	public float timeRemaining = 99;
	private bool timerIsActive = true;
	public static bool goal = false;

	// Use this for initialization
	void Start () {
		timerText = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		if (timerIsActive) {
			timeRemaining -= Time.deltaTime;
			timerText.text = timeRemaining.ToString ("f1");
			//print (timeRemaining);
			if (timeRemaining <= 0) {
				timeRemaining = 0;
				timerText.text = "Time's up!";
				timerIsActive = false;
			} else if (timeRemaining > 0 && goal) {
				timerText.text = "Grattis, mann";
			}
				

		
		}

	}


}