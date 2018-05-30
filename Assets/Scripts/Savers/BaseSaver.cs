using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSaver {

	private static string CHOICE = "choice";
	private static string CHOICE_PICKED = "choice_pick";

	//The info of the objects on the tile
	private static string BOARD_UNIT = "board_unit";
	// The info for the tiles themselves
	private static string BOARD_TILE = "board_tile";

	public static void resetChoice() {
		PlayerPrefs.SetString (CHOICE, "");
		PlayerPrefs.SetString (CHOICE_PICKED, "");

		Debug.Log ("Choices reset");
	}

	public static void putChoice(ChoiceInfo choice, int picked) {
		string json = JsonUtility.ToJson (choice);
		PlayerPrefs.SetString (CHOICE, json);
		PlayerPrefs.SetString (CHOICE_PICKED, picked.ToString());

		Debug.Log ("Choices set: " + json);
		Debug.Log ("Choices set: " + picked.ToString());
	}


	public static ChoiceInfo getChoice(){
		string json = PlayerPrefs.GetString (CHOICE);
		if (json.Length == 0) {
			return null;
		}
		return JsonUtility.FromJson<ChoiceInfo> (json);

		Debug.Log ("Choices got");
	}

	public static int getPicked(){
		string json = PlayerPrefs.GetString (CHOICE_PICKED);
		if (json.Length == 0) {
			return -1;
		}
		return int.Parse (json);

	}

	public static void resetBoard(){
		PlayerPrefs.SetString (BOARD_UNIT, "");
		PlayerPrefs.SetString (BOARD_TILE, "");
	}

	public static void putBoard(HexCell[] cells){
		TileInfo[] tiles = new TileInfo[cells.Length];
		UnitInfo[] units = new UnitInfo[cells.Length];

		for (int i = 0; i < cells.Length; i++) {
			tiles [i] = cells [i].GetTile ();
			units [i] = cells [i].GetInfo ();
		}

		string tiles_json = JsonHelper.ToJson(tiles);
		string units_json = JsonHelper.ToJson(units);

		PlayerPrefs.SetString (BOARD_TILE, tiles_json);
		PlayerPrefs.SetString (BOARD_UNIT, units_json);
	}

	public static TileInfo[] getTiles(){
		string json = PlayerPrefs.GetString (BOARD_TILE);
		if (json.Length == 0) {
			return null;
		}
		TileInfo[] tiles = JsonHelper.FromJson<TileInfo>(json);
		return tiles;
	}

	public static UnitInfo[] getUnits(){
		string json = PlayerPrefs.GetString (BOARD_UNIT);
		if (json.Length == 0) {
			return null;
		}
		UnitInfo[] units = JsonHelper.FromJson<UnitInfo>(json);
		return units;
	}
}
