using System;
using UnityEngine;

[Serializable]
public class GameInfo {
	public string name;

	public int movement;
	public int fatigue;

  public int gold;

	public UnitInfo[] playerRoster;
  public UnitInfo[] enemyRoster;
  public string[] attributes;

	public void PrintInfo() {
	  Debug.Log("Name: " + name);
	  Debug.Log("Movement: " + movement.ToString());
	  Debug.Log("Fatigue: " + fatigue.ToString());
    Debug.Log("Gold: " + gold.ToString());
    Debug.Log("Roster: " + playerRoster.Length);
    Debug.Log ("Attributes");
    foreach(string attribute in attributes){
      Debug.Log (attribute);
    }
	}
}
