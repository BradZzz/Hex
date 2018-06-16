using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoicePanel : MonoBehaviour {

	public GameObject cGlossary;

	private ChoiceGlossary glossy;

	private GameObject thisPlayer;
	private GameObject infoPnl;
	private GameObject[] infoBtns;

	private OptionInfo[] lstOptions;

  private int idx;

  UnitInfo.unitType[] choiceArr = new UnitInfo.unitType[] {
    UnitInfo.unitType.Knight,
    UnitInfo.unitType.Lancer,
    UnitInfo.unitType.Swordsman,
  };

  int getCharacterIdx(GameObject[] options, TileInfo.tileType type){
    int total = 0;
    Queue<int> chcRng = new Queue<int> ();
    foreach (GameObject opt in options) {
      ChoiceInfo chc = opt.GetComponent<ChoiceMain> ().choice;
      if (chc.firstLocation == type) {
        total += 4;
      } else if (chc.secondLocation == type) {
        total += 2;
      } else if (chc.thirdLocation == type) {
        total += 1;
      }
      chcRng.Enqueue (total);
    }
    int pick = Random.Range(0, total);
    while (chcRng.Count > 0) {
      if (chcRng.Dequeue() > pick) {
        return options.Length - chcRng.Count - 1;
      }
    }
    return options.Length - 1;
  }

	//At the start we need to pull out the player 
	//and attach it to the panel gameobject 
	void Start () {
		infoBtns = new GameObject[6];
		for (int i = 0; i < 6; i++) {
			infoBtns [i] = GameObject.Find ("Button_0" + (i + 1).ToString ());
		}
		infoPnl = GameObject.Find ("InfoPanel");

		glossy = cGlossary.GetComponent<ChoiceGlossary> ();

    Debug.Log ("Character: " + BaseSaver.getChoiceCharacter ().ToString());

    bool callBack = BaseSaver.getPicked () > -1;

    if (callBack) {
      idx = BaseSaver.getCharIdx ();
    } else {
      idx = getCharacterIdx(glossy.options, BaseSaver.getChoiceCharacter ());
    }

    Debug.Log ("Populating: " + idx.ToString());

		populateInfoPanel (glossy);

    if (callBack) {
      Debug.Log ("Picked: " + BaseSaver.getPicked ().ToString ());
      selectChoice (BaseSaver.getPicked (), true);
    }

//    populateInfoPanel (glossy);

    GameObject.Find("ForegroundImage").GetComponent<Image>().sprite = glossy.options[idx].GetComponent<Image>().sprite;
	}

	public void selectChoice(int btn, bool callback) {
    ChoiceInfo choice = glossy.options[idx].GetComponent<ChoiceMain>().choice;
		OptionInfo option = lstOptions [btn - 1];

		if (callback) {
			GameObject.Find ("InfoDescription").GetComponent<Text> ().text = BaseSaver.getBattle().won ? choice.winningGreeting : choice.losingGreeting;

			OptionInfo final = new OptionInfo ();
			final.TextOptions = new string[]{ "Continue" };
			final.result = OptionInfo.resultType.None;
			final.reaction = "<confirm/>";

			populateInfoButtons (new OptionInfo[]{ final });

      BaseSaver.resetBattle ();
			BaseSaver.resetChoice ();
		} else {
			if (!option.reaction.Equals ("<confirm/>")) {
				GameObject.Find ("InfoDescription").GetComponent<Text> ().text = option.reaction;

				OptionInfo final = new OptionInfo ();
				final.TextOptions = new string[]{ "Continue" };
				final.result = (option.result == OptionInfo.resultType.MiniGame || option.result == OptionInfo.resultType.Battle)
					? option.result : OptionInfo.resultType.None;
				final.reaction = "<confirm/>";

				populateInfoButtons (new OptionInfo[]{ final });
			} else {
				if (option.result == OptionInfo.resultType.MiniGame) {
					BaseSaver.putChoice(choice, idx, btn);
					SceneManager.LoadScene ("MiniGameScene");
				} else {
					if (option.result == OptionInfo.resultType.Battle) {
						BaseSaver.putChoice(choice, idx, btn);

            GameInfo game = BaseSaver.getGame ();

            BattleInfo battle = new BattleInfo ();
            battle.redirect = "ChoiceScene";
            battle.playerRoster = game.playerRoster;

            int enemies = 2;
            battle.enemyRoster = new UnitInfo[enemies];
            for (int i = 0; i < enemies; i++) {
              UnitInfo info = new UnitInfo ();
              info.playerNo = 0;
              info.type = choiceArr[Random.Range(0, 3)];
              Debug.Log ("Choice: " + info.type);
              info.human = true;
              battle.enemyRoster [i] = info;
            }
            BaseSaver.putBattle (battle);

						SceneManager.LoadScene ("BattleScene");
					} else {
						SceneManager.LoadScene ("AdventureScene");
					}
				}
			}
		}
	}

	void populateInfoPanel(ChoiceGlossary glossy){
    ChoiceInfo choice = glossy.options[idx].GetComponent<ChoiceMain>().choice;
		GameObject.Find ("InfoHeader").GetComponent<Text> ().text = choice.name;
		GameObject.Find ("InfoDescription").GetComponent<Text> ().text = choice.openingGreeting;

		populateInfoButtons(choice.options);
	}

	void populateInfoButtons(OptionInfo[] options){
		for (int i = 0; i < 6; i++) {
			if (i < options.Length) {
				infoBtns [i].SetActive (true);
				infoBtns [i].transform.transform.Find("Text").GetComponent<Text> ().text = options[i].TextOptions[0];
			} else {
				infoBtns [i].SetActive (false);
			}
		}

		lstOptions = options;
	}
}
