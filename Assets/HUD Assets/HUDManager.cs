using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

	public GUIText gameOverText, instructionText, titleText;

	// Use this for initialization
	void Start () {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		gameOverText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
				if (Input.GetMouseButtonDown (0)) {
			 			GameEventManager.TriggerGameStart();
				}
		}

	private void GameStart () {
		gameOverText.enabled = false;
		instructionText.enabled = false;
		titleText.enabled = false;
		enabled = false;
	}

	private void GameOver () {
		gameOverText.enabled = true;
		instructionText.enabled = true;
		enabled = true;
	}
}
