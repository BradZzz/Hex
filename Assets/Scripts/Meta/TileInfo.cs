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

	public static string getName(TileInfo.tileType type){
    switch(type){
      case tileType.Castle:
        return "Castle";
      case tileType.City:
        return "City";
      case tileType.Forest:
        return "Forest";
      case tileType.Grass:
        return "Grass";
      case tileType.Road:
        return "Road";
      case tileType.Mountain:
        return "Mountain";
      case tileType.Sand:
        return "Sand";
      case tileType.Treasure:
        return "Treasure";
      case tileType.Water:
        return "Water";
      case tileType.eSpawn:
        return "Enemy Spawn";
      case tileType.pSpawn:
        return "Player Spawn";
      default:
        return "None";
    }
	}

	public enum tileType {
		Castle, City, Forest, Grass, Road, Mountain, Sand, Treasure, Water, eSpawn, pSpawn, None
	}

	public enum tileColor {
		Purple, Brown, DarkGreen, Green, Sand, Gray, Gold, Black, Blue, White, Blurple, None
	}
}
