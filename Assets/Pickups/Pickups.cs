using UnityEngine;
using System.Collections;

public class Pickups : MonoBehaviour {
    public PlayerManager player;  // the player
    public Vector3 rotationVelocity;  // the rotation of pickups
	
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();  // get the player
	}
	
	void Update () {
        transform.Rotate(rotationVelocity * Time.deltaTime);  // rotate the pickups about
	}

	/**
	 * Handles touching pickups and their status changes
	 * @param other The collider that the player is interacting with of the object to trigger
	 */
    void OnTriggerEnter(Collider other){  
        if (other.tag == "Player"){
            Debug.Log("Entered Trigger " + this.tag);
            switch(this.tag){ // grab tag of the triggered object
                case "AmmoPickup":
                    Debug.Log("Picked up Ammo");
                    PlayerManager.ammo = Mathf.Min (40, PlayerManager.ammo + 5); // max ammo at 40, incriment 5
                    break;
                case "BattPickup":
                    Debug.Log("Picked up Battery");
                    player.battery = Mathf.Min(100, (int)player.battery + 20); // max battery at 100, incriment 20
                    break;
                case "HPPickup":
                    Debug.Log("Picked up Health");
                    player.health = Mathf.Min(100, player.health + 20); // max health at 100, incriment 20
                    break;
				case "OxyPickup":
					Debug.Log ("Picked up Oxygen");
					player.oxy = Mathf.Min(100, player.health + 20); // max oxygen at 100, incriment 20
					break;
                default: // if the trigger did not have a tag set
                    Debug.Log("bad trigger");
                    break;
            }
            Object.Destroy(this.gameObject); // remove upon touching
        }
    }
}
