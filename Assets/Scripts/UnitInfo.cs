using System;

[Serializable]
public class UnitInfo {
	
	//The player number
	public int playerNo;
	//The type of unit the player is using
	public unitType type;
	//How many actions this unit has left
	public int actions = 0;
	//How many attacks this unit has left
	public int attacks = 0;

	public enum unitType {
		Knight, Swordsman, Lancer, None
	}
}
