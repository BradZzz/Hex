using System;
using UnityEngine;

[Serializable]
public class BattleInfo {
  public UnitInfo[] playerRoster;
  public UnitInfo[] enemyRoster;

	public bool won = false;
  public string redirect;
}
