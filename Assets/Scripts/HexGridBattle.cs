using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class HexGridBattle : HexGrid {

	void Start () {
		Queue<HexCell> availableCells = new Queue<HexCell> ();
		availableCells.Enqueue (cells[0]);
		for (int i = 0; i < game.roster.Length; i++) {
			HexCell cell = availableCells.Dequeue ();
			foreach(HexDirection dir in cell.dirs){
				HexCell n = cell.GetNeighbor (dir);
				if (n) {
					availableCells.Enqueue (n);
				}
			}
			placePlayer(cell, 0, false, game.roster[i].type, true);
		}

		//		ResetCells ();

//		placePlayer(cells[0], 0, true, UnitInfo.unitType.Knight, true);
//		placePlayer(cells[1], 0, false, UnitInfo.unitType.Knight, true);
//		placePlayer (cells [width], 0, false, UnitInfo.unitType.Knight, true);
//		placePlayer (cells [2], 0, false, UnitInfo.unitType.Knight, true);



		//		placePlayer (cells [width + 1], 0, false, UnitInfo.unitType.Lancer, true);
		//		placePlayer (cells [2 * width + 1], 0, false, UnitInfo.unitType.Lancer, true);

		placePlayer(cells[cells.Length - 1], 1, false, UnitInfo.unitType.Swordsman, false);
		//		placePlayer(cells[cells.Length - 2], 1, false, UnitInfo.unitType.Swordsman, true);
		//		placePlayer(cells[cells.Length - 1 - width], 1, false, UnitInfo.unitType.Swordsman, true);
		//		placePlayer(cells[cells.Length - 3], 1, false, UnitInfo.unitType.Knight, true);
		//		placePlayer(cells[cells.Length - 2 - width], 1, false, UnitInfo.unitType.Knight, true);
		//		placePlayer(cells[cells.Length - 2 - width * 2], 1, false, UnitInfo.unitType.Knight, true);
		//
		//		if (players > 2) {
		//			placePlayer(cells[cells.Length - width], 2, false, UnitInfo.unitType.Lancer, true);
		//			placePlayer(cells[cells.Length - width + 1], 2, false, UnitInfo.unitType.Knight, true);
		//			placePlayer(cells[cells.Length - width * 2], 2, false, UnitInfo.unitType.Swordsman, true);
		//		}
		//
		//		if (players > 3) {
		//			placePlayer (cells [width - 1], 3, false, UnitInfo.unitType.Lancer, true);
		//			placePlayer (cells [width - 2], 3, false, UnitInfo.unitType.Knight, true);
		//			placePlayer (cells [width * 2 - 1], 3, false, UnitInfo.unitType.Swordsman, true);
		//		}

    foreach (HexCell cell in cells) {
      cell.setType(TileInfo.tileType.Road);
    }

		hexMesh.Triangulate(cells);

		setPTurn (players - 1);
		EndTurn ();

		ResetCells ();
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