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
    public const int maxbpm = 150;
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
        // distance not returned from FindClosestEnemy() so has to be calculated / buffed again.
        float dist = Vector3.Distance(transform.position, closest.transform.position);

        if (dist < maxRange)
		{
            RaycastHit hit;

            //  buff distance if there is an obstacle.
            if (Physics.Raycast(transform.position, (closest.transform.position - transform.position), out hit, 15f))
            {
                if (hit.transform != transform)
                {
                    dist += 10f;
                }
            }

            bpm = Mathf.Clamp((int)(ParabolicCurve(dist) * defaultbpm), defaultbpm, maxbpm); // clamp heart rate between realistic bounds.
		}
    }

    IEnumerator Beat()
	{
        // play appropriate heartbeat sound clip depending on heart rate.
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
        
        float timeBetweenBeats = (float)60 / (float)bpm; // calculate time before calling itself.
        yield return new WaitForSeconds((float)timeBetweenBeats); // wait.
        StartCoroutine(Beat());
	}

    GameObject FindClosestEnemy()
	{
        GameObject[] objs;
        objs = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closest = objs[0];

		for (int i = 1; i < objs.Length; i++)
		{
			// get distance to enemy being checked.
			float linearDist1 = Vector3.Distance(transform.position, objs[i].transform.position);
			// get distance to currently closest enemy. not stored so has to be calculated each cycle.
			float linearDist2 = Vector3.Distance(transform.position, closest.transform.position);

            RaycastHit hitCurrent;
            RaycastHit hitClosest;

            // buff distance if enemy through obstacle.
            if (Physics.Raycast(transform.position, (objs[i].transform.position - transform.position), out hitCurrent, 15f))
            {
                if (hitCurrent.transform != transform)
                {
                    linearDist1 += 15f;
                }
            }

            // buff distance if enemy through obstacle.
			if (Physics.Raycast(transform.position, (closest.transform.position - transform.position), out hitClosest, 15f))
			{
				if (hitClosest.transform != transform)
				{
					linearDist2 += 15f;
				}
			}
             // compare distances.
			if (linearDist1 < linearDist2)
			{
                closest = objs[i];
			}
		}

        // raycast drawn to closest enemy after distance buff. this is a debugging tool and will be removed later.
        Debug.DrawLine(transform.position, closest.transform.position, Color.black);
        return closest;
    }

    float ParabolicCurve(float x)
	{
        float y = Mathf.Pow(2, ((-1f/3f) * x + 2f)) + 1;
        return y;
	}
}