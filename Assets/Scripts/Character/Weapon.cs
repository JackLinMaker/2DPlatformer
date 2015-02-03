using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public float fireRate = 0;
	public float Damage = 10f;
	public LayerMask whatToHit;
	public Transform BulletTrailPrefab;
	public Transform MuzzleFlashPrefab;

	float timeToFire = 0;
	Transform firePoint;
	float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;

	void Awake() {
		firePoint = transform.FindChild("Fire Point");
		if (firePoint == null) {
			Debug.LogError("No fire point!!!");		
		}
	}


	void Update() {
		if (fireRate == 0) {
			if (Input.GetButtonDown ("Fire1")) {
				Shoot();
			}		
		} else {
			if(Input.GetButton("Fire1") && Time.time > timeToFire) {
				timeToFire = Time.time + 1 / fireRate;
				Shoot();
			}		
		}
	}

	void Shoot() {
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,  Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100,  whatToHit);
		if (Time.time >= timeToSpawnEffect) {
			Effect();
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}

		//Debug.DrawLine (firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
		if (hit.collider != null) {
			//Debug.DrawLine(firePointPosition, hit.point, Color.red);	
			Debug.Log("we hit " + hit.collider.name + " and did damage " + Damage);
			GameObject enemy = hit.collider.gameObject;
			if(enemy.tag == "Enemy") {
				enemy.GetComponent<EnemyHealth>().Damage(Damage);

			}
		}
	}

	void Effect() {
		Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation);
		Transform clone = Instantiate (MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		clone.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, size);
		Destroy (clone.gameObject, 0.02f);
	}

}
