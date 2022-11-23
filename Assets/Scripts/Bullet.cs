using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;

	public bool collision = false;

	public List<string> tagsToIgnore = new List<string>();

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
		// check that collision object's tag isn't in list of ignored tags.
		if (!tagsToIgnore.Contains(other.gameObject.tag))
		{
			Destroy(this.gameObject);
		}
	}
}