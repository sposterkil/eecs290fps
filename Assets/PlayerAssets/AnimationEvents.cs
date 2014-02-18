using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {
	public void Attack() {
		//Aimed distance
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
	
		if ((PlayerManager.wep == PlayerManager.Weapons.Pistol)||(PlayerManager.wep == PlayerManager.Weapons.Submachine)) {
			if (PlayerManager.ammo > 0) {
				PlayerManager.ammo--;
				if (Physics.Raycast(transform.position, fwd, out hit, 100)) {
					Debug.DrawLine(transform.position, hit.point, Color.green);
				}
			}
			//AnimationManager.pistolShot.Play();
		}
		else if(Physics.Raycast(transform.position, fwd, out hit, 3)) {
			Debug.DrawLine(transform.position, hit.point, Color.red);
		}
	}
}
