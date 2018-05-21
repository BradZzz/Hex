using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapAdventure : MonoBehaviour {

	public Color[] colors = new Color[]{Color.white};

	public HexGridAdventure hexGrid;

	private Color activeColor;

	void Update () {
		if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
			HandleInput();
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			hexGrid.ColorCell(hit.point, Color.white);
		}
	}

	public void EndTurn () {
		hexGrid.EndTurn ();
	}

	public void PlayAI () {
		hexGrid.PlayAI ();
	}
}