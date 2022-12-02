using UnityEngine;
using UnityEditor; // UnityEditor package required.

[CustomEditor (typeof (FieldOfView))] // enables FieldOfView script to function in scene mode.
public class FieldOfViewEditor : Editor
{
	void OnSceneGUI()
	{
		FieldOfView fov = (FieldOfView)target; // create instance of FieldOfView script in scene.
		Handles.color = Color.white;
		Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360f, fov.viewRadius); // draw radius.

		// angle between vector A and B will be equal to fov (view angle).
		Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
		Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

		Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius); // draw angle A.
		Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius); // draw angle B.

		Handles.color = Color.red;
		// draw raycast to each target.
		foreach (Transform visibleTarget in fov.visibleTargets)
		{
			Handles.DrawLine(fov.transform.position, visibleTarget.position);
		}
	}
}