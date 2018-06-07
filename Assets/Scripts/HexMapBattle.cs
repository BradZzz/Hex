using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapBattle : MonoBehaviour {

	public Color[] colors;
	public HexGridBattle hexGrid;
	private Color activeColor;

//	void Awake () {
//		SelectColor(0);
//	}

	void Update () {
		if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
      Debug.Log ("Click");
			HandleInput();
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
      Debug.Log ("Paint");
      hexGrid.ColorCell(hit.point, activeColor);
		}
	}

//	public void SelectColor (int index) {
//		activeColor = colors[index];
//	}

	public void EndTurn () {
		hexGrid.EndTurn ();
	}

	public void PlayAI () {
		hexGrid.PlayAI ();
	}
}