using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoButton : MonoBehaviour {

//	public void hidePanel() {
//		GameObject.Find ("InfoPanel").SetActive (false);
//		GameObject.Find ("ChoicePanel").GetComponent<ChoicePanel> ().start ();
//	}

	public void buttonClicked(int clkBtn){
		Debug.Log ("Button Clicked: " + clkBtn.ToString());
		GameObject.Find ("ChoicePanel").GetComponent<ChoicePanel> ().selectChoice (clkBtn, false);
	}

	public void startGame(){
		SceneManager.LoadScene ("AdventureScene");
	}
}
