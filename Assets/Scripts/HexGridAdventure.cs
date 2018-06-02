using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class HexGridAdventure : HexGrid {

  int turn = 0;
  bool toggle = false;

  string getRecruitStr(GameInfo game){
    string army = "";
    foreach(UnitInfo unit in game.roster){
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
  }

  void LateUpdate()
  {
  	if (turn == 0) {
  		updateArmyInfo(game);
    	foreach(HexCell cell in cells) {
  			if (cell.GetInfo().human){
            //Toggle this to change how the camera follows the player in adventure mode
  					moveCamera (cell.gameObject.transform.position + new Vector3(0,45,-45));
  			}
      }
  	} else {
  		GameInfo nGame = new GameInfo ();
  		nGame.name = "Sir Kingsly";
  		nGame.movement = 0;
  		nGame.fatigue = 0;
  		nGame.roster = new UnitInfo[0];
  		updateArmyInfo(nGame);
  	}
  }

	void Start () {
		TileInfo[] bTiles = BaseSaver.getTiles ();
		UnitInfo[] bUnits = BaseSaver.getUnits ();

		if (bTiles != null && bUnits != null) {
		  game = BaseSaver.getGame ();
			for (int i = 0; i < cells.Length; i++) {
				cells [i].SetInfo (bUnits[i]);
				cells [i].SetTile (bTiles[i]);

          if (bUnits[i].human) {
            int mv = game.movement - game.fatigue;
            cells [i].GetInfo().actions = mv < 0 ? 0 : mv;
          }

//				if (bTiles[i].interaction) {
//					cells [i].setLabel ("I");
//				}
			}

			hexMesh.Triangulate(cells);
			GameObject.Find ("TurnImg").GetComponent<Image>().color = playerColors [0];
			setPTurn (0);
			ResetCells ();

			BaseSaver.resetBoard ();

		} else {
			foreach(HexCell cell in cells) {
				cell.GetTile ().fog = true;
			}
				
			placePlayer(cells[0], 0, false, UnitInfo.unitType.Adventure, true);

			cells [0].removeFog ();

			placePlayer(cells[cells.Length - 1], 1, false, UnitInfo.unitType.Adventure, false);

			if (players > 2) {
				placePlayer(cells[cells.Length - width], 2, false, UnitInfo.unitType.Adventure, false);
			}

			if (players > 3) {
				placePlayer (cells [width - 1], 3, false, UnitInfo.unitType.Adventure, false);
			}

			foreach (HexCell cell in cells) {
				//			cell.setType((TileInfo.tileType) Random.Range(0, 7));
				cell.setType(TileInfo.tileType.Grass);
			}

			HexCell[] road = HexAI.aStar (cells, cells[0], cells[cells.Length - 1]);
			foreach (HexCell cell in road) {
				cell.setType(TileInfo.tileType.Road);
			}

			int mountain = Random.Range (3, cells.Length - 3);
			cells [mountain].setType (TileInfo.tileType.Mountain);
			foreach(HexDirection dir in cells [mountain].dirs) {
				if (cells [mountain].GetNeighbor(dir) && TileInfo.tileType.Grass == cells [mountain].GetNeighbor(dir).GetTile().type) {
					cells [mountain].GetNeighbor (dir).setType (TileInfo.tileType.Forest);
				}
			}

			foreach (HexCell cell in cells){
				if (cell.GetPlayer() == -1) {
					int chp = Random.Range(0, 4);
					if (chp == 0) {
						cell.GetTile ().interaction = true;
//						cell.setLabel ("I");
					}
				}
			}
//			foreach (HexCell cell in cells){
//				if (cell.GetPlayer() == -1) {
//					int chp = Random.Range(0, 4);
//					if (chp == 0) {
//						cell.GetTile ().interaction = true;
//						cell.setLabel ("I");
//					}
//				}
//      30,55,-60 / 20,0,0
//			}

			hexMesh.Triangulate(cells);
      setPTurn (players - 1);
      EndTurn ();
      ResetCells ();
		}
	}

  protected override void movedCell(HexCell cell) {
    if (cell.GetInfo().human) {
      game.fatigue++;
    }
    BaseSaver.putGame (game);
  }

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
		foreach (HexCell cell in cells) {
			if (cell.GetInfo().human && cell.GetTile().interaction) {
				cell.GetTile ().interaction = false;

				// Save Board Here
				BaseSaver.putBoard(cells);
				SceneManager.LoadScene ("ChoiceScene");
			}
		}
	}
}