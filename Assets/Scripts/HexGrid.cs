using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class HexGrid : MonoBehaviour {

	/*
	 * TODO:
	 * add dumb ai
	 * add A* ai
	 */

	public int width = 6;
	public int height = 6;

	public Color defaultColor = Color.white;

	public Color[] playerColors;
	public int players = 2;
	private int pTurn;
	private bool locked;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	HexCell[] cells;

	Canvas gridCanvas;
	HexMesh hexMesh;

	void Awake () {
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

		cells = new HexCell[height * width];

		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void Start () {
		hexMesh.Triangulate(cells);

		ResetCells ();

		placePlayer(cells[0], 0, true, UnitInfo.unitType.Knight);
		placePlayer(cells[1], 0, false, UnitInfo.unitType.Knight);
		placePlayer (cells [width], 0, false, UnitInfo.unitType.Knight);

		placePlayer(cells[cells.Length - 1], 1, false, UnitInfo.unitType.Knight);
		placePlayer(cells[cells.Length - 2], 1, false, UnitInfo.unitType.Knight);
		placePlayer(cells[cells.Length - 1 - width], 1, false, UnitInfo.unitType.Knight);

		if (players > 2) {
			placePlayer(cells[cells.Length - width], 2, false, UnitInfo.unitType.Knight);
			placePlayer(cells[cells.Length - width + 1], 2, false, UnitInfo.unitType.Knight);
			placePlayer(cells[cells.Length - width * 2], 2, false, UnitInfo.unitType.Knight);
		}

		if (players > 3) {
			placePlayer (cells [width - 1], 3, false, UnitInfo.unitType.Knight);
			placePlayer (cells [width - 2], 3, false, UnitInfo.unitType.Knight);
			placePlayer (cells [width * 2 - 1], 3, false, UnitInfo.unitType.Knight);
		}

		hexMesh.Triangulate(cells);

		pTurn = players - 1;
		EndTurn ();
	}

	public void EndTurn(){
		pTurn++;
		if (pTurn == players) {
			pTurn = 0;
		}
		GameObject.Find ("TurnImg").GetComponent<Image>().color = playerColors [pTurn];
		foreach (HexCell cell in cells) {
			if (cell.GetPlayer () == pTurn) {
				cell.EndTurn ();
			} else {
				cell.StripTurn();
			}
		}
	}

	void placePlayer(HexCell cell, int idx, bool active, UnitInfo.unitType type){
		cell.color = playerColors[idx];
		UnitInfo info = new UnitInfo ();
		info.playerNo = idx;
		info.type = type;
		cell.SetInfoStart(info);
		cell.SetInfo (info);
		cell.SetActive (active);
		if (active) {
			cell.paintNeigbors ();
		}
	}

	private IEnumerator WaitAndContinue(HexAI ai, float waitTime)
	{
		HexCell player = ai.GetNextPlayer (cells);
		if (player) {
			bool moved = false;
			bool attacked = false;
			if (player.GetInfo ().actions > 0) {
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
			if (!moved && player.getActiveEnemy () != HexDirection.None && player.GetInfo ().attacks > 0) {
				HexDirection enemyDir = player.getActiveEnemy ();
				attackCell (player, player.GetNeighbor (enemyDir));
				attacked = true;
			} 
			if (!moved && !attacked) {
				if (player.GetInfo().type == UnitInfo.unitType.Knight && player.GetInfo().actions > 0 
					&& player.getActiveEnemy () != HexDirection.None) {
					foreach (HexDirection dir in player.dirs) {
						HexCell neigh = player.GetNeighbor (dir);
						if (neigh) {
							if (neigh.GetPlayer () == -1 && neigh.getActiveEnemy (player.GetPlayer()) == HexDirection.None) {
								moveCell (player, neigh);
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
			EndTurn ();
		}
	}

	public void PlayAI(){
		ResetCells ();
		StartCoroutine(WaitAndContinue(new HexAI (pTurn), 0.25f));
	}

	private void moveCell(HexCell cell, HexCell adjCell){
		cell.SetActive(false);
		adjCell.SetActive(false);

		UnitInfo move_from_info = cell.GetInfo();
		UnitInfo move_to_info = adjCell.GetInfo();

		move_from_info.actions --;

		int player = cell.GetPlayer ();

		cell.SetInfo (move_to_info);
		adjCell.SetInfo (move_from_info);
		adjCell.color = playerColors [player];

		ResetCells ();
	}

	private void moveCell(HexCell cell, HexDirection dir){
		if (cell.GetNeighbor (dir).GetInfo().actions > 0) {
			int player = cell.GetNeighbor (dir).GetPlayer ();
			UnitInfo parent_info = cell.GetNeighbor (dir).GetInfo ();
			UnitInfo this_info = cell.GetInfo ();

			parent_info.actions -= 1;
			cell.SetInfo (parent_info);
			cell.color = playerColors [player];
			cell.GetNeighbor (dir).SetActive(false);
			cell.GetNeighbor (dir).SetInfo(this_info);

			ResetCells ();
		}
	}

	private void attackCell(HexCell attacker, HexCell defender){
		UnitInfo attacker_info = attacker.GetInfo ();
		if (attacker_info.playerNo == pTurn && attacker_info.attacks > 0) {
			attacker_info.attacks--;
			// Swordsman attack strikes around hero
			if (attacker_info.type == UnitInfo.unitType.Swordsman) {
				attacker.swordAttackAround (pTurn);
			} else {
				defender.TakeHit ();
				// Lance attack strikes through enemy
				if (attacker_info.type == UnitInfo.unitType.Lancer) {
					HexDirection dir = attacker.GetNeighborDir (defender);
					HexCell farUnit = defender.GetNeighbor (dir);
					if (farUnit && farUnit.GetPlayer() > -1 && farUnit.GetPlayer() != pTurn) {
						farUnit.TakeHit ();
					}
				}
			}

			ResetCells ();
		}
	}

	public void ColorCell (Vector3 position, Color color) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;

		HexCell cell = cells [index];
		if (cell.GetPlayer () > -1) {
			if (cell.GetPlayer () == pTurn && (cell.GetInfo().actions > 0 || cell.GetInfo().attacks > 0)) {
				if (cell.GetActive ()) {
					ResetCells ();
				} else {
					ResetCells ();
					cell.paintNeigbors ();
					cell.SetActive (true);
				}
			} else if (cell.GetPlayer () != pTurn) {
				HexDirection dir = cell.getActiveNeigbor ();
				if (dir != HexDirection.None) {
					HexCell attacker = cell.GetNeighbor (dir);
					attackCell(attacker, cell);
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

	void ResetCells() {
		foreach (HexCell cell in cells) {
			if (cell.GetPlayer () == -1) {
				cell.color = Color.white;
			}
			cell.SetActive (false);
		}
	}

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.color = defaultColor;

		if (x > 0) {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0) {
			if ((z & 1) == 0) {
				cell.SetNeighbor(HexDirection.SE, cells[i - width]);
				if (x > 0) {
					cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
				}
			}
			else {
				cell.SetNeighbor(HexDirection.SW, cells[i - width]);
				if (x < width - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
				}
			}
		}

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
		cell.init (label);
	}
}