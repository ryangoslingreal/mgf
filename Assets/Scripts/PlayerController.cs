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
    float headLeanAngle = 15f;

    float lrMovementSpeed = 5f;
    float fbMovementSpeed = 7f;
    
    void Update()
    {
        Quaternion initRotTop = topPivot.transform.localRotation;
        Quaternion initRotHead = headPivot.transform.localRotation;

        topPivot.transform.localRotation = Quaternion.Lerp(initRotTop, Quaternion.Euler(initRotTop.x, initRotTop.y, torsoLeanAngle * CheckLean()), rotateSpeed * Time.deltaTime);
        headPivot.transform.localRotation = Quaternion.Lerp(initRotHead, Quaternion.Euler(initRotTop.x, initRotTop.y, -headLeanAngle * CheckLean()), rotateSpeed * Time.deltaTime);

        player.transform.Translate(lrMovementSpeed * Time.deltaTime * CheckLRMovement(), 0, fbMovementSpeed * Time.deltaTime * CheckFBMovement(), Space.Self);
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
}