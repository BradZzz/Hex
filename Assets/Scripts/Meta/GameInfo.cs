using System;
using UnityEngine;

[Serializable]
public class GameInfo {
	public string name;

	public int movement;
	public int fatigue;

  public int gold;

	public UnitInfo[] roster;

	public void PrintInfo() {
	  Debug.Log("Name: " + name);
	  Debug.Log("Movement: " + movement.ToString());
	  Debug.Log("Fatigue: " + fatigue.ToString());
    Debug.Log("Gold: " + gold.ToString());
	  Debug.Log("Roster: " + roster.Length);
	}
}
