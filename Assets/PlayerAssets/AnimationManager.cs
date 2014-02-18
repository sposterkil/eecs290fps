using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {
	int count;
	enum Weapons {Pistol, Submachine, Sword};
	enum Animations {Attack, Idle, Running};
	Weapons wep;
	Animations anim;
	public AudioSource pistolShot;

	// Use this for initialization
	void Start () {
		anim = Animations.Idle;
	}
	
	// Update is called once per frame
	void Update () {
		string w = null;
		if (PlayerManager.wep == PlayerManager.Weapons.Pistol) w = "Pistol";
		else if (PlayerManager.wep == PlayerManager.Weapons.Submachine) w = "Submachine";
		else if (PlayerManager.wep == PlayerManager.Weapons.Sword) w = "Sword";
		
		if (!transform.GetChild(1).animation.isPlaying)
			anim = Animations.Idle;
			
		if (CombatSystem.attacking) {
			if (anim != Animations.Attack) {
				transform.GetChild(1).animation.Play("Attack" + w);
				anim = Animations.Attack;
				pistolShot.Play ();
			}
		}
		
		else if ((Input.GetAxis("Vertical") > .01)&&(anim != Animations.Attack)) {
			transform.GetChild(1).animation.Play("Running" + w);
			anim = Animations.Running;
		}
		
		if (anim == Animations.Idle)
			transform.GetChild(1).animation.Play("Idle" + w);
	}
}
