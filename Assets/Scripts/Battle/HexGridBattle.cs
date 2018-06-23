using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class HexGridBattle : HexGrid {

  protected override void Awake () {
    game = BaseSaver.getGame ();

    GameObject.Find ("HeaderTxt").GetComponent<Text> ().text = game.name;

    gridCanvas = GetComponentInChildren<Canvas>();
    hexMesh = GetComponentInChildren<HexMesh>();

    BattleInfo thisBattle = BaseSaver.getBattle ();
    boardHeight = thisBattle.height > 0 ? thisBattle.height : 10;
    boardWidth = thisBattle.width > 0 ? thisBattle.width : 10;

    cells = new HexCell[boardHeight * boardWidth];

    for (int z = 0, i = 0; z < boardHeight; z++) {
      for (int x = 0; x < boardWidth; x++) {
        CreateCell(x, z, i++);
      }
    }
  }

	void Start () {
    BattleInfo thisBattle = BaseSaver.getBattle ();

    if (thisBattle != null) {
      placeAround(boardWidth + 1, thisBattle.playerRoster, 0, true);

//      UnitInfo[] roster = new UnitInfo[3];
//      for (int i = 0; i < 3; i++) {
//        UnitInfo info = new UnitInfo ();
//        info.playerNo = 0;
//        info.type = UnitInfo.unitType.Lancer;
//        info.human = true;
//        roster[i] = info;
//      }
//      placeAround(0, roster, 0, true);
      placeAround(cells.Length - 2 - boardWidth, thisBattle.enemyRoster, 1, false);
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
    Debug.Log ("Roster Size: " + roster.Length);

    Queue<HexCell> availableCells = new Queue<HexCell> ();
    availableCells.Enqueue (cells[idx]);
    int total = roster.Length -1;
    while(total >= 0){
      HexCell cell = availableCells.Dequeue ();
      HexDirection[] dirs = cell.dirs;
      HexUtilities.ShuffleArray (dirs);
      foreach(HexDirection dir in dirs){
        HexCell n = cell.GetNeighbor (dir);
        if (n) {
          availableCells.Enqueue (n);
        }
      }
      if (cell.GetInfo().playerNo == -1) {
        placePlayer(cell, player, false, roster[total].type, human);
        total--;
      }
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
      //This means that the player was attacked by the monster
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