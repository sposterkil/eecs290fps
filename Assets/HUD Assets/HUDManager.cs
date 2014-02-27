using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

	public GUIText gameOverText, instructionText, titleText, // title screen texts
	                        healthText, ammoText, batteryText, oxyText; // hud texts
	public GUITexture ammo, health, battery, crosshairs, oxy; // hud icons
	private static HUDManager instance;
	
	public Transform player; // the player

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
		oxyText.enabled = false;
		oxy.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump")) {  //start the game
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
		oxyText.enabled = true;
		oxy.enabled = true;
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
		oxyText.enabled = false;
		oxy.enabled = false;
		player.GetComponent<PlayerManager>().disable();
	}

	// setter for the health to the hud
	public static void SetHealth(int health){
				instance.healthText.text = (health.ToString () + "%");
		}

	// setter for the battery to the hud
	public static void SetBattery(int battery){
				instance.batteryText.text = (battery.ToString () + "%");
		}

	// setter for the the oxygen to the hud
	public static void SetOxy(int oxy) {
				instance.oxyText.text = (oxy.ToString () + "%");
		}

	// setter for the ammo to the hud
	public static void SetAmmo (int rounds) {
				instance.ammoText.text = rounds.ToString ();
		}
}
