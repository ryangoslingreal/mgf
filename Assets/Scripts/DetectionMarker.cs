using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionMarker : MonoBehaviour
{
	GameObject player;
	GameObject enemy; // enemy received from SyncUI.

	void Update()
	{
		Vector3 dirToEnemy = (enemy.transform.position - player.transform.position); // vector from player to enemy.
		float angle = Vector3.Angle(player.transform.forward, dirToEnemy); // angle from player to enemy.

		// dirToEnemy repurposed to show vector from player to enemy relative to player's local axes.
		// (x + y + z where z is player's transform.forward).
		dirToEnemy = player.transform.forward - dirToEnemy;

		//adjust angle for direction of vector between player and enemy.
		if (dirToEnemy.x < 0)
		{
			angle = -angle;
		}

		Debug.Log($"Player->Enemy vector: {dirToEnemy}");
		Debug.Log($"Angle: {angle}");

		transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
	}

	void CreateDetectionMarker(GameObject _enemy) // receive enemy from SyncUI.
	{
		enemy = _enemy;
		player = GameObject.FindGameObjectWithTag("Player");
	}
}
