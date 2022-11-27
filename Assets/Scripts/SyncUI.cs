using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SyncUI : MonoBehaviour
{
	public GameObject player;
	PlayerController playerController;

	public RawImage ammoBG;
	public Text ammoText;
	RectTransform ammoBGTransform; // required for resizing ammo count box.

	public Text healthText;

	void Start()
	{
		playerController = player.GetComponent<PlayerController>();

		ammoBGTransform = ammoBG.GetComponent<RectTransform>(); // required for resizing ammo count box.
	}

	void Update()
	{
		RefreshHealth(playerController.health); // health refresh not called in other script so called in Update().
	}

	void RefreshAmmo(int ammoCount) // ammo count received from weapon script.
	{
		ammoText.text = ammoCount.ToString() + "/-";

		if (ammoCount > 9) // resize ammo count box.
		{
			ammoBGTransform.sizeDelta = new Vector2(250f, 120f);
		}
		else
		{
			ammoBGTransform.sizeDelta = new Vector2(200f, 120f);
		}
	}

	void RefreshHealth(float playerHealth) // health retrieved manually. not set by another script.
	{
		healthText.text = playerHealth.ToString();
	}
}