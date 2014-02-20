using UnityEngine;
using System.Collections;

public class Pickups : MonoBehaviour {
    public PlayerManager player;
    public Vector3 rotationVelocity;


	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
	}

	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationVelocity * Time.deltaTime);
	}

    void OnTriggerEnter(Collider trigger){
        Debug.Log("Entered Trigger " + this.tag);
        switch(this.tag){
            case "AmmoPickup":
                Debug.Log("Picked up Ammo");
                PlayerManager.ammo += 60;
                break;
            case "BattPickup":
                Debug.Log("Picked up Battery");
                player.battery = Mathf.Min(100, (int)player.battery + 20);
                break;
            case "HPPickup":
                Debug.Log("Picked up Health");
                player.health = Mathf.Min(100, player.health + 20);
                break;
            default:
                Debug.Log("bad trigger");
                break;
        }
        Object.Destroy(this.gameObject);
    }
}
