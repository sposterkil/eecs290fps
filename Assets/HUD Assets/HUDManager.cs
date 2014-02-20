using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

	public GUIText gameOverText, instructionText, titleText, 
	                        healthText, ammoText, batteryText;
	public GUITexture ammo, health, battery, crosshairs;
	private static HUDManager instance;
	
	public Transform player;

	// Use this for initialization
	void Start () {
		instance = this;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		gameOverText.enabled = false;
		healthText.enabled = false;
		ammoText.enabled = false;
		batteryText.enabled = false;
		ammo.enabled = false;
		health.enabled = false;
		battery.enabled = false;
		crosshairs.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump")) {
			GameEventManager.TriggerGameStart();
		}
	}

	private void GameStart () {
		gameOverText.enabled = false;
		instructionText.enabled = false;
		titleText.enabled = false;
		healthText.enabled = true;
		ammoText.enabled = true;
		batteryText.enabled = true;
		ammo.enabled = true;
		health.enabled = true;
		battery.enabled = true;
		crosshairs.enabled = true;
		enabled = false;
		SetBattery (100);
		player.GetComponent<PlayerManager>().Start();
		player.GetComponent<PlayerManager>().enable();
	}

	private void GameOver () {
		gameOverText.enabled = true;
		instructionText.enabled = true;
		enabled = true;
		healthText.enabled = false;
		ammoText.enabled = false;
		batteryText.enabled = false;
		ammo.enabled = false;
		health.enabled = false;
		battery.enabled = false;
		crosshairs.enabled = false;
		player.GetComponent<PlayerManager>().disable();
	}

	public static void SetHealth(int health){
				instance.healthText.text = (health.ToString () + "%");
		}

	public static void SetBattery(int battery){
				instance.batteryText.text = (battery.ToString () + "%");
		}

	public static void SetAmmo (int rounds) {
				instance.ammoText.text = rounds.ToString ();
		}
}
