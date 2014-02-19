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
					if (hit.collider.gameObject.name == "monster(Clone)") {
						if (PlayerManager.wep == PlayerManager.Weapons.Pistol)
							hit.transform.gameObject.GetComponent<MonsterAI>().damage(40);
						else
							hit.transform.gameObject.GetComponent<MonsterAI>().damage(20);
					}
				}
			}
		}
		else if(Physics.Raycast(transform.position, fwd, out hit, 3)) {
			Debug.DrawLine(transform.position, hit.point, Color.red);
			if (hit.collider.gameObject.name == "monster(Clone)")
				hit.transform.gameObject.GetComponent<MonsterAI>().damage(50);
		}
	}
}
