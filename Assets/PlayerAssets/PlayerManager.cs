﻿using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	public enum Weapons {Pistol, Submachine, Sword};
	public static Weapons wep;
	public int current;
	
	public Transform pistol;
	public Transform submachine;
	public Transform sword;
	
	// Use this for initialization
	void Start () {
		wep = Weapons.Pistol;
		pistol.active = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Scroll") != 0) {
			transform.GetChild(1).animation.Stop();
			pistol.active = false;
			submachine.active = false;
			sword.active = false;
			if (Input.GetAxis("Scroll") < -.01)
				current += 2;
			else if (Input.GetAxis("Scroll") > .01)
				current += 1;
		}
		current %= 3;
		switch (current) {
			case 0: 
				wep = Weapons.Pistol;
				pistol.active = true;
				break;
			case 1: 
				wep = Weapons.Submachine;
				submachine.active = true;
				break;
			case 2: 
				wep = Weapons.Sword;
				sword.active = true;
				break;
		}
	}
}
