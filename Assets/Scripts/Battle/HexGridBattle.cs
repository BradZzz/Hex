using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class HexGridBattle : HexGrid {

	void Start () {
		Queue<HexCell> availableCells = new Queue<HexCell> ();
		availableCells.Enqueue (cells[0]);
    for (int i = 0; i < game.playerRoster.Length; i++) {
			HexCell cell = availableCells.Dequeue ();
			foreach(HexDirection dir in cell.dirs){
				HexCell n = cell.GetNeighbor (dir);
				if (n) {
					availableCells.Enqueue (n);
				}
			}
      placePlayer(cell, 0, false, game.playerRoster[i].type, true);
		}

		placePlayer(cells[cells.Length - 1], 1, false, UnitInfo.unitType.Swordsman, false);

    foreach (HexCell cell in cells) {
      cell.setType(TileInfo.tileType.Road);
    }

		hexMesh.Triangulate(cells);

		setPTurn (players - 1);
		EndTurn ();

		ResetCells ();
    ResetBoard ();
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
				
			BattleInfo battle = new BattleInfo ();
			battle.won = playersLeft;
			BaseSaver.putBattle (battle);

			SceneManager.LoadScene ("ChoiceScene");
		}
	}
}