using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	void Start() {
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
		}
	}

	public Transform playerPrefab;
	public Transform[] spawnPoints;
	public int spawnDelay = 2;
	public Transform spawnPrefab;

	public IEnumerator RespawnPlayer() {
		audio.Play ();
		yield return new WaitForSeconds (spawnDelay);
		Debug.Log ("RespawnPlayer");
		if (playerPrefab == null) {
			Debug.Log("playerPrefab is null");
		}
		int index = Random.Range (0, spawnPoints.Length);

		Instantiate (playerPrefab, spawnPoints[index].position, Quaternion.identity);
		GameObject  clone =  Instantiate (spawnPrefab, spawnPoints[index].position, Quaternion.identity) as GameObject;
		Destroy (clone, 3f);
	}

	public static void KillPlayer(Player player) {
		//player.GetComponent<AudioSource> ().Play ();
		Destroy (player.gameObject);
		gm.StartCoroutine (gm.RespawnPlayer ());

	}


}
