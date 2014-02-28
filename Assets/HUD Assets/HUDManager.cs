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

		// set the screen texts for  initial title and instructions
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

	/**
	 * Starts the game, turns off the start menu turns on the hud
	 */
	private void GameStart () {

		// set the HUD for gameplay
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

	/**
	 * This triggers the hud to display game over
	 */
	private void GameOver () {

		// disable HUD and set the game to end message
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

	/**
	 * setter for the health to the hud
	 * @param health The player health to display
	 */
	public static void SetHealth(int health){
				instance.healthText.text = (health.ToString () + "%");
		}

	/**
	 * setter for the battery to the hud
	 * @param battery The player battery to display
	 */
	public static void SetBattery(int battery){
				instance.batteryText.text = (battery.ToString () + "%");
		}

	/**
	 * setter for the oxygen to the hud
	 * @param oxy The player oxygen level to display
	 */
	public static void SetOxy(int oxy) {
				instance.oxyText.text = (oxy.ToString () + "%");
		}

	/**
	 * setter for the ammo to the hud
	 * @param ammo The player ammo to display
	 */
	public static void SetAmmo (int rounds) {
				instance.ammoText.text = rounds.ToString ();
		}
}
