using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoButton : MonoBehaviour {

	private int armySize = 4;

	public void buttonClicked(int clkBtn){
		Debug.Log ("Button Clicked: " + clkBtn.ToString());
//		GameObject.Find ("ChoicePanel").GetComponent<ChoicePanel> ().selectChoice (clkBtn, 0, false);
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
		nGame.movement = 3;
		nGame.fatigue = 0;
    nGame.rations = 50;
    nGame.gold = 250;
    nGame.enemyRoster = new UnitInfo[armySize];
		for (int i = 0; i < armySize; i++) {
			UnitInfo info = new UnitInfo ();
			info.playerNo = 0;
			info.type = choiceArr[Random.Range(0, 3)];
			info.human = true;
      nGame.enemyRoster [i] = info;
		}
		BaseSaver.putGame (nGame);

    BaseSaver.putLocation ("StartScreen");
		SceneManager.LoadScene ("LocationScene");
	}
}
