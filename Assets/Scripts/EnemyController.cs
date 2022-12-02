using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    AudioSource audioSource;
    //public AudioClip[] detectionSounds;

    //public GameObject[] nodes;

    const float maxHealth = 100f;
    public float health = 100f;

    [HideInInspector]
    public GameObject player;
    public bool playerDetected;
	public float detection = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
    public void FindPlayerInListOfDetectedTargets(List<Transform> targets)
    {
		bool listContainsPlayer = false;

		if (targets.Count > 0) // check that list isn’t empty to avoid out of bounds error.
		{
			for (int i = 0; i < targets.Count; i++) // iterate through list.
			{
				if (targets[i].transform.gameObject.tag == "Player") // if current target is player.
				{
                    listContainsPlayer = true;
					break;
				}
			}
		}

		IncrementDetection(listContainsPlayer); // start detection grace period.
	}

	// detection grace period.
	void IncrementDetection(bool visible)
	{
		if (visible)
		{
			detection = Mathf.Clamp(detection += 5, 0, 100); // increment if player is visible.

			if (detection == 100)
			{
				playerDetected = true; // player has been fully detected.
				Debug.Log("player has been detected.");
			}
		}
		else if (!visible && !playerDetected)
		{
			detection = Mathf.Clamp(detection -= 3, 0, 100); // decrement if player is not visible.
		}

		Debug.Log(detection);
	}
}
