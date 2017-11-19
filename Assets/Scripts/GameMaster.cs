using System.Collections;
using System.Collections.Generic;
using UnityEngine;

		
public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	private static int _remainingLives = 3;
	public static int RemainingLives
	{
		get { return _remainingLives; }
	}

	public static bool dead = false;

	void Start () {
		dead = false;
		_remainingLives = 3;
		if (gm == null) {
			gm = this; //GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}

	public Transform playerPrefab;
	public Transform [] spawnPoints;
	public int currentSpawnPoint = 0;
	public int spawnDelay = 2;

	// Spawn particles:
	public Transform spawnPrefab;

	[SerializeField]
	private GameObject gameOverUI;



	public void EndGame ()
	{
		_remainingLives = 0;
		dead = true;
		Debug.Log ("GAME OVER");
		gameOverUI.SetActive (true);
	}

	public IEnumerator RespawnPlayer () {
		Debug.Log ("TODO: Add waiting for spawn sound");
		yield return new WaitForSeconds (spawnDelay);

		Transform spawnPoint = spawnPoints [currentSpawnPoint];

		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);

		Transform clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation);
		Destroy (clone.gameObject, 3f);
	}

	public static void KillPlayer (Player player) {
		Destroy (player.gameObject);
		_remainingLives -= 1;
		if (_remainingLives <= 0){
			countdownTimer.dead = true; 
			gm.EndGame ();
		} else {
			gm.StartCoroutine(gm.RespawnPlayer());
		}


	}


}
