using UnityEngine;
using System.Collections;

public class GameEventManager : MonoBehaviour {
		
	public delegate void GameEvent();
		
	public static event GameEvent GameStart, GameOver;  // Game Events
		
	/**
	 * Starts the game
	 */
	public static void TriggerGameStart(){
		if(GameStart != null){
			GameStart();  // start the game
		}
	}
	/**
	 * Ends the game
	 */
	public static void TriggerGameOver(){
		if(GameOver != null){
			GameOver();
			GameStart = null;  // reset game state
			GameOver = null;  // reset game state
			Application.LoadLevel (0);  // restart application
		}
	}	
}

