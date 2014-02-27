using UnityEngine;
using System.Collections;

public class DestroyAfter10 : MonoBehaviour {

	/**
	 * On initialization, sets the ragdoll to be destroyed in 10 seconds (presumably, the player will not be looking when this occurs)
	 */
	void Start () {
		DestroyObject (this.gameObject, 10f);
	}

	/**
	 * Ragdolls just lie around and do nothing, so the Update method is nonfunctional.
	 */
	void Update () {
	}
}
