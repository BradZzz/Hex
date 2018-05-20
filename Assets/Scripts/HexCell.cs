using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;

	public Color color;
	public UnitInfo info;

	private bool active;
	private Text label;

	public HexDirection[] dirs = { 
		HexDirection.NE,
		HexDirection.NW,
		HexDirection.SE,
		HexDirection.SW,
		HexDirection.W,
		HexDirection.E
	};

	public void init(Text label){
		info = new UnitInfo ();
		info.playerNo = -1;
		info.type = UnitInfo.unitType.None;

		active = false;
		this.label = label;
		this.label.text = "";
	}

	public UnitInfo GetInfo() { return info; }
	public void SetInfoStart(UnitInfo info) { 
		this.info = info; 
		if (this.info.type != UnitInfo.unitType.None) {
			switch (this.info.type) {
			case UnitInfo.unitType.Knight:
				this.info.health = 3;
				break;
			case UnitInfo.unitType.Lancer:
				this.info.health = 3;
				break;
			case UnitInfo.unitType.Swordsman:
				this.info.health = 3;
				break;
			}
		} else {
			label.text = "";
		}
	}
	public void SetInfo(UnitInfo info) { 
		this.info = info; 
		if (this.info.type != UnitInfo.unitType.None) {
			switch (this.info.type) {
			case UnitInfo.unitType.Knight:
				label.text = "K" + this.info.health.ToString();
				break;
			case UnitInfo.unitType.Lancer:
				label.text = "L" + this.info.health.ToString();
				break;
			case UnitInfo.unitType.Swordsman:
				label.text = "S" + this.info.health.ToString();
				break;
			}
		} else {
			label.text = "";
		}
	}
	public void EndTurn(){
		if (info.type != UnitInfo.unitType.None) {
			switch (info.type) {
			case UnitInfo.unitType.Knight:
				info.actions = 3;
				info.attacks = 1;
				break;
			case UnitInfo.unitType.Lancer:
				info.actions = 2;
				info.attacks = 2;
				break;
			case UnitInfo.unitType.Swordsman:
				info.actions = 2;
				info.attacks = 1;
				break;
			}
		}
	}
	public void StripTurn(){
		info.actions = 0;
		info.attacks = 0;
	}

	public int GetPlayer() { return info.playerNo; }

	public bool GetActive() { return active; }
	public void SetActive(bool active) { this.active = active; }

	[SerializeField]
	HexCell[] neighbors;

	public HexDirection GetNeighborDir (HexCell cell) {
		foreach(HexDirection dir in dirs) {
			if (GetNeighbor(dir) == cell) {
				return dir;
			}
		}
		return HexDirection.None;
	}

	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

	public void paintNeigbors (){
		foreach(HexDirection dir in dirs) {
			setNeigbor(dir);
		}
	}

	private void setNeigbor(HexDirection direction){
		if (GetNeighbor (direction) && GetNeighbor (direction).GetPlayer() == -1) {
			GetNeighbor (direction).color = Color.gray;
		}
	}

	public HexDirection getActiveNeigbor (){
		foreach (HexDirection dir in dirs) {
			if (getActiveNeigbor (dir) > -1) {
				return dir;
			}
		}
		return HexDirection.None;
	}

	public HexDirection getActiveEnemy (){
		foreach (HexDirection dir in dirs) {
			if (GetNeighbor (dir) && GetNeighbor (dir).GetPlayer() > -1 && GetNeighbor (dir).GetPlayer() != GetPlayer()) {
				return dir;
			}
		}
		return HexDirection.None;
	}

	public void swordAttackAround (int pTurn){
		foreach (HexDirection dir in dirs) {
			if (GetNeighbor (dir) && GetNeighbor (dir).GetPlayer() > -1 && GetNeighbor (dir).GetPlayer() != pTurn) {
				GetNeighbor (dir).TakeHit ();
			}
		}
	}

	public void TakeHit(){
		info.health--;
		if (info.health < 1) {
			info.playerNo = -1;
			info.type = UnitInfo.unitType.None;
		}
		SetInfo(info);
	}

	public HexDirection[] GetLancerDirs(int atkPlayer){
		List<HexDirection> validDirs = new List<HexDirection> ();
		foreach (HexDirection dir in dirs) {
			if (GetNeighbor(dir) && GetNeighbor(dir).GetPlayer() > -1 && GetNeighbor(dir).GetPlayer() != atkPlayer 
				&& GetNeighbor(HexUtilities.oppositeSide(dir)) && GetNeighbor(HexUtilities.oppositeSide(dir)).GetPlayer() == -1) {
				validDirs.Add (HexUtilities.oppositeSide(dir));
			}
		}
		return validDirs.ToArray ();
	}

	public HexDirection[] GetSwordDirs(int atkPlayer){
		int adjEnemy = 1;
		List<HexDirection> validDirs = new List<HexDirection> ();
		foreach (HexDirection dir in dirs) {
			if (GetNeighbor(dir) && GetNeighbor(dir).GetPlayer() == -1) {
				//Here's an empty space around the enemy. see if there are more than 1 enemy adjoining
				int thisEnemy = 0;
				foreach (HexDirection dir2 in dirs) {
					HexCell surrCell = GetNeighbor (dir).GetNeighbor (dir2);
					if (surrCell && surrCell.GetPlayer() > -1 && surrCell.GetPlayer() != atkPlayer) {
						thisEnemy++;
					}
				}
				if (thisEnemy > adjEnemy) {
					adjEnemy = thisEnemy;
					validDirs.Clear ();
					validDirs.Add (dir);
				} else if (thisEnemy == adjEnemy) {
					validDirs.Add (dir);
				}
			}
		}
		return validDirs.ToArray ();
	}

	private int getActiveNeigbor(HexDirection direction){
		if (GetNeighbor (direction) && GetNeighbor (direction).GetActive()) {
			return GetNeighbor (direction).GetPlayer ();
		}
		return -1;
	}
}