using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class HexGrid : MonoBehaviour {

	public GameObject camera;

	public int boardWidth = 6;
	public int boardHeight = 6;

	public Color defaultColor = Color.white;

	public Color[] playerColors;
	public int players = 2;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	protected HexCell[] cells;
	protected Canvas gridCanvas;
	protected HexMesh hexMesh;
	protected int pTurn;
	protected bool locked;

	public GameInfo game;

	protected virtual void Awake () {
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

	public void EndTurn(){
    if (!locked){
  		setPTurn (getPTurn() + 1);
  		if (getPTurn() == players) {
  			setPTurn (0);
  		}
      GameObject turnI = GameObject.Find ("TurnImg");
      if (turnI) {
        turnI.GetComponent<Image>().color = playerColors [getPTurn()];
      }
  		foreach (HexCell cell in cells) {
  			if (cell.GetPlayer () == getPTurn()) {
  				cell.EndTurn ();
  			} else {
  				cell.StripTurn();
  			}
  		}
  		postEndCheck(getPTurn());
    }
	}

	public void placePlayer(HexCell cell, int idx, bool active, UnitInfo.unitType type, bool human){
		//cell.setColor(playerColors[idx]);
		UnitInfo info = new UnitInfo ();
		info.playerNo = idx;
		info.type = type;
		info.human = human;
		cell.SetInfoStart(info);
		cell.SetInfo (info);
		cell.SetActive (active);
		if (active) {
			cell.paintNeigbors ();
		}
	}

	protected void setPTurn(int pTurn) {
		this.pTurn = pTurn;
	}

	protected int getPTurn() {
		return pTurn;
	}

	protected virtual void checkEnd(){
		Debug.Log("Regular CheckEnd");
	}

	protected virtual void postEndCheck(int turn){
	    Debug.Log("Regular postEndCheck");
	}

	protected bool checkCells(bool human) {
		foreach (HexCell cell in cells) {
			if (human && cell.GetInfo ().human && cell.GetPlayer() > -1) {
				return true;
			} else if (!human && !cell.GetInfo ().human && cell.GetPlayer() > -1) {
				return true;
			}
		}
		return false;
	}

	private IEnumerator WaitAndContinue(HexAI ai, float waitTime)
	{
		HexCell player = ai.GetNextPlayer (cells);
		if (player) {
      locked = true;
			bool moved = false;
			bool attacked = false;
      if (!attacked && (player.GetInfo ().type != UnitInfo.unitType.Knight && player.GetInfo ().actions > 0) || 
        (player.GetInfo ().type == UnitInfo.unitType.Knight && player.GetInfo ().actions > 1)) {
        HexCell[] path = HexAI.aStar (cells, player);
        if (path.Length > 0) {
          for (int i = path.Length - 1; i >= 0; i--) {
            if (path [i].GetPlayer () == -1 && player.GetNeighborDir (path [i]) != HexDirection.None) {
              moveCell (player, path [i]);
              moved = true;
              break;
            }
          }
        }
      }
      if (!moved && player.getActiveEnemyAttack () != HexDirection.None && player.GetInfo ().attacks > 0) {
        HexDirection enemyDir = player.getActiveEnemyAttack ();
        attackCell (player, player.GetNeighbor (enemyDir));
        if (player.GetInfo().type == UnitInfo.unitType.Lancer && player.GetNeighbor (enemyDir).GetNeighbor (enemyDir)) {
          attackCell (player, player.GetNeighbor (enemyDir).GetNeighbor (enemyDir));
        }
        attacked = true;
        player.GetInfo().attacks--;
      }
			if (!moved && !attacked) {
				if (player.GetInfo ().type != UnitInfo.unitType.Swordsman && player.GetInfo ().actions > 0
				    && player.getActiveEnemy () != HexDirection.None) {
					foreach (HexDirection dir in player.dirs) {
						HexCell neigh = player.GetNeighbor (dir);
						if (neigh) {
							if (neigh.GetPlayer () == -1 && neigh.getActiveEnemy (player.GetPlayer ()) == HexDirection.None) {
								moveCell (player, neigh);
								neigh.StripTurn ();
								break;
							}
						}
					}
				}
				player.StripTurn ();
			}
			hexMesh.Triangulate (cells);
			yield return new WaitForSeconds(waitTime);
			PlayAI ();
		} else {
      locked = false;
			EndTurn ();
		}
	}

	public void PlayAI(){
		ResetCells ();
		StartCoroutine(WaitAndContinue(new HexAI (getPTurn()), 0.25f));
	}

	protected void moveCell(HexCell cell, HexCell adjCell){
		cell.SetActive(false);
		adjCell.SetActive(false);

		UnitInfo move_from_info = cell.GetInfo();
		UnitInfo move_to_info = adjCell.GetInfo();

		move_from_info.actions --;

		int player = cell.GetPlayer ();

		cell.SetInfo (move_to_info);
		adjCell.SetInfo (move_from_info);
		//adjCell.setColor(playerColors [player]);
		adjCell.removeFog ();

		movedCell(adjCell);

		ResetCells ();
	}

	protected void moveCell(HexCell cell, HexDirection dir){
    if (cell.GetNeighbor (dir).GetInfo().actions > 0 && cell.GetNeighbor (dir).canMoveThere(cell.GetTile()) && canMove(cell)) {
			int player = cell.GetNeighbor (dir).GetPlayer ();
			UnitInfo parent_info = cell.GetNeighbor (dir).GetInfo ();
			UnitInfo this_info = cell.GetInfo ();

			parent_info.actions -= 1;
			cell.SetInfo (parent_info);
			//cell.setColor(playerColors [player]);
			cell.GetNeighbor (dir).SetActive(false);
			cell.GetNeighbor (dir).SetInfo(this_info);
			cell.removeFog ();

      movedCell(cell);

			ResetCells ();
		}
	}

  protected virtual void attackCell(HexCell attacker, HexCell defender){
		UnitInfo attacker_info = attacker.GetInfo ();
    if (attacker.GetPlayer() > -1 && defender.GetPlayer() > -1 && attacker_info.playerNo == getPTurn() && attacker_info.attacks > 0) {
//			attacker_info.attacks--;
			// Swordsman attack strikes around hero
			if (attacker_info.type == UnitInfo.unitType.Swordsman) {
				attacker.swordAttackAround (getPTurn());
			} else {
				defender.TakeHit ();
				// Lance attack strikes through enemy
//				if (attacker_info.type == UnitInfo.unitType.Lancer) {
//					HexDirection dir = attacker.GetNeighborDir (defender);
//					HexCell farUnit = defender.GetNeighbor (dir);
//					if (farUnit && farUnit.GetPlayer() > -1 && farUnit.GetPlayer() != getPTurn()) {
//						farUnit.TakeHit ();
//					}
//				}
			}

			ResetCells ();
      Attacked(attacker);
		}
	}

  protected virtual void Attacked(HexCell cell){
    Debug.Log("Attacked");
  }

  protected virtual void Deactivated(HexCell cell){
    Debug.Log("Deactivated");
  }

  protected virtual bool canMove(HexCell cell){
    Debug.Log("canMove");
    return true;
  }

  protected virtual void movedCell(HexCell cell) {
    Debug.Log("Regular movedCell");
  }

	public void ColorCell (Vector3 position, Color color) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
    int index = coordinates.X + coordinates.Z * boardWidth + coordinates.Z / 2;

		HexCell cell = cells [index];
		if (cell.GetPlayer () > -1) {
      Clicked(cell);
			if (cell.GetPlayer () == getPTurn() && (cell.GetInfo().actions > 0 || cell.GetInfo().attacks > 0)) {
				if (cell.GetActive ()) {
					ResetCells ();
          Deactivated (cell);
				} else {
					ResetCells ();
					cell.paintNeigbors ();
					cell.SetActive (true);
				}
      } else if (cell.GetPlayer () != getPTurn()) {
				HexDirection dir = cell.getActiveNeigbor ();
        if (dir != HexDirection.None) {
          HexCell attacker = cell.GetNeighbor (dir);
          attackCell (attacker, cell);
          if (attacker.GetInfo().type == UnitInfo.unitType.Lancer && cell.GetNeighbor(HexUtilities.oppositeSide(dir))) {
            attackCell (attacker, cell.GetNeighbor(HexUtilities.oppositeSide(dir)));
          }
          attacker.GetInfo().attacks--;
        } else {
          dir = cell.getActiveLancer ();
          if (dir != HexDirection.None && cell.GetNeighbor (dir).GetNeighbor (dir).GetInfo().type == UnitInfo.unitType.Lancer) {
            HexCell attacker = cell.GetNeighbor (dir).GetNeighbor (dir);
            attackCell (attacker, cell);
            attackCell (attacker, cell.GetNeighbor (dir));
            attacker.GetInfo().attacks--;
          }
        }
			}
		} else {
			HexDirection dir = cell.getActiveNeigbor ();
      if (dir != HexDirection.None) {
				moveCell(cell, dir);
			}
		}
		hexMesh.Triangulate(cells);
	}

  protected virtual void Clicked(HexCell cell){
    Debug.Log("Regular Clicked");
  }

	protected virtual void moveCamera(Vector3 pos){
		camera.transform.position = pos;
	}

	protected virtual void CheckInteraction() {
		Debug.Log("Regular CheckInteraction");
	}

  public virtual void showPlayerMenu(){
    Debug.Log("Regular showPlayerMenu");
  }

	protected virtual void ResetCells() {
		foreach (HexCell cell in cells) {
      cell.setColor(Color.white);
			cell.SetActive (false);
      cell.updateTile ();
//      if (cell.GetTile().interaction) {
//        cell.setColor(new Color (.5f, .15f, .6f, .9f));
//        Debug.Log ("Interaction cell!");
//      }
		}
		CheckInteraction ();
		checkEnd ();
	}

	protected virtual void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.setColor(defaultColor);

    if (i == 0) {
      Debug.Log ("First Coordinate HexGrid: " + cell.coordinates.ToString());
    }

		if (x > 0) {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0) {
			if ((z & 1) == 0) {
        cell.SetNeighbor(HexDirection.SE, cells[i - boardWidth]);
				if (x > 0) {
          cell.SetNeighbor(HexDirection.SW, cells[i - boardWidth - 1]);
				}
			}
			else {
        cell.SetNeighbor(HexDirection.SW, cells[i - boardWidth]);
        if (x < boardWidth - 1) {
          cell.SetNeighbor(HexDirection.SE, cells[i - boardWidth + 1]);
				}
			}
		}

//		Text label = Instantiate<Text>(cellLabelPrefab);
//		label.rectTransform.SetParent(gridCanvas.transform, false);
//		label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
//		label.text = cell.coordinates.ToStringOnSeparateLines();
//
		cell.init (playerColors);
	}
}