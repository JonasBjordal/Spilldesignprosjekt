using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {




	[System.Serializable]
	public class PlayerStats {
		public float Health = 100f;
		public int delayGoalRespawn = 2;
	}

	public PlayerStats playerStats = new PlayerStats ();

	public int fallBoundary = -20;


	void Start()
	{
		//PlatformerCharacter2D playerScript = Player.GetComponent<PlatformerCharacter2D> ();

	}




	void Update () {
		if (transform.position.y <= fallBoundary) {
			DamagePlayer (999999999);
		}
		//if (playerScript.goal) {
		//	Invoke (autoKill, delayGoalRespawn);
		//}
		
	}

	public void autoKill() 
	{
		DamagePlayer (999999999);
	}


	public void DamagePlayer (int damage) {
		playerStats.Health -= damage;
		if (playerStats.Health <= 0) {
			GameMaster.KillPlayer(this);
		}

	}

}
