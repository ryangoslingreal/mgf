using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidearmWeapon : MonoBehaviour
{
	public GameObject bulletPrefab;
	public GameObject sidearmMuzzle;

	// based on FN 5.7. https://en.wikipedia.org/wiki/FN_Five-seven.
	public float fireRate = 120f; // 120rpm = trigger pull every 0.5s.
	public float muzzleVelocity = 762f; // in m/s.

	public const int magMax = 10;
	public int mag = 10;

	bool canShoot = true;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0) && mag > 0 && canShoot)
		{
			GameObject bullet = Instantiate(bulletPrefab, sidearmMuzzle.transform.position, Quaternion.identity); // create bullet.
			bullet.SendMessage("SetVelocity", muzzleVelocity); // set bullet's velocity and direction.
		}
	}
}