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
    if (thisBattle != null) {
      boardHeight = thisBattle.height > 0 ? thisBattle.height : 10;
      boardWidth = thisBattle.width > 0 ? thisBattle.width : 10;
    } else {
      boardHeight = 6;
      boardWidth = 8;
    }

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
      focusOnCell(cells[boardWidth + 1]);
    } else {
      placeAround(0, game.playerRoster, 0, true);
      focusOnCell(cells[0]);
      placePlayer(cells[cells.Length - 1], 1, false, UnitInfo.unitType.Monster, false);
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

  private IEnumerator MoveCamera(Vector3 pos)
  {
//    Maincamera.transform.position = Vector3.Lerp (transform.position, Targetposition.transform.position, speed * Time.deltaTime);

    float t = 0;
//    Vector3 startPos = GameObject.Find("BattleSceneCamera").transform.position;
    //    float factor = 1f;
    float speed = 1f;

    while (t < 1f)
    {
      t += Time.deltaTime * speed;
      GameObject.Find("BattleSceneCamera").transform.position = Vector3.Lerp (GameObject.Find("BattleSceneCamera").transform.position, pos, speed * Time.deltaTime);
      yield return null;
    }

    yield return 0;
  }

  private void focusOnCell(HexCell cell){
//    bool perspective = GameObject.Find ("BattleSceneCamera").GetComponent<CameraPerspective> ().toggle;
//    Vector3 angle = new Vector3(0,55,-70);
//    if (perspective) {
//      angle = new Vector3(0,70,-40);
//    }
//    Vector3 newPos = cell.gameObject.transform.position + angle;
    Vector3 newPos = GameObject.Find("BattleSceneCamera").transform.position;
    newPos.x = cell.gameObject.transform.position.x;
    StartCoroutine(MoveCamera(newPos));
//    newPos.y -= 5;
//    newPos.z -= 20;
//    GameObject.Find("BattleSceneCamera").transform.position = newPos;
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
//    Debug.Log ("Clicked");
    cell.updateUIInfo ();
    focusOnCell (cell);
//    Vector3 pos = camera.transform.position;
//    pos.x = cell.gameObject.transform.position.x;
//    Debug.Log("Before");
//    Debug.Log(GameObject.Find("BattleSceneCamera").transform.position.ToString());
//    Vector3 newPos = cell.gameObject.transform.position + new Vector3(0,45,-45);
//    newPos.y -= 10;
//    newPos.z -= 10;
//    GameObject.Find("BattleSceneCamera").transform.position = newPos;
//    Debug.Log("After");
//    Debug.Log(GameObject.Find("BattleSceneCamera").transform.position.ToString());
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
      focusOnCell (cell);
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