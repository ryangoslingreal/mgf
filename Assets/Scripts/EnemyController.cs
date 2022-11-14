using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] detectionSounds;

    public GameObject[] nodes;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(detectionSounds[2], 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
