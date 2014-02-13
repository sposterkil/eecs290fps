using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    private int health, battery, level;

    public static GameManager instance{
        get{
            if(instance == null){
                instance = new GameObject("GameManager").AddComponent();
            }
            return instance
        }
    }

    // Health Gettersetters.  Up and Down methods to simplify damage/heal.
    public int getHealth(){
        return health;
    }
    public void healthUp(int heal){
        health += heal;
    }
    public void healthDown(int damage){
        health -= damage;
    }

    // Battery Gettersetters.  Drain removes 1, up takes value
    public int getBattery(){
        return battery;
    }
    public void batteryDrain(){
        battery -= 1;
    }
    public void batteryUp(int juice){
        battery += juice;
    }

    // GetterSetters for Level
    public int getLevel(){
        return level;
    }
    public int setLevel(int newLevel){
        level = newLevel;
    }
}
public void OnApplicationQuit(){
    instance = null;
}
