using System;
using UnityEngine;

[Serializable]
public class ChoiceInfo {
	public string name;
  [HideInInspector]
	public string image;
	[TextArea(3, 3)]
	public string openingGreeting;
	[TextArea(3, 3)]
	public string winningGreeting;
	[TextArea(3, 3)]
	public string losingGreeting;
	public OptionInfo[] options;
  public DifficultyType difficulty;

  //Odds of character appearing on this type of tile = 100%
  public TileInfo.tileType firstLocation;
  //Odds of character appearing on this type of tile = 50% other chars
  public TileInfo.tileType secondLocation;
  //Odds of character appearing on this type of tile = 25% other chars
  public TileInfo.tileType thirdLocation;

  public enum DifficultyType {
    Easy, Medium, Hard, Insane, None
  }
}
