using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	public enum Weapons {Pistol, Submachine, Sword};
	public static Weapons wep;
	public int current;
	public int health;
	public float battery;
	public static int ammo;

	public Transform pistol;
	public Transform submachine;
	public Transform sword;
	public Transform Flashlight;

	public static bool enabled;

	// Use this for initialization
	public void Start () {
		wep = Weapons.Pistol;
		Flashlight.active = true;
		pistol.active = true;
		submachine.active = false;
		sword.active = false;
		current = 0;
		health = 100;
		battery = 100;
		ammo = 60;
		HUDManager.SetBattery ((int)battery);
		HUDManager.SetHealth (health);
		HUDManager.SetAmmo (ammo);
		disable();
	}

	public void enable() {
		enabled = true;
		CharacterMotor.Enable();
	}

	public void disable() {
		enabled = false;
		CharacterMotor.Disable();
	}

	public void SetWeapon(Weapons w){
		if (enabled) {
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

	// Update is called once per frame
	 void Update () {
		if (enabled) {
			HUDManager.SetBattery ((int)battery);
			HUDManager.SetHealth (health);
			HUDManager.SetAmmo (ammo);

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

			if (Input.GetKeyDown ("f2")) {// kill player when f2 is pressed
					this.health = 0;
			}

			if (this.health <= 0) { // player is dead
				GameEventManager.TriggerGameOver ();
			}

			if (battery > 0) {
				battery -= .001f;
			}
			else{
				Flashlight.active = false;
			}
		}
		else {
			transform.GetChild(1).animation.Stop();
			disable();
		}
	}
}
