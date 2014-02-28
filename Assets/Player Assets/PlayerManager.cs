using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	//list of avaliable weapons
	public enum Weapons {Pistol, Submachine, Sword}; 
	//the current weapon
	public static Weapons wep;
	// the number of bullets avaliable for shooting
	public static int ammo;
	// the sound for heartbeating
	public AudioSource heartBeat;
	// the sound for breathing
	public AudioSource breathing;

	// which is the current weapon
	public int current;
	// player health
	public float health;
	// player battery
	public float battery;
	// player oxygen
	public float oxy;
	// is the player sprinting or not?
	public bool sprinting;

	// prefabs for the guns and flashlight
	public Transform pistol;
	public Transform submachine;
	public Transform sword;
	public Transform Flashlight;

	// can the player move or not?
	public static bool canMove;

	// Use this for initialization
	public void Start () {
		// Start with the Pistol equipped
		wep = Weapons.Pistol; 
		Flashlight.active = true;  // flashlight is on
		pistol.active = true;  // pistol is the current weapon
		submachine.active = false; // SMG is turned off
		sword.active = false; // sword is turned off
		current = 0; // the current gun's value
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

		// start the volume for health and oxy at max at the menu
		heartBeat.volume = 1f;
		breathing.volume = 1f;
		heartBeat.Play (); // start the sounds
		breathing.Play ();
	}


	/**
	 * allows the player to move
	 */
	public void enable() {
		canMove = true;
		CharacterMotor.Enable();
	}

	/**
	 * disables player movement
	 */
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

	/**
	 * handles the rate of battery drop, also turns it off when zero
	 */
	void HandleBattery(){
		if (battery > 0) {
			battery -= 1 * Time.deltaTime * 2;
			Flashlight.active = true;  // set on
		}
		else{
			Flashlight.active = false;  // set off
		}
	}


	/**
	 *  handles the rate of oxygen drop, if lower then zero damage health
	 */
	void HandleOxygen(){
		if (oxy > 0) { // have oxygen
			if (sprinting){ // remove more oxygen if sprinting
				oxy -= 1 * Time.deltaTime;
			}
			else{ // not sprinting
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
			// set the sounds so they get louder as values get closer to 0
			heartBeat.volume = (1f - (health/100));  
			breathing.volume = (1f - (oxy/100));

			// scrolls through the weapons using the mouse wheel
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

			// if the player's health drops below 0 they is dead
			if (this.health <= 0) { 
				GameEventManager.TriggerGameOver ();  // end game
			}

		}
		else { // keep the player from moving
			transform.GetChild(1).animation.Stop();
			disable();
		}
		HandleBattery(); // every frame, handle battery
		HandleOxygen();  // handle oxygen
	}
}
