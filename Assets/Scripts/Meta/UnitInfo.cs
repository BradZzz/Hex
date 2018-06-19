using System;

[Serializable]
public class UnitInfo {
	
	//The player number
	public int playerNo;
	//Non-AI player
	public bool human = false;
	//The type of unit the player is using
	public unitType type;
	//How many actions this unit has left
	public int actions = 0;
	//How many attacks this unit has left
	public int attacks = 0;
	//How much health this unit has
	public int health = 0;
	//If the background color should be clear or not
	public bool clear = false;

	public enum unitType {
		Knight, Swordsman, Lancer, Adventure, None
	}

  public string unitSymbol(){
    switch(type){
    case UnitInfo.unitType.Knight:
      return "K";
    case UnitInfo.unitType.Lancer:
      return "L";
    case UnitInfo.unitType.Swordsman:
      return "S";
    }
    return "";
  }
}
