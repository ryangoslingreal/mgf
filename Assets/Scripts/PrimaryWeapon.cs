using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryWeapon : MonoBehaviour
{
	GameObject UI;

	public GameObject bulletPrefab;
	public GameObject primaryMuzzle;

	public float fireRate = 600f; // 600rpm = trigger pull every 0.1s.
	public float muzzleVelocity = 35f; // in m/s.
	public float reloadTime = 3.5f; // in seconds.

	public const int magMax = 30;
	public int mag = 30;

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

		if (Input.GetKey(KeyCode.Mouse0) && mag > 0 && canShoot) //shoot.
		{
			StartCoroutine(ShootingDelay());
			GameObject bullet = Instantiate(bulletPrefab, primaryMuzzle.transform.position, transform.rotation); // create bullet.
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