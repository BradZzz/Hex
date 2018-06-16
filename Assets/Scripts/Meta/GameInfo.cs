using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class GameInfo {
	public string name;

	public int movement;
	public int fatigue;

  public int gold;
  public int rations;

	public UnitInfo[] playerRoster;
  public UnitInfo[] enemyRoster;
  public CharacterInfo.attributeType[] attributes;

  public static CharacterInfo.attributeType getAvailableAttribute(GameInfo gameI){
    String[] names = Enum.GetNames (typeof(CharacterInfo.attributeType));
    List<CharacterInfo.attributeType> attribs = new List<CharacterInfo.attributeType> (gameI.attributes);

    //Find the enum that matches the name here
    for (int i = 0; i < names.Length; i++) {
      CharacterInfo.attributeType thisAttrib = (CharacterInfo.attributeType) Enum.GetValues (typeof(CharacterInfo.attributeType)).GetValue (i);
      if (!attribs.Contains(thisAttrib)) {
        return thisAttrib;
      }
    }
    return CharacterInfo.attributeType.None;
  }

	public void PrintInfo() {
	  Debug.Log("Name: " + name);
	  Debug.Log("Movement: " + movement.ToString());
	  Debug.Log("Fatigue: " + fatigue.ToString());
    Debug.Log("Gold: " + gold.ToString());
    Debug.Log("Roster: " + playerRoster.Length);
    Debug.Log ("Attributes");
    foreach(CharacterInfo.attributeType attribute in attributes){
      Debug.Log (attribute.ToString());
    }
	}
}
