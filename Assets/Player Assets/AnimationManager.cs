using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {
	int count;
	enum Weapons {Pistol, Submachine, Sword}; // types of weapons
	enum Animations {Attack, Idle, Running, Sprinting}; // types of animations
	Weapons wep; // which weapon is currently equiped
	Animations anim; // animation holder
	public AudioSource pistolShot;  // sound for pistol
	public AudioSource smgShot; // sound for SMG shot
	public AudioSource swordSound; // sound for sword swipe

	// Use this for initialization
	void Start () {
		anim = Animations.Idle;  // start in the idle animation
	}

	// Update is called once per frame
	void Update () {
		if (PlayerManager.canMove) { // if they can move
			string w = null;
			if (PlayerManager.wep == PlayerManager.Weapons.Pistol) w = "Pistol";
			else if (PlayerManager.wep == PlayerManager.Weapons.Submachine) w = "Submachine";
			else if (PlayerManager.wep == PlayerManager.Weapons.Sword) w = "Sword";

			if (!transform.GetChild(1).animation.isPlaying) //if there isn't an animation playing
				anim = Animations.Idle; // idle animation

			if (CombatSystem.attacking) { // if player attacks
					if (anim != Animations.Attack) { // if the player is not already attacking
						if ((((PlayerManager.wep == PlayerManager.Weapons.Pistol)
							||(PlayerManager.wep == PlayerManager.Weapons.Submachine))
							&&(PlayerManager.ammo > 0)) // make sure they have ammo if they ahve a gun

							||(PlayerManager.wep == PlayerManager.Weapons.Sword)) { // or they have a sword
								transform.GetChild(1).animation.Play("Attack" + w);
								anim = Animations.Attack; // play the animation for that weapon
						if (PlayerManager.wep == PlayerManager.Weapons.Pistol)
								pistolShot.Play (); // if they shot pistol, play pistol sound
						else if (PlayerManager.wep == PlayerManager.Weapons.Submachine)
							smgShot.Play (); // if they shot SMG, play sound
						else if (PlayerManager.wep == PlayerManager.Weapons. Sword)
							swordSound.Play (); // if they used sword, play sound
					}
				}
			}
			
			else if (anim != Animations.Attack) {
				bool forward = Input.GetAxis("Vertical") > .01;
				if (Input.GetAxis("Sprint") > .01) {
					if (forward) {
						transform.GetChild(1).animation.Play("Sprinting" + w);
						anim = Animations.Sprinting;
					}
					else {
						transform.GetChild(1).animation.Play("Running" + w);
						anim = Animations.Running;
					}
				}
				else if ((forward)&&!(Input.GetAxis("Sprint") < -.01)) {
					transform.GetChild(1).animation.Play("Running" + w);
					anim = Animations.Running;
				}
			}

			if (anim == Animations.Idle)
				transform.GetChild(1).animation.Play("Idle" + w);
		}
	}
}
