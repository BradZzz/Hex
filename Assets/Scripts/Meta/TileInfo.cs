using System;

[Serializable]
public class TileInfo {
	
	//The type of unit the player is using
	public tileType type = TileInfo.tileType.None;
	//How many actions this unit has left
	public tileColor color = TileInfo.tileColor.None;
	//The movement penalty / bonus for landing on this tile
	public int movement = 0;
	//Fog of war is active on this tile
	public bool fog = false;
	//If this tile has an interaction stored in it
	public bool interaction = false;

	public string meta = "";

	public enum tileType {
		Castle, City, Forest, Grass, Road, Mountain, Treasure, Water,  None
	}

	public enum tileColor {
		Purple, Brown, DarkGreen, Green, Sand, Gray, Gold, Blue,  None
	}
}
