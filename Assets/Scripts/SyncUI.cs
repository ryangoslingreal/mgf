using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SyncUI : MonoBehaviour
{
	public GameObject player;
	PlayerController playerController;

	GameObject activeWeapon;

	public RawImage healthBG;
	RectTransform healthBGTransform;

	public Text healthText;
	RectTransform healthTextTransform;

	void Start()
	{
		playerController = player.GetComponent<PlayerController>();

		healthBGTransform = healthBG.GetComponent<RectTransform>();
		healthTextTransform = healthText.GetComponent<RectTransform>();
	}

	void Update()
	{
		int ammoCount = GetAmmoCount();

		healthText.text = ammoCount.ToString() + "/-";

		if (ammoCount > 9)
		{
			healthBGTransform.sizeDelta = new Vector2(300f, 120f);
		}
		else
		{
			healthBGTransform.sizeDelta = new Vector2(250f, 120f);
		}
		
	}

	void SetActiveWeapon(GameObject _activeWeapon)
	{
		activeWeapon = _activeWeapon;
	}

	int GetAmmoCount()
	{
		int ammoCount;

		if (activeWeapon.name == "sidearm")
		{
			healthBG.gameObject.SetActive(true);
			ammoCount = activeWeapon.GetComponent<SidearmWeapon>().mag;
		}
		else if (activeWeapon.name == "primary")
		{
			healthBG.gameObject.SetActive(true);
			ammoCount = activeWeapon.GetComponent<PrimaryWeapon>().mag;
		}
		else
		{
			healthBG.gameObject.SetActive(false);
			ammoCount = 0;
		}

		return ammoCount;
	}
}