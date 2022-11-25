using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
	public GameObject UI;

	void Update()
	{
		UI.SendMessage("SetActiveWeapon", this.gameObject);
	}
}
