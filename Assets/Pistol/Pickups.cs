using UnityEngine;
using System.Collections;

public class Pickups : MonoBehaviour {
    public PlayerManager player;
    public Vector3 rotationVelocity;


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationVelocity * Time.deltaTime);
	}

    void OnTriggerEnter(Collider trigger){
        switch(trigger.transform.tag){
            case "pistol":
                player.SetWeapon(PlayerManager.Weapons.Pistol);
                break;
            case "smg":
                player.SetWeapon(PlayerManager.Weapons.Submachine);
                break;
            case "sword":
                player.SetWeapon(PlayerManager.Weapons.Sword);
                break;
            default:
                Debug.Log("Entered non-weapon or bad-weapon trigger");
                break;
        }
        gameObject.SetActive(false);
    }

    void Spawn(Vector3 pos){
        if (false){
            gameObject.SetActive(true);
        }
    }
}
