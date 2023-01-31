using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionMarker : MonoBehaviour
{
	GameObject player;
	GameObject enemy; // enemy received from SyncUI.
	FieldOfView enemyFOV; // saved so doesn't have to be retrieved every time.
	GameObject detectionSprite; // marker's child with Image component.

	void Update()
	{
		if (enemyFOV.detection > 0) // if player is visible.
		{
			detectionSprite.SetActive(true); // enable Image.

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

			//Debug.Log($"Player->Enemy vector: {dirToEnemy}");
			//Debug.Log($"Angle: {angle}");

			transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // rotate marker.
		}
		else // if player is not visible.
		{
			detectionSprite.SetActive(false);
		}

		if (enemyFOV.detection >= 100) // if player has been detected.
		{
			Debug.Log("detected");

			StartCoroutine(FlashAndHide(0.5f));
		}
	}

	IEnumerator FlashAndHide(float delay)
	{
		for (int i = 0; i < 3; i++)
		{
			detectionSprite.SetActive(false);
			yield return new WaitForSeconds(delay); // wait.
			detectionSprite.SetActive(true);
			yield return new WaitForSeconds(delay); // wait.
		}
	}

	void CreateDetectionMarker(GameObject _enemy) // receive enemy from SyncUI.
	{
		enemy = _enemy;
		enemyFOV = enemy.GetComponent<FieldOfView>(); // get enemy's FOV script for detection.
		player = GameObject.FindGameObjectWithTag("Player"); // get player.
		detectionSprite = transform.GetChild(0).gameObject; // get marker's child containing Image component.
		detectionSprite.SetActive(false); // hide initially.
	}
}
