using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidearmWeapon : MonoBehaviour
{
	public GameObject bulletPrefab;
	public GameObject sidearmMuzzle;

	public float fireRate = 120f; // 120rpm = trigger pull every 0.5s.
	public float muzzleVelocity = 20f; // in m/s.

	public const int magMax = 10;
	public int mag = 10;

	bool canShoot = true;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0) && mag > 0 && canShoot)
		{
			StartCoroutine(ShootingDelay());
			GameObject bullet = Instantiate(bulletPrefab, sidearmMuzzle.transform.position, transform.rotation); // create bullet.
			bullet.SendMessage("SetVelocity", muzzleVelocity); // set bullet's velocity.
		}
	}

	IEnumerator ShootingDelay()
	{
		canShoot = false;
		float timeBeforeShooting = (float)60 / (float)fireRate; // calculate time before firing.
		yield return new WaitForSeconds((float)timeBeforeShooting); // wait.
		canShoot = true;
	}
}