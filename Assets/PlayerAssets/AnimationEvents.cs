using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {
	public void Attack() {
		//Aimed distance
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
	
		if ((PlayerManager.wep == PlayerManager.Weapons.Pistol)||(PlayerManager.wep == PlayerManager.Weapons.Submachine)) {
			PlayerManager.ammo--;
			//AnimationManager.pistolShot.Play();
		}
	
		if(Physics.Raycast(transform.position, fwd, out hit, 3)) {
			print("Object encountered! (" + hit.distance + ")");
			Debug.DrawLine(transform.position, hit.point, Color.red);
			
			//Destroy(hit.transform.gameObject);
		}
			
		else if (Physics.Raycast(transform.position, fwd, out hit, 100)) {
			Debug.DrawLine(transform.position, hit.point, Color.green);
		}
	}
}
