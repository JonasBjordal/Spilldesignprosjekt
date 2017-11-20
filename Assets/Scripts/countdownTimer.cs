﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countdownTimer : MonoBehaviour {

	public Text timerText;
	public static float timeRemaining = 200f;
	public static float timeInitial;
	private bool timerIsActive = true;
	public static bool goal = false;
	public static bool dead = false;

	public static Player player;

	// Use this for initialization
	void Start () {
		dead = false;
		goal = false;
		timerIsActive = true;
		timeRemaining = 200f;
		timeInitial = timeRemaining;

	
		timerText = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		if (timerIsActive) {
			timeRemaining -= Time.deltaTime;
			timerText.text = "TIME: " + timeRemaining.ToString ("f1");
			//print (timeRemaining);
			if (timeRemaining < 0.01f) {
				timeRemaining = 0f;
				timerText.text = "Time's up!";
				GameMaster.gm.EndGame ();
				GameMaster.KillPlayer (player);
				timerIsActive = false;
		
			} else if (timeRemaining > 0f && dead) {
				timerIsActive = false;
			}
			else if (timeRemaining > 0f && goal) {
				//timerText.text = "You made it!";
				timerText.text = "You made it! You spent " + (timeInitial - timeRemaining).ToString ("f1") + " seconds.";
				timerIsActive = false;
			}
				

		
		}

	}


}