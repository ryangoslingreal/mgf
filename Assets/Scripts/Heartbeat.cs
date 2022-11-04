using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heartbeat : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip slow;
    public AudioClip med;
    public AudioClip fast;

    public const int defaultbpm = 60;
    public const int maxbpm = 180;
    public const int maxRange = 15;
    public const int midlowbpm = 80;
    public const int midhighbpm = 120;
    public int bpm = 60;

	void Start()
	{
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(Beat());
    }
    
	void FixedUpdate()
    {
        GameObject closest = FindClosestEnemy();
        float dist = Vector3.Distance(transform.position, closest.transform.position);

        if (dist < maxRange)
		{
            RaycastHit hit;

            if (Physics.Raycast(transform.position, (closest.transform.position - transform.position), out hit, 15f))
            {
                if (hit.transform != transform)
                {
                    dist += 5f;
                }
            }

            bpm = Mathf.Clamp((int)(ParabolicCurve(dist) * defaultbpm), defaultbpm, maxbpm);
		}
    }

    IEnumerator Beat()
	{
        if (bpm >= defaultbpm && bpm < midlowbpm)
		{
            audioSource.PlayOneShot(slow, 0.3f);
		}
        else if (bpm >= midlowbpm && bpm < midhighbpm)
		{
            audioSource.PlayOneShot(med, 0.5f);
		}
        else if (bpm >= midhighbpm && bpm <= maxbpm)
		{
            audioSource.PlayOneShot(fast, 0.8f);
		}
        
        float timeBetweenBeats = (float)60 / (float)bpm;
        yield return new WaitForSeconds((float)timeBetweenBeats);
        StartCoroutine(Beat());
	}

    GameObject FindClosestEnemy()
	{
        GameObject[] objs;
        objs = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closest = objs[0];

		for (int i = 1; i < objs.Length; i++)
		{
            float linearDist1 = Vector3.Distance(transform.position, objs[i].transform.position);
            float linearDist2 = Vector3.Distance(transform.position, closest.transform.position);

            RaycastHit hitCurrent;
            RaycastHit hitClosest;

            if (Physics.Raycast(transform.position, (objs[i].transform.position - transform.position), out hitCurrent, 15f))
            {
                if (hitCurrent.transform != transform)
                {
                    linearDist1 += 15f;
                }
            }

			if (Physics.Raycast(transform.position, (closest.transform.position - transform.position), out hitClosest, 15f))
			{
				if (hitClosest.transform != transform)
				{
					linearDist2 += 15f;
				}
			}

			if (linearDist1 < linearDist2)
			{
                closest = objs[i];
			}
		}

        Debug.DrawLine(transform.position, closest.transform.position, Color.black);
        return closest;
    }

    float ParabolicCurve(float x)
	{
        float y = Mathf.Pow(2, ((-1f/3f) * x + 2f)) + 1;
        return y;
	}
}