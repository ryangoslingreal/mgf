using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;

	public bool collision = false;

	void Update()
	{
		rb.velocity += new Vector3(0, 0 -10f * Time.deltaTime);

		if (collision || transform.position.y <= 0f)
		{
			Destroy(this.gameObject);
		}
	}

	void SetVelocity(float velocity)
    {
		rb = GetComponent<Rigidbody>();
		rb.velocity = transform.forward * velocity; // vector = direction vector * magnitude.
	}

	private void OnTriggerEnter(Collider other)
	{
		Destroy(this.gameObject);
	}
}