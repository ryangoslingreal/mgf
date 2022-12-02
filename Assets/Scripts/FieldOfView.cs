using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)] // clamp enemy’s view angle between 0 and 360 in inspector.
	public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

	void Start()
	{
        StartCoroutine("FindTargetsWithDelay", 0.2f);
	}

	IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
			yield return new WaitForSeconds(delay * Time.deltaTime);
            FindVisibleTargets();

			gameObject.SendMessage("FindPlayerInListOfDetectedTargets", visibleTargets); // send list to enemy.
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
}