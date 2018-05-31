using System;
using UnityEngine;

[Serializable]
public class GameInfo {
	public string name;

	public int movement;
	public int fatigue;

	public UnitInfo[] roster;
}
