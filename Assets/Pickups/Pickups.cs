using UnityEngine;
using System.Collections;

public class Pickups : MonoBehaviour {
    public PlayerManager player;  // the player
    public Vector3 rotationVelocity;  // the rotation of pickups


	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();  // get the player
	}

	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationVelocity * Time.deltaTime);  // rotate the pickups about
	}

	// takes a collider and grabs the tag of the object that was triggered to decide behavior
    void OnTriggerEnter(Collider other){  
        if (other.tag == "Player"){
            Debug.Log("Entered Trigger " + this.tag);
            switch(this.tag){
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
