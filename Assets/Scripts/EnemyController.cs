using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] detectionSounds;

    public GameObject[] nodes;

    const float maxHealth = 100f;
    public float health = 100f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource.PlayOneShot(detectionSounds[2], 0.3f);
    }

    void Update()
    {
        
    }
}
