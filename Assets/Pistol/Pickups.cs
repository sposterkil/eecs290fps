using UnityEngine;
using System.Collections;

public class Pickups : MonoBehaviour {
    public PlayerManager player;
    public Vector3 rotationVelocity;


	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
	}

	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationVelocity * Time.deltaTime);
	}

    void OnTriggerEnter(Collider trigger){
        switch(trigger.transform.tag){
            case "AmmoPickup":
                PlayerManager.ammo += 60;
                break;
            case "BattPickup":
                player.battery += 100;
                break;
            default:
                Debug.Log("Entered non-weapon or bad-weapon trigger");
                break;
        }
        gameObject.Destroy(trigger);
    }
}
