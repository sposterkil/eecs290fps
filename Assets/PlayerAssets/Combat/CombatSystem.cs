using UnityEngine;
using System.Collections;

public class CombatSystem : MonoBehaviour {
	float distGround;
	public static bool attacking;
	
	void Start() {
		attacking = false;
	}
	
	void Update() {
		//Aimed distance
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		
		if (Input.GetButton("Fire1")) {
			if(Physics.Raycast(transform.position, fwd, out hit, 3)) {
				print("Object encountered! (" + hit.distance + ")");
				Debug.DrawLine(transform.position, hit.point, Color.red);
				
				//Destroy(hit.transform.gameObject);
			}
			else if (Physics.Raycast(transform.position, fwd, out hit, 100))
					Debug.DrawLine(transform.position, hit.point, Color.green);
			attacking = true;
		}
		else
			attacking = false;
	}
}

