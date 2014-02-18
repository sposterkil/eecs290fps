using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	public enum Weapons {Pistol, Submachine, Sword};
	public static Weapons wep;
	public int current;
	public int health, battery;
	public static int ammo;

	public Transform pistol;
	public Transform submachine;
	public Transform sword;

	// Use this for initialization
	void Start () {
		wep = Weapons.Pistol;
		pistol.active = true;
		health = battery = 100;
		ammo = 60;
		HUDManager.SetBattery (battery);
		HUDManager.SetHealth (health);
		HUDManager.SetAmmo (ammo);
	}
	
	// Update is called once per frame
	void Update () {
		HUDManager.SetBattery (battery);
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

		if (this.health <= 0) { // player is dead
			GameEventManager.TriggerGameOver ();
		}

		if (Input.GetKeyDown ("f2")) {// kill player when f2 is pressed
				this.health = 0;
		}
	}
}
