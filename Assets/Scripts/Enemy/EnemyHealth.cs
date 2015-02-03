using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	public EnemyMaster enemyMaster;
	public float health = 30f;

	public void Damage(float damage) {
		audio.Play ();
		health -= damage;
		if (health <= 0f) {
			Destroy(gameObject);		
			enemyMaster.subCount();
		}
	}
}
