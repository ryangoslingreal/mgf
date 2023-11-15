using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	AudioSource audioSource;

	//public AudioClip[] detectionSounds;

	const float maxHealth = 100f;
	public float health = 100f;

	public GameObject[] patrolRoute;
	bool patrolling = false;
	int i;

	[HideInInspector]
	public GameObject player;
	public bool playerDetected;

	public Transform target;
	float speed = 0.05f;
	Vector3[] path;
	int targetIndex;

	void Start()
    {
        audioSource = GetComponent<AudioSource>();
		target = player.transform;
	}

	void Update()
	{
		PathRequestManager.RequestPath(transform.position, target.position, OnPathFound); // request path.
	}

	private void OnTriggerEnter(Collider other) // check for collision with bullet.
	{
		if (other.gameObject.tag == "Bullet") // check tag of object collided with.
		{
			health -= 25; // take dmg.
			other.gameObject.GetComponent<Bullet>().collision = true; // bullet destroyed next frame.

			if (health <= 0)
            {
                Destroy(this.gameObject); // die if health < 0.
			}
        }
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) // if path has been found.
	{
		if (pathSuccessful)
		{
			path = newPath;

			//restart FollowPath coroutine.
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}
	
	//IEnumerator FollowRoute()
	//{
	//	yield return null;
	//}

	IEnumerator FollowPath()
	{
		Vector3 currentWaypoint = path[0]; // set initial waypoint as first waypoint in path array.

		while (true)
		{
			if (transform.position == currentWaypoint) // waypoint reached.
			{
				targetIndex++; // next waypoint in array.

				if (targetIndex >= path.Length) // prevent index out of bounds error.
				{
					yield break;
				}

				currentWaypoint = path[targetIndex]; // set next waypoint.
			}

			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed); // move to waypoint.
			yield return null;
		}
	}
}