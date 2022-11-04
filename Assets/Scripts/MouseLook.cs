using UnityEngine;

public class MouseLook : MonoBehaviour
{
	float mouseX;
	float mouseY;

	public float sensitivity = 100f;
	public GameObject body;
	public GameObject head;
	float rotation = 0f;

	public float minAngle = -90f;
	public float maxAngle = 90f;

	void Update()
	{
		mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
		mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

		body.transform.localRotation = Quaternion.Lerp(body.transform.rotation.x + mouseX, body.transform.rotation.y, body.transform.rotation.z, Time.deltaTime * 7f);
	}
}