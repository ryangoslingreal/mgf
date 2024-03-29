using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    GameObject[] enemyGameObjects;
    List<MonoBehaviour> enemyControllers = new List<MonoBehaviour>();

    public GameObject player;
	public GameObject cam;
	public GameObject topPivot;
    public GameObject headPivot;
    public GameObject wpnAnchor;

	const float rotateSpeed = 7f;
    const float torsoLeanAngle = 30f;
    const float headLeanAngle = -15f;

    float movementSpeed;
    const float defaultSprintMovementSpeed = 9f;
    const float defaultWalkMovementSpeed = 4.5f;
    const float defaultCrouchMovementSpeed = 3f;

    bool crouching = false;
    bool sprinting = false;
    bool walkingFB = false;

    float defaultCrouchSpeed = 4f;
    float crouchHeight = 1.4f;

    public bool canAim = true;
    float sensitivity = 1000f;
    private float rotY = 0f;
    private float rotX = 0f;
    float maxHeadTiltAngle = 60f;
    
    public GameObject primary;
    public GameObject sidearm;
    public GameObject melee;

    public const float healthMax = 100f;
    public float health = 100f;

    public float detection = 0;
    public bool detected = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        primary.SetActive(false);
        sidearm.SetActive(true);
        melee.SetActive(false);
    }

    void Update()
    {
        RefreshEnemyReferences();

		// mouse delta in pixels * sensitivity * time since last frame / screen width * 100.
		// returns axis rotation for current frame in degrees.
		float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime / Screen.width * 100;
		float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime / Screen.height * 100;

		rotY += mouseX;
		rotX -= mouseY;

		rotX = Mathf.Clamp(rotX, -maxHeadTiltAngle, maxHeadTiltAngle);

        if (canAim) // toggle for debugging.
        {
			player.transform.eulerAngles = new Vector3(0f, rotY, 0f); // body rotated left/right.
			cam.transform.localEulerAngles = new Vector3(rotX, 0f, 0f); // head tilted up/down.
		}
		
        // saving current torso and head rotation to shorten reference in successive lines.
		Quaternion initRotTop = topPivot.transform.localRotation;
        Quaternion initRotHead = headPivot.transform.localRotation;

        // torso rotated in direction of input.
        topPivot.transform.localRotation = Quaternion.Lerp(initRotTop, Quaternion.Euler(initRotTop.x, initRotTop.y, torsoLeanAngle * CheckLean()), rotateSpeed * Time.deltaTime);
		// head rotated in opposite direction of input.
		headPivot.transform.localRotation = Quaternion.Lerp(initRotHead, Quaternion.Euler(initRotTop.x, initRotTop.y, headLeanAngle * CheckLean()), rotateSpeed * Time.deltaTime);

		// player translated every frame by their speed * time since last frame if they have pressed movement key.
		player.transform.Translate(movementSpeed * Time.deltaTime * CheckLRMovement(), 0, movementSpeed * Time.deltaTime * CheckFBMovement(), Space.Self);

        CheckStance();

        WeaponControl();

		UpdateAnimator();
    }

    // returns dir of lean.
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

    // returns dir of lr movement.
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

    // returns dir of fb movement.
    int CheckFBMovement()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            walkingFB = false;
            return 0;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            walkingFB = true;
            return 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            walkingFB = true;
            return -1;
        }
        else
        {
            walkingFB = false;
            return 0;
        }
    }

    // checks crouching and sprinting, switches between movement speeds. 
    void CheckStance()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouching = !crouching; 
        }
        
        if (crouching)
        {
	    	movementSpeed = defaultCrouchMovementSpeed;
            // move player down smoothly.
			player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(player.transform.position.x, Mathf.Clamp(player.transform.position.y - crouchHeight, -1.4f, 0), player.transform.position.z), defaultCrouchSpeed * Time.deltaTime);
		}
        else
        {
			movementSpeed = defaultWalkMovementSpeed;
            // move player up smoothly.
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

    // updates animator parameters.
    void UpdateAnimator()
    {
        animator.SetBool("walkingFB", walkingFB);
    }

    // allows weapon switching.
    void WeaponControl()
    {
		if (Input.GetKey(KeyCode.Alpha1))
		{
			primary.SetActive(true);
			sidearm.SetActive(false);
			melee.SetActive(false);
		}
		else if (Input.GetKey(KeyCode.Alpha2))
		{
			primary.SetActive(false);
			sidearm.SetActive(true);
			melee.SetActive(false);
		}
		// allow quick switching primary and sidearm if not using melee.
		else if (Input.GetKeyDown(KeyCode.C) && !melee.activeInHierarchy) 
		{
			primary.SetActive(!primary.activeInHierarchy);
			sidearm.SetActive(!sidearm.activeInHierarchy);
		}
		else if (Input.GetKey(KeyCode.Alpha3))
		{
			primary.SetActive(false);
			sidearm.SetActive(false);
			melee.SetActive(true);
		}
	}

    // refreshes enemy script references.
    void RefreshEnemyReferences() 
    {
		enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy"); // new values override old ones.

		enemyControllers.Clear(); // list cleared.

		foreach (GameObject enemy in enemyGameObjects)
		{
			enemyControllers.Add(enemy.GetComponent<EnemyController>());
		}
	}
}