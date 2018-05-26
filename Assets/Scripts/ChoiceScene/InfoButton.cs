using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoButton : MonoBehaviour {

	public void hidePanel() {
		GameObject.Find ("InfoPanel").SetActive (false);
	}
}
