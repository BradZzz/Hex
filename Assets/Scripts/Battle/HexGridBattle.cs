using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class HexGridBattle : HexGrid {

	void Start () {
    BattleInfo thisBattle = BaseSaver.getBattle ();

    if (thisBattle != null) {
//      placeAround(0, thisBattle.playerRoster, 0, true);

      UnitInfo[] roster = new UnitInfo[3];
      for (int i = 0; i < 3; i++) {
        UnitInfo info = new UnitInfo ();
        info.playerNo = 0;
        info.type = UnitInfo.unitType.Lancer;
        info.human = true;
        roster[i] = info;
      }
      placeAround(0, roster, 0, true);
      placeAround(cells.Length - 1, thisBattle.enemyRoster, 1, false);
    } else {
      placeAround(0, game.playerRoster, 0, true);
      placePlayer(cells[cells.Length - 1], 1, false, UnitInfo.unitType.Lancer, false);
    }

    foreach (HexCell cell in cells) {
      cell.setType(TileInfo.tileType.Road);
    }

		hexMesh.Triangulate(cells);

		setPTurn (players - 1);
		EndTurn ();

		ResetCells ();
    ResetBoard ();
	}

  private void placeAround(int idx, UnitInfo[] roster, int player, bool human){
    Queue<HexCell> availableCells = new Queue<HexCell> ();
    availableCells.Enqueue (cells[idx]);
    for (int i = 0; i < roster.Length; i++) {
      HexCell cell = availableCells.Dequeue ();
      foreach(HexDirection dir in cell.dirs){
        HexCell n = cell.GetNeighbor (dir);
        if (n) {
          availableCells.Enqueue (n);
        }
      }
      placePlayer(cell, player, false, roster[i].type, human);
    }
  }

  private void ResetBoard(){
    GameObject.Find ("InfoPanel").GetComponent<InfoPanel> ().togglePanel(false);
  }

  protected override void Attacked(HexCell cell){
    cell.updateUIInfo ();
  }

  protected override void Clicked(HexCell cell){
    cell.updateUIInfo ();
  }

  protected override void Deactivated(HexCell cell){
    ResetBoard ();
  }

  protected override void postEndCheck(int turn) {
    if (turn != 0) {
      PlayAI ();
      ResetBoard ();
    }
  }

  protected override void movedCell(HexCell cell) {
    if (cell.GetPlayer() == 0) {
      cell.updateUIInfo ();
    }
  }

	protected override void checkEnd(){
		bool playersLeft = checkCells (true);
		bool enemyLeft = checkCells (false);

		if (!playersLeft || !enemyLeft) {
			if (!playersLeft) {
				Debug.Log ("Enemy Wins!");
			} else {
				Debug.Log ("Player Wins!");
			}

      BattleInfo battle = BaseSaver.getBattle ();
      if (battle == null) {
        battle = new BattleInfo ();
        battle.redirect = "AdventureScene";
      }
			battle.won = playersLeft;
			BaseSaver.putBattle (battle);

      SceneManager.LoadScene (battle.redirect);
		}
	}
}