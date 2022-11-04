using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public GameObject topPivot;
    public GameObject headPivot;

    float rotateSpeed = 7f;
    float torsoLeanAngle = 30f;
    float headLeanAngle = -15f;

    float movementSpeed;
    const float defaultSprintMovementSpeed = 9f;
    const float defaultWalkMovementSpeed = 4.5f;
    const float defaultCrouchMovementSpeed = 3f;

    bool crouching = false;
    bool sprinting = false;

    float defaultCrouchSpeed = 4f;
    float crouchHeight = 1.4f;
    
    void Update()
    {
        Quaternion initRotTop = topPivot.transform.localRotation;
        Quaternion initRotHead = headPivot.transform.localRotation;

        topPivot.transform.localRotation = Quaternion.Lerp(initRotTop, Quaternion.Euler(initRotTop.x, initRotTop.y, torsoLeanAngle * CheckLean()), rotateSpeed * Time.deltaTime);
        headPivot.transform.localRotation = Quaternion.Lerp(initRotHead, Quaternion.Euler(initRotTop.x, initRotTop.y, headLeanAngle * CheckLean()), rotateSpeed * Time.deltaTime);

        player.transform.Translate(movementSpeed * Time.deltaTime * CheckLRMovement(), 0, movementSpeed * Time.deltaTime * CheckFBMovement(), Space.Self);

        CheckStance();
    }

    int CheckLean()
	{
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
		{
            return 0;
		}
        else if (Input.GetKey(KeyCode.Q))
		{
            return 1;
		}
        else if (Input.GetKey(KeyCode.E))
		{
            return -1;
		}
		else
		{
            return 0;
		}
	}

    int CheckLRMovement()
	{
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
		{
            return 0;
		}
        else if (Input.GetKey(KeyCode.A))
		{
            return -1;
		}
        else if (Input.GetKey(KeyCode.D))
        {
            return 1;
        }
        else
		{
            return 0;
		}
    }

    int CheckFBMovement()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            return 0;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            return 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    void CheckStance()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouching = !crouching; 
        }
        
        if (crouching)
        {
	    	movementSpeed = defaultCrouchMovementSpeed;
			player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(player.transform.position.x, Mathf.Clamp(player.transform.position.y - crouchHeight, -1.4f, 0), player.transform.position.z), defaultCrouchSpeed * Time.deltaTime);
		}
        else
        {
			movementSpeed = defaultWalkMovementSpeed;
			player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(player.transform.position.x, Mathf.Clamp(player.transform.position.y + crouchHeight, -1.4f, 0), player.transform.position.z), defaultCrouchSpeed * Time.deltaTime);
		}

        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprinting = true;
            crouching = false;
            movementSpeed = defaultSprintMovementSpeed;
        }
        else
        {
            sprinting = false;
        }

        if (!sprinting && !crouching)
        {
            movementSpeed = defaultWalkMovementSpeed;
        }
    }
}