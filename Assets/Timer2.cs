using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer2 : MonoBehaviour {
	public Text timerText;

	void Start () {
		timerText = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {

		timerText.text = " Your time: " + (countdownTimer.timeSpentString) + " seconds" + "\r\n\r\n" + "But can you do it faster?";
	}
}
