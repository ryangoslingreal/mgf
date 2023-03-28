using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	AudioSource audioSource;

	//public AudioClip[] detectionSounds;

	const float maxHealth = 100f;
	public float health = 100f;

	[HideInInspector]
	public GameObject player;
	public bool playerDetected;

	public Transform target;
	float speed = 1f;
	Vector3[] path;
	int targetIndex;

	void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

			transform.LookAt(currentWaypoint);
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime); // move to waypoint.
			yield return null;
		}
	}
}