using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoButton : MonoBehaviour {

	public void hidePanel() {
		GameObject.Find ("InfoPanel").SetActive (false);
		GameObject.Find ("ChoicePanel").GetComponent<ChoicePanel> ().start ();
	}

	public void buttonClicked(int clkBtn){
		Debug.Log ("Button Clicked: " + clkBtn.ToString());
	}
}
