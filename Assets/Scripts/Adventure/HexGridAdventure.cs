using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class HexGridAdventure : HexGrid {

  public static string MAP_NAME = "Level1";

  int turn = 0;
  bool toggle = false;

  string getRecruitStr(GameInfo game){
    string army = "";
    foreach(UnitInfo unit in game.playerRoster){
      switch (unit.type) {
      case UnitInfo.unitType.Knight:
        army+="K";
        break;
      case UnitInfo.unitType.Lancer:
        army+="L";
        break;
      case UnitInfo.unitType.Swordsman:
        army+="S";
        break;
      }
    }
    return army;
  }

  string getMoveStr(GameInfo game){
    if (game.movement + game.fatigue == 0) {
      return "";
    } else {
        int mLeft = game.movement - game.fatigue;
        return game.movement.ToString() + "(" + mLeft.ToString() + ")";
    }
  }

  void updateArmyInfo(GameInfo game){
    GameObject.Find ("HeaderTxt").GetComponent<Text> ().text = game.name;
    GameObject.Find ("armyInfo").GetComponent<Text> ().text = getRecruitStr(game);
    GameObject.Find ("moveInfo").GetComponent<Text> ().text = getMoveStr(game);
    GameObject.Find ("goldInfo").GetComponent<Text> ().text = game.gold > 0 ? "$" + game.gold.ToString() : "";
    GameObject.Find ("rationInfo").GetComponent<Text> ().text = game.rations > 0 ? "<>" + game.rations.ToString() : "";
  }

  void LateUpdate()
  {
  	if (turn == 0) {
  		updateArmyInfo(game);
//    	foreach(HexCell cell in cells) {
//  			if (cell.GetInfo().human){
//            //Toggle this to change how the camera follows the player in adventure mode
//  					moveCamera (cell.gameObject.transform.position + new Vector3(0,45,-45));
//  			}
//      }
  	} else {
  		GameInfo nGame = new GameInfo ();
  		nGame.name = "The Monster";
  		nGame.movement = 0;
  		nGame.fatigue = 0;
      nGame.gold = 0;
      nGame.rations = 0;
      nGame.playerRoster = new UnitInfo[0];
  		updateArmyInfo(nGame);
  	}
  }

  protected override void Awake () {
    game = BaseSaver.getGame ();

    GameObject.Find ("HeaderTxt").GetComponent<Text> ().text = game.name;

    gridCanvas = GetComponentInChildren<Canvas>();
    hexMesh = GetComponentInChildren<HexMesh>();

    cells = new HexCell[boardHeight * boardWidth];

    for (int z = 0, i = 0; z < boardHeight; z++) {
      for (int x = 0; x < boardWidth; x++) {
        CreateCell(x, z, i++);
      }
    }
  }

  void checkEndGame(){
    BattleInfo battle = BaseSaver.getBattle ();
    if (battle != null) {
      if (battle.won) {
        Debug.Log ("Won");
      } else {
        Debug.Log ("Lost");
      }
      BaseSaver.resetBattle ();
      SceneManager.LoadScene ("MainMenuScene");
    }
  }

	void Start () {
    checkEndGame ();

		TileInfo[] bTiles = BaseSaver.getTiles ();
		UnitInfo[] bUnits = BaseSaver.getUnits ();

    HexCell hPos = cells [0];

		if (bTiles != null && bUnits != null) {
		  game = BaseSaver.getGame ();
			for (int i = 0; i < cells.Length; i++) {
				cells [i].SetInfo (bUnits[i]);
				cells [i].SetTile (bTiles[i]);

          if (bUnits[i].human) {
            int mv = game.movement - game.fatigue;
            cells [i].GetInfo().actions = mv < 0 ? 0 : mv;
            hPos = cells [i];
          }
			}

			GameObject.Find ("TurnImg").GetComponent<Image>().color = playerColors [0];
			setPTurn (0);

			/*
			  Here is where the new quests need to be calculated if there are any
			*/
			foreach(QuestInfo quest in game.quests){
        if (!quest.placed) {
          quest.startIdx = hPos.coordinates;
          List<HexCell> dests = new List<HexCell> ();
          for (int i = 0; i < cells.Length; i++) {
            if (cells[i].GetTile().type == quest.locType && !cells[i].GetTile().interaction) {
              HexCell[] path = HexAI.aStar (cells,hPos,cells[i]);
              if(path != null && path.Length < 10){
                dests.Add (cells[i]);
              }
            }
          }
          if (dests.Count > 0) {
            HexCell[] theseDests = dests.ToArray ();
            HexUtilities.ShuffleArray (theseDests);
            theseDests [0].GetTile().interaction = true;
            quest.endIdx = theseDests [0].coordinates;
            quest.placed = true;
            Debug.Log ("Destination Set: " + quest.endIdx.ToString ());
            Debug.Log ("Tiles saved");
            BaseSaver.putBoard (cells, BaseSaver.getBoardInfo().name, 
              BaseSaver.getBoardInfo().height, BaseSaver.getBoardInfo().width);
            BaseSaver.putGame (game);
          } else {
            Debug.Log ("No destinations! Quest invalid...");
          }
        }
			}

			ResetCells ();
      hexMesh.Triangulate(cells);
//			BaseSaver.resetBoard ();

		} else {
      Debug.Log ("Maps: ");
      foreach(MapInfo mp in BaseSaver.getMaps()){
        Debug.Log (mp.name);
      }


//			foreach(HexCell cell in cells) {
//				cell.GetTile ().fog = true;
//			}

//			MapInfo map = BaseSaver.getMap("Basic Level");
      MapInfo map = BaseSaver.getMap(MAP_NAME);

      List<int> playerPos = new List<int> ();
      List<int> enemyPos = new List<int> ();

      for (int i = 0; i < cells.Length; i++) {
        if (map.tiles[i].type == TileInfo.tileType.pSpawn) {
          cells[i].setType(TileInfo.tileType.Road);
          playerPos.Add (i);
        } else if (map.tiles[i].type == TileInfo.tileType.eSpawn) {
          cells[i].setType(TileInfo.tileType.Road);
          enemyPos.Add (i);
        } else {
          cells[i].setType(map.tiles[i].type);
        }
			}

      int[] pArr = playerPos.ToArray ();
      int[] eArr = enemyPos.ToArray ();

      HexUtilities.ShuffleArray (pArr);
      HexUtilities.ShuffleArray (eArr);


      placePlayer(cells[pArr[0]], 0, false, UnitInfo.unitType.Adventure, true);
      cells [pArr[0]].removeFog ();

      placePlayer(cells[eArr[0]], 1, false, UnitInfo.unitType.Adventure, false);

//			if (players > 2) {
//				placePlayer(cells[cells.Length - width], 2, false, UnitInfo.unitType.Adventure, false);
//			}
//
//			if (players > 3) {
//				placePlayer (cells [width - 1], 3, false, UnitInfo.unitType.Adventure, false);
//			}

//			foreach (HexCell cell in cells) {
//				//			cell.setType((TileInfo.tileType) Random.Range(0, 7));
//				cell.setType(TileInfo.tileType.Grass);
//			}

//			HexCell[] road = HexAI.aStar (cells, cells[0], cells[cells.Length - 1]);
//			foreach (HexCell cell in road) {
//				cell.setType(TileInfo.tileType.Road);
//			}
//
//      HexAdventureGenerator.generateMap (cells, height, width);
//
//			int mountain = Random.Range (3, cells.Length - 3);
//			cells [mountain].setType (TileInfo.tileType.Mountain);
//			foreach(HexDirection dir in cells [mountain].dirs) {
//				if (cells [mountain].GetNeighbor(dir) && TileInfo.tileType.Grass == cells [mountain].GetNeighbor(dir).GetTile().type) {
//					cells [mountain].GetNeighbor (dir).setType (TileInfo.tileType.Forest);
//				}
//			}

//			foreach (HexCell cell in cells){
//				if (cell.GetPlayer() == -1) {
//					int chp = Random.Range(0, 4);
//					if (chp == 0) {
//						cell.GetTile ().interaction = true;
////						cell.setLabel ("I");
//					}
//				}
//			}

			hexMesh.Triangulate(cells);
      setPTurn (players - 1);
      EndTurn ();
      ResetCells ();
		}
	}

  private void setMessageTxt(string txt){
    Debug.Log ("Setting Message Text: " + txt);
    GameObject.Find ("MsgText").GetComponent<Text> ().text = txt;
  }

  private void resetMessageTxt(){
    GameObject.Find ("MsgText").GetComponent<Text> ().text = "";
  }

  protected override void attackCell(HexCell attacker, HexCell defender){
    GameInfo game = BaseSaver.getGame ();

    BattleInfo thisBattle = new BattleInfo ();
    thisBattle.playerRoster = game.playerRoster;
    thisBattle.enemyRoster = game.enemyRoster;
    thisBattle.redirect = "AdventureScene";
    BaseSaver.putBattle (thisBattle);

    SceneManager.LoadScene ("BattleScene");
  }

  private string receiveTreasure(){
    GameInfo player = BaseSaver.getGame ();
    int pick = Random.Range (0,20);
    if (pick == 0){
      Debug.Log ("Gained Attribute!");
      CharacterInfo.attributeType newAttrib = GameInfo.getAvailableAttribute (player);
      if (newAttrib != CharacterInfo.attributeType.None) {
        List<CharacterInfo.attributeType> attribs = new List<CharacterInfo.attributeType> (player.attributes);
        attribs.Add (newAttrib);
        player.attributes = attribs.ToArray ();
      }
      return "Gained Attribute!";
    } else if (pick < 8){
      Debug.Log ("Gained Gold!");
      player.gold += Random.Range (80,220);
      return "Gained Gold!";
    } else {
      Debug.Log ("Gained Rations!");
      player.rations += Random.Range (200,450);
      return "Gained Rations!";
    }
  }

  protected override void movedCell(HexCell cell) {
    resetMessageTxt ();

    Debug.Log ("Moved onto: " + cell.GetTile().type.ToString());

    if (cell.GetInfo().human) {
      game.fatigue++;
      game.rations--;
    }

    BaseSaver.putGame (game);

//    bool cellFound = false;
//    foreach(HexCell bCell in cells){
//      foreach(QuestInfo quest in BaseSaver.getGame().quests){
//        Debug.Log ("Checking Cell: " + bCell.coordinates.ToString() + " vs " + quest.endIdx.ToString());
//        if(quest.endIdx.Equals(bCell.coordinates)){
//          Debug.Log ("Quest Cell: " + bCell.coordinates.ToString());
//          Debug.Log ("Changing Color");
//          bCell.setColor(new Color (.15f, .55f, .6f, .9f));
//          cellFound = true;
//        }
//      }
//    }
//    if (!cellFound) {
//      Debug.Log ("No valid cells found");
//    }

    if (cell.GetInfo ().human) {

      if (cell.GetTile ().interaction) {
        Debug.Log ("Interaction Cell!");
        Debug.Log ("Cell Coords: " + cell.coordinates.ToString());

        // Check which quest was in this location. When we find the right one, remove it
        foreach (QuestInfo quest in game.quests) {
          Debug.Log ("Q Coords: " + quest.endIdx.ToString());
          if (quest.endIdx.Equals (cell.coordinates)) {
            Debug.Log ("Cells equal");
            quest.completed = true;
            cell.GetTile ().interaction = false;
          } else {
            Debug.Log ("Cells not equal");
          }
        }
      } else {
        //Decide if we need to do anything now that we stepped on the tile
        switch (cell.GetTile ().type) {
        case TileInfo.tileType.Castle:
          Debug.Log ("Enter Castle");
          BaseSaver.putLocation ("Great Hayre Junction");
          enterLocation ();
          break;
        case TileInfo.tileType.City:
          Debug.Log ("Enter City");
          BaseSaver.putLocation ("Lost Village");
          enterLocation ();
          break;
        case TileInfo.tileType.Forest:
          Debug.Log ("Enter Forest");
          if (Random.Range (0, 5) < 1) {
            Debug.Log ("Bear Attack!");
            BaseSaver.putChoiceCharacter (TileInfo.tileType.Forest);
            createInteraction ();
          }
          break;
        case TileInfo.tileType.Grass:
          Debug.Log ("Enter Grass");
          if (Random.Range (0, 9) < 1) {
            Debug.Log ("Cow Attack!");
            BaseSaver.putChoiceCharacter (TileInfo.tileType.Grass);
            createInteraction ();
          }
          break;
        case TileInfo.tileType.Road:
          Debug.Log ("Enter Road");
          if (Random.Range (0, 16) < 1) {
            Debug.Log ("Bunny Attack!");
            BaseSaver.putChoiceCharacter (TileInfo.tileType.Road);
            createInteraction ();
          }
          break;
        case TileInfo.tileType.Mountain:
          Debug.Log ("Enter Mountain");
          break;
        case TileInfo.tileType.Treasure:
          Debug.Log ("Enter Treasure");
          setMessageTxt ("Gained Treasure!");
          cell.setType (TileInfo.tileType.Grass);
          break;
        case TileInfo.tileType.Water:
          Debug.Log ("Enter Water");
          break;
        }
      }
    }
  }

  private void createInteraction(){
    BaseSaver.putBoard(cells, MAP_NAME, boardHeight, boardWidth);

    SceneManager.LoadScene ("ChoiceScene");
  }

  private void enterLocation(){
    BaseSaver.putBoard(cells, MAP_NAME, boardHeight, boardWidth);

    SceneManager.LoadScene ("LocationScene");
  }

  public override void showPlayerMenu(){
    BaseSaver.putBoard(cells, MAP_NAME, boardHeight, boardWidth);

    SceneManager.LoadScene ("QuestScene");
  }

  protected override bool canMove(HexCell cell){
    Debug.Log("canMove");
    return game.rations > 0;
  }

//  HexCell[] getQuestCells(int[] idxs){
//    HexCell[] rCells = new HexCell[idxs.Length];
//    for(int i =0; i < idxs.Length; i++){
//      rCells [i] = cells [idxs [i]];
//    }
//    return rCells;
//  }

//  protected override void movedCell(HexCell cell) {
//    if (cell.GetTile().interaction) {
//      /*
//       * Check which quest was in this location. When we find the right one, remove it
//       */
//      foreach(QuestInfo quest in game.quests){
//
//      }
//    }
//    Debug.Log("Regular movedCell");
//    game.rations--;
//    BaseSaver.putGame (game);
//  }

	protected override void postEndCheck(int turn) {
		this.turn = turn;
		if (turn == 0) {
			game.fatigue = 0;
			BaseSaver.putGame (game);
		} else {
			PlayAI ();
		}
	}

	protected override void checkEnd(){
		bool playersLeft = checkCells (true);
		bool enemyLeft = checkCells (false);

		Debug.Log ("checkEnd: " + playersLeft.ToString() + " / " + enemyLeft.ToString());

		if (!playersLeft || !enemyLeft) {
			if (!playersLeft) {
				Debug.Log ("Enemy Wins!");
			} else {
				Debug.Log ("Player Wins!");
			}
			SceneManager.LoadScene ("MainMenuScene");
		}
      
	}

	protected override void CheckInteraction() {
//    GameInfo gm = BaseSaver.getGame ();
//    foreach(QuestInfo quest in gm.quests){
//      if(quest.endIdx.Equals()){
//
//      }
//    }
//    HexCoordinates[] coors = new 
		foreach (HexCell cell in cells) {
			if (cell.GetTile().interaction) {
        cell.setColor(new Color (.5f, .15f, .6f, .9f));
			}
		}
	}
}