using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryWeapon : MonoBehaviour
{
	public GameObject bulletPrefab;
	public GameObject primaryMuzzle;

	public float fireRate = 600f; // 300rpm = trigger pull every 0.1s.
	public float muzzleVelocity = 35f; // in m/s.

	public const int magMax = 30;
	public int mag = 30;

	bool canShoot = true;

	void Update()
	{
		if (Input.GetKey(KeyCode.Mouse0) && mag > 0 && canShoot)
		{
			StartCoroutine(ShootingDelay());
			GameObject bullet = Instantiate(bulletPrefab, primaryMuzzle.transform.position, transform.rotation); // create bullet.
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