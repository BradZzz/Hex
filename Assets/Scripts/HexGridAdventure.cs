using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class HexGridAdventure : HexGrid {

	void Start () {
		TileInfo[] bTiles = BaseSaver.getTiles ();
		UnitInfo[] bUnits = BaseSaver.getUnits ();

		if (bTiles != null && bUnits != null) {
			for (int i = 0; i < cells.Length; i++) {
				cells [i].SetInfo (bUnits[i]);
				cells [i].SetTile (bTiles[i]);

				if (bUnits[i].human){
					Debug.Log("Player tile");
					Debug.Log(bUnits[i].ToString());
				}
				if (bTiles[i].interaction) {
					cells [i].setLabel ("I");
				}
			}
			BaseSaver.resetBoard ();
		} else {
			foreach(HexCell cell in cells) {
				cell.GetTile ().fog = true;
			}
				
			placePlayer(cells[0], 0, true, UnitInfo.unitType.Adventure, true);

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
						cell.setLabel ("I");
					}
				}
			}

			foreach (HexCell cell in cells){
				if (cell.GetPlayer() == -1) {
					int chp = Random.Range(0, 4);
					if (chp == 0) {
						cell.GetTile ().interaction = true;
						cell.setLabel ("I");
					}
				}
			}
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
			//SceneManager.LoadScene ("MainMenuScene");
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