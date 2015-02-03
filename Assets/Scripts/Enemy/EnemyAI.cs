using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D) )]
[RequireComponent (typeof (Seeker) )]
public class EnemyAI : MonoBehaviour {
	public static GameMaster gm;

	// what to chase
	public Transform target;
	// how many times each second we will update our path
	public float updateRate = 2f;

	private Seeker seeker;

	private Rigidbody2D rb;

	// The calculated path
	public Path path;

	// The AI speed per second
	public float speed = 300;
	// way to change between force and impulse
	public ForceMode2D fMode;

	[HideInInspector]
	public bool pathIsEnded = false;
	// the max distance from the ai to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	private int currentWaypoint = 0;

	float nextTimeToSearch = 0;

	void Start() {
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();
		if (target == null) {
			Debug.LogError("we can't find target");		
			return;
		}
		// start a new path to the target positon, returns the results to the OnPathComplete.
		seeker.StartPath (transform.position, target.position, OnPathComplete);

		StartCoroutine (UpdatePath ());
	}

	IEnumerator  UpdatePath () {
		//Debug.Log ("UpdatePath");
		if (target == null) {
						//TODO: Insert a player search here.
						
			yield return false;
		} else {
			// Start a new path to the target position, return the result to the OnPathComplete method
			
			//Debug.Log ("StartPath target position =" + target.position);
			
			seeker.StartPath (transform.position, target.position, OnPathComplete);
			
			yield return new WaitForSeconds ( 1f / updateRate );

			StartCoroutine (UpdatePath());
		}
		
	
	}

	public void OnPathComplete(Path p) {
		//Debug.Log ("we got a path, didi it have an error? " + p.error);
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}

	void FixedUpdate() {
		if (target == null) {
			// TODO: insert a player search here.
			FindPlayer();

			return;
		}

		// TODO: Always look at player
		//seeker.StartPath (transform.position, target.position, OnPathComplete);
		if (path == null)
				return;
		if (currentWaypoint >= path.vectorPath.Count) {
			if(pathIsEnded) {
				return;
			}
			//Debug.Log("end of path reached.");

			pathIsEnded = true;
		}

		pathIsEnded = false;

		// Direction to the next waypoint
		if (currentWaypoint < path.vectorPath.Count) {
			Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
			dir *= speed * Time.fixedDeltaTime;
			
			// move the ai
			rb.AddForce (dir, fMode);
			float dist = Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]);
			
			if (dist < nextWaypointDistance) {
				currentWaypoint++;
				return;
			}
		}


	

	}

	void FindPlayer() {
		GameObject searchPlayer = GameObject.FindGameObjectWithTag("Player");
		if (searchPlayer != null){
			target = searchPlayer.transform;
			seeker.StartPath(transform.position,target.position, OnPathComplete);
			StartCoroutine (UpdatePath());
		} else {
			return;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.name == "Player") {
			Debug.Log("OnTriggerEnter2D player");
			GameMaster.KillPlayer(collider.gameObject.GetComponent<Player>());
		}
	}




}
