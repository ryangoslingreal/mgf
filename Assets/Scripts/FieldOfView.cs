using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
	EnemyController enemyController;

    public float viewRadius;
    [Range(0, 360)] // clamp enemy’s view angle between 0 and 360 in inspector.
	public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

	public float detection;
	public bool isPlayerVisible;

	void Start()
	{
		enemyController = this.GetComponent<EnemyController>();

        StartCoroutine("FindTargetsWithDelay", 0.1f);
		StartCoroutine("IncrementDetection", 0.005f);
	}

	IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
			yield return new WaitForSeconds(delay * Time.deltaTime);
            FindVisibleTargets();
		}
    }

	IEnumerator IncrementDetection(float delay)
	{
		while (true)
		{
			isPlayerVisible = FindPlayerInListOfDetectedTargets(); // check if player is in list of targets.

			if (isPlayerVisible)
			{
				detection = Mathf.Clamp(detection += 1, 0, 100); // increment if player is visible.
				

				if (detection == 100)
				{
					enemyController.playerDetected = true; // player has been fully detected.
				}
			}
			else if (!isPlayerVisible && !enemyController.playerDetected)
			{
				detection = Mathf.Clamp(detection -= 1, 0, 100); // decrement if player is not visible.
			}
			
			yield return new WaitForSeconds(delay); // wait.
		}
	}

	void FindVisibleTargets()
    {
        visibleTargets.Clear();

		// fill array with all colliders of targets that interact with an overlap sphere.
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask); 

        for (int i = 0; i < targetsInViewRadius.Length; i++) // for each target in the array of colliders.
        {
            Transform target = targetsInViewRadius[i].transform; // store transform of current target.
            Vector3 dirToTarget = (target.position - transform.position).normalized; // get unit vector of dir to target.

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) // if target is in field of view.
            {
                float distToTarget = Vector3.Distance(transform.position, target.position); // get distance to target.

				if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask)) // if raycast doesn't collide with obstacle.
                {
					// ensure that target's root isn't already in list.
					if (!visibleTargets.Contains(target.transform.root))
                    {
						visibleTargets.Add(target.transform.root); // store visible enemy.
					}
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) // returns unit vector of dir.
	{
        if (!angleIsGlobal) // convert local angle to global angle.
		{
            angleInDegrees += transform.eulerAngles.y;
        }

		// formula for converting angle to unit vector can be found here:
        // https://www.softschools.com/math/pre_calculus/direction_angles_of_vectors/
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)); 
    }

	public bool FindPlayerInListOfDetectedTargets()
	{
		bool listContainsPlayer = false;

		if (visibleTargets.Count > 0) // check that list isn’t empty to avoid out of bounds error.
		{
			for (int i = 0; i < visibleTargets.Count; i++) // iterate through list.
			{
				if (visibleTargets[i].transform.gameObject.tag == "Player") // if current target is player.
				{
					listContainsPlayer = true;
					break;
				}
			}
		}

		return listContainsPlayer;
	}
}