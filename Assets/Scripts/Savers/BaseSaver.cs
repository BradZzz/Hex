using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSaver {

  private static string MAPS = "maps";

  private static string GAME = "game";

  private static string BATTLE_INFO = "battle_info";

  private static string LOCATION = "location";

  private static string CHOICE = "choice";
  private static string CHOICE_QUEST = "choice_quest";
  private static string CHOICE_CHARACTER = "choice_character";
  private static string CHOICE_CHARACTER_IDX = "choice_character_idx";
  private static string CHOICE_PICKED = "choice_pick";

  private static string BOARD_INFO = "board_info";
  private static string BOARD_UNIT = "board_unit";
  private static string BOARD_TILE = "board_tile";

  public static void resetAll() {
    resetGame ();
    resetBattle ();
    resetChoice ();
    resetBoard ();
  }

  /*
   * Maps
   */

  public static void resetMaps() {
    PlayerPrefs.SetString (MAPS, "");

    Debug.Log ("Maps reset");
  }

  /*
  * Here we need to put the map into an array and save
  */
  public static void putMap(MapInfo info) {
    string json = PlayerPrefs.GetString (MAPS);
    MapInfo[] maps;
    if (json.Length == 0) {
      maps = new MapInfo[1];
      maps[0] = info;
    } else {
      bool found = false;
      List<MapInfo> lMaps = new List<MapInfo>(JsonHelper.FromJson<MapInfo>(json));
      for (int i = 0; i < lMaps.Count; i++) {
        if (lMaps[i].name.Equals(info.name)) {
          found = true;
          lMaps[i] = info;
        }
      }
      if (!found) {
        lMaps.Add(info);
      }
      maps = lMaps.ToArray();
    }
    json = JsonHelper.ToJson(maps);
    PlayerPrefs.SetString (MAPS, json);

    Debug.Log ("Map set: " + json);
  }

  /*
  * here we need to take the saved array and return it
  */
  public static MapInfo[] getMaps(){
    string json = PlayerPrefs.GetString (MAPS);
    if (json.Length == 0) {
      return null;
    }
    Debug.Log ("Maps got");
    return JsonHelper.FromJson<MapInfo>(json);
  }

  public static MapInfo getMap(string name) {
    foreach(MapInfo map in getMaps()) {
      if (map.name.Equals(name)) {
        return map;
      }
    }
    return null;
  }

  /*
   * Player
   * 
   */

  public static void resetGame() {
    PlayerPrefs.SetString (GAME, "");

    Debug.Log ("Game reset");
  }

  public static void putGame(GameInfo info) {
    string json = JsonUtility.ToJson (info);
    PlayerPrefs.SetString (GAME, json);

    Debug.Log ("Game set: " + json);
  }


  public static GameInfo getGame(){
    string json = PlayerPrefs.GetString (GAME);
    if (json.Length == 0) {
      return null;
    }
    Debug.Log ("Game got");
    return JsonUtility.FromJson<GameInfo> (json);
  }

  /*
   * Battle
   * 
   */

  public static void resetBattle() {
    PlayerPrefs.SetString (BATTLE_INFO, "");

    Debug.Log ("Battle reset");
  }

  public static void putBattle(BattleInfo info) {
    string json = JsonUtility.ToJson (info);
    PlayerPrefs.SetString (BATTLE_INFO, json);

    Debug.Log ("Battle set: " + json);
  }


  public static BattleInfo getBattle(){
    string json = PlayerPrefs.GetString (BATTLE_INFO);
    if (json.Length == 0) {
      return null;
    }
    Debug.Log ("Battle got");
    return JsonUtility.FromJson<BattleInfo> (json);
  }

  /*
   * Location
   * 
   */

  public static void resetLocation() {
    PlayerPrefs.SetString (LOCATION, "");

    Debug.Log ("Location reset");
  }

  public static void putLocation(string info) {
    PlayerPrefs.SetString (LOCATION, info);

    Debug.Log ("Location set: " + info);
  }


  public static string getLocation(){
    string str = PlayerPrefs.GetString (LOCATION);
    if (str.Length == 0) {
      return null;
    }
    Debug.Log ("Location got");
    return str;
  }

  /*
   * Choices
   *
   */

  public static void resetChoice() {
    PlayerPrefs.SetString (CHOICE, "");
    PlayerPrefs.SetString (CHOICE_QUEST, "");
    PlayerPrefs.SetString (CHOICE_CHARACTER, "");
    PlayerPrefs.SetString (CHOICE_CHARACTER_IDX, "");
    PlayerPrefs.SetString (CHOICE_PICKED, "");

    Debug.Log ("Choices reset");
  }

  public static void putChoiceCharacter(TileInfo.tileType type){
    string ty = JsonUtility.ToJson (type);
    PlayerPrefs.SetString (CHOICE_CHARACTER, ty);
  }

  public static TileInfo.tileType getChoiceCharacter(){
    string json = PlayerPrefs.GetString (CHOICE_CHARACTER);
    if (json.Length == 0) {
      return TileInfo.tileType.None;
    }
    Debug.Log ("Choices got");
    return JsonUtility.FromJson<TileInfo.tileType> (json);
  }

  public static void putChoiceQuest(ChoiceInfo choice){
    string ty = JsonUtility.ToJson (choice);
    PlayerPrefs.SetString (CHOICE_QUEST, ty);
  }

  public static ChoiceInfo getChoiceQuest(){
    string json = PlayerPrefs.GetString (CHOICE_QUEST);
    if (json.Length == 0) {
      return null;
    }
    Debug.Log ("Choices got");
    return JsonUtility.FromJson<ChoiceInfo> (json);
  }

  public static void putChoice(ChoiceInfo choice, int charIdx, int picked) {
    string json = JsonUtility.ToJson (choice);
    PlayerPrefs.SetString (CHOICE, json);
    PlayerPrefs.SetString (CHOICE_CHARACTER_IDX, charIdx.ToString());
    PlayerPrefs.SetString (CHOICE_PICKED, picked.ToString());

    Debug.Log ("Choices set: " + json);
    Debug.Log ("Choices set: " + charIdx.ToString());
    Debug.Log ("Choices set: " + picked.ToString());
  }


  public static ChoiceInfo getChoice(){
    string json = PlayerPrefs.GetString (CHOICE);
    if (json.Length == 0) {
      return null;
    }
    Debug.Log ("Choices got");
    return JsonUtility.FromJson<ChoiceInfo> (json);
  }

  public static int getCharIdx(){
    string json = PlayerPrefs.GetString (CHOICE_CHARACTER_IDX);
    if (json.Length == 0) {
      return -1;
    }
    return int.Parse (json);
  }

  public static int getPicked(){
    string json = PlayerPrefs.GetString (CHOICE_PICKED);
    if (json.Length == 0) {
      return -1;
    }
    return int.Parse (json);
  }

  /*
   * Board
   * 
   */

  public static void resetBoard(){
    PlayerPrefs.SetString (BOARD_UNIT, "");
    PlayerPrefs.SetString (BOARD_TILE, "");
  }

  public static void putBoard(HexCell[] cells, string name, int height, int width){
    TileInfo[] tiles = new TileInfo[cells.Length];
    UnitInfo[] units = new UnitInfo[cells.Length];

    for (int i = 0; i < cells.Length; i++) {
      tiles [i] = cells [i].GetTile ();
      units [i] = cells [i].GetInfo ();
    }

    BoardInfo boardInfo = new BoardInfo ();
    boardInfo.name = name;
    boardInfo.height = height;
    boardInfo.width = width;

    string info_json = JsonUtility.ToJson(boardInfo);
    string tiles_json = JsonHelper.ToJson(tiles);
    string units_json = JsonHelper.ToJson(units);

    PlayerPrefs.SetString (BOARD_INFO,info_json);
    PlayerPrefs.SetString (BOARD_TILE, tiles_json);
    PlayerPrefs.SetString (BOARD_UNIT, units_json);
  }

  public static BoardInfo getBoardInfo(){
    string json = PlayerPrefs.GetString (BOARD_INFO);
    if (json.Length == 0) {
      return null;
    }
    return JsonUtility.FromJson<BoardInfo>(json);
  }

  public static void setTiles(TileInfo[] tiles){
    string tiles_json = JsonHelper.ToJson(tiles);
    PlayerPrefs.SetString (BOARD_TILE, tiles_json);
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