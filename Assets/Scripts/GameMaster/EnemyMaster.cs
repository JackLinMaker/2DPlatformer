using UnityEngine;
using System.Collections;

public class EnemyMaster : MonoBehaviour {
	public Transform enemyPrefab;
	public Transform[] spawnPoints;
	public float spawnDelay = 2f;
	public int count = 0;
	public int maxCount = 10;
	void Start() {
		InvokeRepeating ("RespawnEnemy", 0f, 2f);
	}


	public void RespawnEnemy() {
		if (count < maxCount) {
			Debug.Log("RespawnEnemy");
			int index = Random.Range (0, spawnPoints.Length);
			Instantiate (enemyPrefab, spawnPoints[index].position, Quaternion.identity);
			count += 1;
		} else {
			return;
		}
	}



	public void subCount() {
		count -= 1;
	}
}
