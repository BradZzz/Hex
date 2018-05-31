using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoButton : MonoBehaviour {

	private int armySize = 3;

	public void buttonClicked(int clkBtn){
		Debug.Log ("Button Clicked: " + clkBtn.ToString());
		GameObject.Find ("ChoicePanel").GetComponent<ChoicePanel> ().selectChoice (clkBtn, false);
	}

	public void startGame(){
		BaseSaver.resetAll ();

		/*
		 * Create the new player here
		 */

		UnitInfo.unitType[] choiceArr = new UnitInfo.unitType[] {
			UnitInfo.unitType.Knight,
			UnitInfo.unitType.Lancer,
			UnitInfo.unitType.Swordsman,
		};

		GameInfo nGame = new GameInfo ();
		nGame.name = "General Reginald Longbottom";
		nGame.roster = new UnitInfo[armySize];
		for (int i = 0; i < armySize; i++) {
			UnitInfo info = new UnitInfo ();
			info.playerNo = 0;
			info.type = choiceArr[Random.Range(0, 3)];
			info.human = true;
			nGame.roster [i] = info;
		}
		BaseSaver.putGame (nGame);

		SceneManager.LoadScene ("AdventureScene");
	}
}
