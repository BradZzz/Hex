using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;

	private Color[] colors;
	private Color color;

	public TileInfo tile;
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

	public void init(Color[] colors, Text label){
		this.colors = colors;

		info = new UnitInfo ();
		info.playerNo = -1;
		info.type = UnitInfo.unitType.None;

		tile = new TileInfo ();
		tile.movement = 1;
		tile.meta = "";

		active = false;
		this.label = label;
		this.label.text = "";
	}

	public void setType(TileInfo.tileType type){
		switch(type){
		case TileInfo.tileType.Castle:
			GetTile ().type = TileInfo.tileType.Castle;
			GetTile ().color = TileInfo.tileColor.Blue;
			break;
		case TileInfo.tileType.City:
			GetTile ().type = TileInfo.tileType.City;
			GetTile ().color = TileInfo.tileColor.Brown;
			break;
		case TileInfo.tileType.Forest:
			GetTile ().type = TileInfo.tileType.Forest;
			GetTile ().color = TileInfo.tileColor.DarkGreen;
			break;
		case TileInfo.tileType.Grass:
			GetTile ().type = TileInfo.tileType.Grass;
			GetTile ().color = TileInfo.tileColor.Green;
			break;
		case TileInfo.tileType.Road:
			GetTile ().type = TileInfo.tileType.Road;
			GetTile ().color = TileInfo.tileColor.Sand;
			break;
		case TileInfo.tileType.Mountain:
			GetTile ().type = TileInfo.tileType.Mountain;
			GetTile ().color = TileInfo.tileColor.Gray;
			break;
		case TileInfo.tileType.Treasure:
			GetTile ().type = TileInfo.tileType.Treasure;
			GetTile ().color = TileInfo.tileColor.Gold;
			break;
		default:
			setType (TileInfo.tileType.Grass);
			break;
		}
	}

	private Color getColorTile(TileInfo.tileColor clr) {
		switch(clr){
		case TileInfo.tileColor.Blue:
			return Color.blue;
		case TileInfo.tileColor.Brown:
			return Color.magenta;
		case TileInfo.tileColor.DarkGreen:
			return new Color(0, .5f, .4f, 1);
		case TileInfo.tileColor.Gold:
			return Color.yellow;
		case TileInfo.tileColor.Gray:
			return Color.gray;
		case TileInfo.tileColor.Green:
			return Color.green;
		case TileInfo.tileColor.Sand:
			return new Color(.5f, .4f, .35f, 1);
		default:
			return Color.white;
		}
	}

	public void setColor(Color color) {
		this.color = color;
	}

	public Color getColor() {
		Color tmp = color;
		if (GetTile().fog) {
			return Color.black;
		}
		if (tile.color != TileInfo.tileColor.None) {
			if (GetPlayer () > -1) {
				return (colors[GetPlayer()] + new Color (1, 1, 1, .8f))/2;
			} else if (tmp != Color.white) {
				return (getColorTile(tile.color) + new Color (tmp.r, tmp.g, tmp.b, .2f)) / 2;
			} else {
				return getColorTile(tile.color);
			}
		}
		return color;
	}

	public TileInfo GetTile() { return tile; }
	public UnitInfo GetInfo() { return info; }
	public void SetInfoStart(UnitInfo info) { 
		this.info = info; 
		if (this.info.type != UnitInfo.unitType.None) {
			switch (this.info.type) {
			case UnitInfo.unitType.Knight:
				this.info.health = 2;
				break;
			case UnitInfo.unitType.Lancer:
				this.info.health = 3;
				break;
			case UnitInfo.unitType.Swordsman:
				this.info.health = 4;
				break;
			case UnitInfo.unitType.Adventure:
				this.info.health = 1;
				break;
			}
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
			case UnitInfo.unitType.Adventure:
				label.text = "A" + this.GetPlayer();
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
				info.actions = 4;
				info.attacks = 1;
				break;
			case UnitInfo.unitType.Lancer:
				info.actions = 3;
				info.attacks = 1;
				break;
			case UnitInfo.unitType.Swordsman:
				info.actions = 2;
				info.attacks = 1;
				break;
			case UnitInfo.unitType.Adventure:
				info.actions = 3;
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

	public void removeFog(){
		if (GetInfo().human == true) {
			GetTile ().fog = false;
			foreach (HexDirection dir in dirs) {
				if (GetNeighbor (dir)) {
					GetNeighbor (dir).GetTile ().fog = false;
				}
			}
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

	public HexDirection getActiveEnemy (int player){
		foreach (HexDirection dir in dirs) {
			if (GetNeighbor (dir) && GetNeighbor (dir).GetPlayer() > -1 && GetNeighbor (dir).GetPlayer() != player) {
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