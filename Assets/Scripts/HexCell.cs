using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;

	public Color color;
	public UnitInfo info;

	private bool active;

	public void init(){
		info = new UnitInfo ();
		info.playerNo = -1;

		active = false;
	}

	public UnitInfo GetInfo() { return info; }
	public void SetInfo(UnitInfo info) { this.info = info; }

	public int GetPlayer() { return info.playerNo; }

	public bool GetActive() { return active; }
	public void SetActive(bool active) { this.active = active; }

	[SerializeField]
	HexCell[] neighbors;

	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

	public void paintNeigbors (){
		setNeigbor(HexDirection.NE);
		setNeigbor(HexDirection.NW);
		setNeigbor(HexDirection.SE);
		setNeigbor(HexDirection.SW);
		setNeigbor(HexDirection.W);
		setNeigbor(HexDirection.E);
	}

	private void setNeigbor(HexDirection direction){
		if (GetNeighbor (direction) && GetNeighbor (direction).GetPlayer() == -1) {
			GetNeighbor (direction).color = Color.gray;
		}
	}

	public HexDirection getActiveNeigbor (){
		HexDirection[] dirs = { 
			HexDirection.NE,
			HexDirection.NW,
			HexDirection.SE,
			HexDirection.SW,
			HexDirection.W,
			HexDirection.E
		};
		foreach (HexDirection dir in dirs) {
			if (getActiveNeigbor (dir) > -1) {
				return dir;
			}
		}
		return HexDirection.None;
	}

	private int getActiveNeigbor(HexDirection direction){
		if (GetNeighbor (direction) && GetNeighbor (direction).GetActive()) {
			return GetNeighbor (direction).GetPlayer ();
		}
		return -1;
	}
}