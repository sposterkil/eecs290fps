using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	public static int BatteryLife;
	public static int BatteryCount;
	public static bool HasGun;
	public static bool HasFlashlight;
	// Use this for initialization
	void Start () {

		if (HasFlashlight){
			GUI.Label(new Rect(10,10,80,30),"Flashlight");
		//	GUI.Label (new Rect(70,10,80,30),Flashlight.BatteryLife.toString("F2"))
	
		if (HasGun) {
			GUI.Label (new Rect(10,10,10,30),"Gun");
			}
		GUI.Label (new Rect(10,40,80,30),"Batteries")
		GUI.Label(new Rect(70,40,80,30),BatteryCount.ToString())
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
