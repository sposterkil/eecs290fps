using UnityEngine;
using System.Collections;

public class DestroyAfter10 : MonoBehaviour {
	//The Player's collider
	Collider _player;
	//The Ragdoll's collider
	Collider _ragdoll;

	/**
	 * On initialization, sets the ragdoll to be destroyed in 10 seconds (presumably, the player will not be looking when this occurs)
	 * Also sets the Ragdoll's collider to not collide with the Player. (Originally, the invisible collider left behind would act as a invisible pillar blocking the player)
	 */
	void Start () {
		DestroyObject (this.gameObject, 10f);
		_player = GameObject.Find ("Player").GetComponent<Collider> ();
		_ragdoll = GetComponent<Collider> ();
		Physics.IgnoreCollision (_player, _ragdoll);
	}

	/**
	 * Ragdolls just lie around and do nothing, so the Update method is nonfunctional.
	 */
	void Update () {
	}
}
