using System;

[Serializable]
public class TileInfo {
	
	//The type of unit the player is using
	public tileType type;
	//How many actions this unit has left
	public string color = "";
	//How many attacks this unit has left
	public int movement = 0;

	public string meta = "";

	public enum tileType {
		Castle, City, Forest, Grass, Mountain, Treasure, Water,  None
	}
}
