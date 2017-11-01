using System.Collections;
using System.Collections.Generic;
using UnityEngine;

		
	public class GameMaster : MonoBehaviour {

		public static GameMaster gm;

		void Start () {
			if (gm == null) {
				gm = this; //GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
			}
		}

		public Transform playerPrefab;

		public Transform [] spawnPoints;
		public int currentSpawnPoint = 0;

		public int spawnDelay = 2;

		public IEnumerator RespawnPlayer () {
			Debug.Log ("TODO: Add waiting for spawn sound");
			yield return new WaitForSeconds (spawnDelay);

			Transform spawnPoint = spawnPoints [currentSpawnPoint];

			Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
			Debug.Log ("TODO: Add spawn particles");
		}

		public static void KillPlayer (Player player) {
			Destroy (player.gameObject);
			gm.StartCoroutine(gm.RespawnPlayer());

		}


	}
