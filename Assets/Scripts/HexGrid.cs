using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

	/*
	 * TODO:
	 * Start the player in one section of the board and only allow them to move onto the tiles that are shaded red
	 * add a second player and make sure that the selected charater is toggled correctly when clicked
	 * 
	 */

	public int width = 6;
	public int height = 6;

	public Color defaultColor = Color.white;

	public Color[] players;

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

//		HexCell cell = cells[0];
//		cell.color = players[0];
//		cell.paintNeigbors ();
//		cell.SetActive (true);
//		cell.SetPlayer (0);
//
//		HexCell cell2 = cells[cells.Length - 1];
//		cell2.color = players[1];
//		cell2.SetPlayer (1);

		placePlayer(cells[0], 0, true);
		placePlayer(cells[cells.Length - 1], 1, false);
		placePlayer(cells[cells.Length - width], 2, false);
		placePlayer(cells[0 + width - 1], 3, false);


		hexMesh.Triangulate(cells);
	}

	void placePlayer(HexCell cell, int idx, bool active){
		cell.color = players[idx];
		cell.SetPlayer (idx);
		cell.SetActive (active);
		if (active) {
			cell.paintNeigbors ();
		}
	}

	public void ColorCell (Vector3 position, Color color) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;

		HexCell cell = cells [index];
		if (cell.GetPlayer () > -1) {
			if (cell.GetActive ()) {
				ResetCells ();
			} else {
				ResetCells ();
				cell.paintNeigbors ();
				cell.SetActive (true);
			}
		} else {
			HexDirection dir = cell.getActiveNeigbor ();
			if (dir != HexDirection.None) {
				int player = cell.GetNeighbor (dir).GetPlayer ();
				cell.SetPlayer (player);
				cell.color = players [player];
				cell.GetNeighbor (dir).SetActive(false);
				cell.GetNeighbor (dir).SetPlayer(-1);

				ResetCells ();
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
		cell.init ();

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
	}
}