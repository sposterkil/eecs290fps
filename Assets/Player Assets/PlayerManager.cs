using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	public enum Weapons {Pistol, Submachine, Sword};
	public static Weapons wep;
	public static int ammo;
	public AudioSource heartBeat;
	public AudioSource breathing;

	public int current;
	public float health;
	public float battery;
	public float oxy;
	public bool sprinting;

	public Transform pistol;
	public Transform submachine;
	public Transform sword;
	public Transform Flashlight;


	public static bool canMove;

	// Use this for initialization
	public void Start () {
		// Start with the Pistol equipped
		wep = Weapons.Pistol;
		Flashlight.active = true;
		pistol.active = true;
		submachine.active = false;
		sword.active = false;
		current = 0;
		// Initialize player stats
		health = 100;
		battery = 100;
		oxy = 100;
		ammo = 30;
		// Tie them to the HUD
		HUDManager.SetBattery ((int)battery);
		HUDManager.SetHealth ((int)health);
		HUDManager.SetAmmo (ammo);
		HUDManager.SetOxy ((int)oxy);
		// Disable movement until game Start
		disable();
		heartBeat.volume = 1f;
		breathing.volume = 1f;
		heartBeat.Play ();
		breathing.Play ();
	}

	public void enable() {
		canMove = true;
		CharacterMotor.Enable();
	}

	public void disable() {
		canMove = false;
		CharacterMotor.Disable();
	}

	// Takes a weapon from the Weapons enum and sets it as the current weapon
	public void SetWeapon(Weapons w){
		if (canMove) {
			switch (w) {
				case Weapons.Pistol:
					pistol.active = true;
					break;
				case Weapons.Submachine:
					submachine.active = true;
					break;
				case Weapons.Sword:
					sword.active = true;
					break;
				default:
					Debug.Log("Got bad weapon type: " + w);
					break;
			}
			wep = w;
		}
	}

	void HandleBattery(){
		if (battery > 0) {
			battery -= 1 * Time.deltaTime / 2;
		}
		else{
			Flashlight.active = false;
		}
	}

	void HandleOxygen(){
		if (oxy > 0) { // have oxygen
			if (sprinting){
				oxy -= 1 * Time.deltaTime;
			}
			else{
				oxy -= 1 * Time.deltaTime /5;
			}
		}
		else {// ran out of oxygen
			health -= (2 * Time.deltaTime);
		}
	}

	// Update is called once per frame
	void Update () {
		if (canMove) {
			// Update the HUD
			HUDManager.SetBattery ((int)battery);
			HUDManager.SetHealth ((int)health);
			HUDManager.SetAmmo (ammo);
			HUDManager.SetOxy ((int)oxy);
			heartBeat.volume = (1f - (health/100));  //heart sounds
			breathing.volume = (1f - (oxy/100));

			if (Input.GetAxis("Scroll") != 0) {
				// Get rid of our current weapon
				transform.GetChild(1).animation.Stop();
				pistol.active = false;
				submachine.active = false;
				sword.active = false;
				// Switch up or down, depending on scroll direction
				if (Input.GetAxis("Scroll") < -.01)
					current += 2;
				else if (Input.GetAxis("Scroll") > .01)
					current += 1;
			}
			// Choose the new weapon
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

			if (Input.GetKeyDown ("f2")) {// kill player when f2 is pressed
					this.health = 0;
			}

			if (this.health <= 0) { // player is dead
				GameEventManager.TriggerGameOver ();
			}

			if (Input.GetKeyDown ("q")) {
				Debug.Log ("Q Button was pressed");
				Object light;
			}




		}
		else {
			transform.GetChild(1).animation.Stop();
			disable();
		}
		HandleBattery();
		HandleOxygen();
	}
}
