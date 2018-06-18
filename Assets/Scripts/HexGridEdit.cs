using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class HexGridEdit : HexGrid {

  int idx = 0;
  static int ELENGTH = Enum.GetNames(typeof(TileInfo.tileType)).Length;

  public void moveIdx(int mv){
    idx += mv;
    if (idx >= ELENGTH) {
      idx = 0;
    } else if (idx < 0) {
      idx = ELENGTH - 1;
    }
    setType(idx);
  }

  private void setType(int idx){
    GameObject.Find ("HeaderTxt").GetComponent<Text> ().text = TileInfo.getName((TileInfo.tileType) idx);
    GameObject.Find ("TileImg").GetComponent<Image> ().sprite = cellPrefab.GetComponent<TileSprite> ().getTile((TileInfo.tileType) idx);
  }

	protected override void Awake () {
		setType(0);

		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

    cells = new HexCell[boardHeight * boardWidth];

    for (int z = 0, i = 0; z < boardHeight; z++) {
      for (int x = 0; x < boardWidth; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

  public void SaveMap(){
    Debug.Log("Save Map Clicked");
//    Debug.Log(GameObject.Find ("MapNameField").GetComponent<InputField> ().text);
//
    List<TileInfo> mTiles = new List<TileInfo> ();

    MapInfo map = new MapInfo ();
    map.h = boardHeight;
    map.w = boardWidth;
    map.name = GameObject.Find ("MapNameField").GetComponent<InputField> ().text;

    foreach(HexCell cell in cells){
      mTiles.Add (cell.GetTile());
    }
      
    map.tiles = mTiles.ToArray ();

    BaseSaver.putMap (map);

    Debug.Log ("Saved");

    Debug.Log ("Current Maps");

    foreach(MapInfo mp in BaseSaver.getMaps ()){
      Debug.Log (mp.name);
    }
  }

  public void ResetMaps(){
    BaseSaver.resetMaps ();
  }

//  int turn = 0;
//  bool toggle = false;
//
//  string getRecruitStr(GameInfo game){
//    string army = "";
//    foreach(UnitInfo unit in game.roster){
//      switch (unit.type) {
//      case UnitInfo.unitType.Knight:
//        army+="K";
//        break;
//      case UnitInfo.unitType.Lancer:
//        army+="L";
//        break;
//      case UnitInfo.unitType.Swordsman:
//        army+="S";
//        break;
//      }
//    }
//    return army;
//  }
//
//  string getMoveStr(GameInfo game){
//    if (game.movement + game.fatigue == 0) {
//      return "";
//    } else {
//        int mLeft = game.movement - game.fatigue;
//        return game.movement.ToString() + "(" + mLeft.ToString() + ")";
//    }
//  }
//
//  void updateArmyInfo(GameInfo game){
//      GameObject.Find ("HeaderTxt").GetComponent<Text> ().text = game.name;
//      GameObject.Find ("armyInfo").GetComponent<Text> ().text = getRecruitStr(game);
//      GameObject.Find ("moveInfo").GetComponent<Text> ().text = getMoveStr(game);
//  }
//
//  void LateUpdate()
//  {
//  	if (turn == 0) {
//  		updateArmyInfo(game);
////    	foreach(HexCell cell in cells) {
////  			if (cell.GetInfo().human){
////            //Toggle this to change how the camera follows the player in adventure mode
////  					moveCamera (cell.gameObject.transform.position + new Vector3(0,45,-45));
////  			}
////      }
//  	} else {
//  		GameInfo nGame = new GameInfo ();
//  		nGame.name = "Sir Kingsly";
//  		nGame.movement = 0;
//  		nGame.fatigue = 0;
//  		nGame.roster = new UnitInfo[0];
//  		updateArmyInfo(nGame);
//  	}
//  }

	void Start () {
	    Debug.Log("Start board");

			foreach (HexCell cell in cells) {
				cell.setType(TileInfo.tileType.Grass);
			}

			hexMesh.Triangulate(cells);
      ResetCells ();
	}

//  protected override void movedCell(HexCell cell) {
//    if (cell.GetInfo().human) {
//      game.fatigue++;
//    }
//    BaseSaver.putGame (game);
//  }
//
//	protected override void postEndCheck(int turn) {
//		this.turn = turn;
//		if (turn == 0) {
//			game.fatigue = 0;
//			BaseSaver.putGame (game);
//		} else {
//			PlayAI ();
//		}
//	}

  public void PaintTile (Vector3 position, Color color) {
    position = transform.InverseTransformPoint(position);
    HexCoordinates coordinates = HexCoordinates.FromPosition(position);
    int index = coordinates.X + coordinates.Z * boardWidth + coordinates.Z / 2;

    HexCell cell = cells [index];
    cell.setType ((TileInfo.tileType) idx);

    hexMesh.Triangulate(cells);
    ResetCells ();
  }

	protected override void checkEnd(){
//		bool playersLeft = checkCells (true);
//		bool enemyLeft = checkCells (false);
//
//		Debug.Log ("checkEnd: " + playersLeft.ToString() + " / " + enemyLeft.ToString());
//
//		if (!playersLeft || !enemyLeft) {
//			if (!playersLeft) {
//				Debug.Log ("Enemy Wins!");
//			} else {
//				Debug.Log ("Player Wins!");
//			}
//			SceneManager.LoadScene ("MainMenuScene");
//		}
	}

	protected override void CheckInteraction() {
//		foreach (HexCell cell in cells) {
//			if (cell.GetInfo().human && cell.GetTile().interaction) {
//				cell.GetTile ().interaction = false;
//
//				// Save Board Here
//				BaseSaver.putBoard(cells);
//				SceneManager.LoadScene ("ChoiceScene");
//			}
//		}
	}
}