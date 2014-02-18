using UnityEngine;
using System.Collections;

public class CombatSystem : MonoBehaviour {
	public static bool attacking;
	
	void Start() {
		attacking = false;
	}
	
	void Update() {
		if (Input.GetButton("Fire1")) {
			attacking = true;
		}
		else
			attacking = false;
	}
}

