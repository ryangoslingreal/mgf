using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SidearmWeapon : MonoBehaviour
{
	GameObject UI;

	public GameObject bulletPrefab;
	public GameObject sidearmMuzzle;

	public float fireRate = 120f; // 120rpm = trigger pull every 0.5s.
	public float muzzleVelocity = 20f; // in m/s.
	public float reloadTime = 2f; // in seconds.

	public const int magMax = 10;
	public int mag = 10;

	bool canShoot = true;

	void Start()
	{
		UI = GameObject.FindWithTag("UI"); // can’t be assigned in inspector as UI is not a prefab.
	}

	void Update()
	{
		UI.SendMessage("RefreshAmmo", mag); // send ammo count to UI.

		if (Input.GetKeyDown(KeyCode.R)) // reload.
		{
			StartCoroutine(Reload());
		}

		if (Input.GetKeyDown(KeyCode.Mouse0) && mag > 0 && canShoot) // shoot.
		{
			StartCoroutine(ShootingDelay());
			GameObject bullet = Instantiate(bulletPrefab, sidearmMuzzle.transform.position, transform.rotation); // create bullet.
			bullet.SendMessage("SetVelocity", muzzleVelocity); // set bullet's velocity.
			mag--;
		}
	}

	IEnumerator ShootingDelay()
	{
		canShoot = false;
		float timeBeforeShooting = (float)60 / (float)fireRate; // calculate time before firing.
		yield return new WaitForSeconds((float)timeBeforeShooting); // wait.
		canShoot = true;
	}

	IEnumerator Reload()
	{
		canShoot = false;
		yield return new WaitForSeconds((float)reloadTime); // wait.
		mag = magMax;
		canShoot = true;
	}
}