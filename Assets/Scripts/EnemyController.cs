using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
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
}