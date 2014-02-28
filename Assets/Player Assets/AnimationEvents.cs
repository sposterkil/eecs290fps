using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {

	/**
	 * Method that does an attack depending on what weapon is equipped and if the player connects to a monster or not
	 */
	public void Attack() {
		//Aimed distance
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		int multiplier = 1; // speed multiplier
		
		if(Input.GetAxis("Sprint") < -.01)
			multiplier = 2;
	
	    // if player has a gun
		if ((PlayerManager.wep == PlayerManager.Weapons.Pistol)||(PlayerManager.wep == PlayerManager.Weapons.Submachine)) {
			if (PlayerManager.ammo > 0) { // with ammo
				PlayerManager.ammo--; // remove a bullet
				if (Physics.Raycast(transform.position, fwd, out hit, 100)) { // project raycast
					Debug.DrawLine(transform.position, hit.point, Color.green);
					if (hit.collider.gameObject.name == "monster(Clone)") { // if hit monster
						if (PlayerManager.wep == PlayerManager.Weapons.Pistol) // if pistol
							hit.transform.gameObject.GetComponent<MonsterAI>().damage(40 * multiplier); 
						else //if SMG
							hit.transform.gameObject.GetComponent<MonsterAI>().damage(20 * multiplier); 
					}
				}
			}
		}
		else if(Physics.Raycast(transform.position, fwd, out hit, 3)) // they have a sword, project raycast
			Debug.DrawLine(transform.position, hit.point, Color.red); // draw line
			if (hit.collider.gameObject.name == "monster(Clone)") // if they hit the monster
				hit.transform.gameObject.GetComponent<MonsterAI>().damage(50 * multiplier); // damage
		}
	}
