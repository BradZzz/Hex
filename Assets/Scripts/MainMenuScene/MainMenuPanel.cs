using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuPanel : MonoBehaviour {

  private int armySize = 6;

	// Use this for initialization
	void Start () {
    List<string> m_DropOptions = new List<string>();
    Dropdown m_Dropdown = GameObject.Find("LevelDropdown").GetComponent<Dropdown>();
    m_Dropdown.ClearOptions();
    foreach(MapInfo map in BaseSaver.getMaps()){ m_DropOptions.Add (map.name);}
    m_Dropdown.AddOptions(m_DropOptions);
    BaseSaver.putNextMap (m_DropOptions[0]);
	}

  public void changed(){
    List<string> m_DropOptions = new List<string>();
    Dropdown m_Dropdown = GameObject.Find("LevelDropdown").GetComponent<Dropdown>();
    foreach(MapInfo map in BaseSaver.getMaps()){ m_DropOptions.Add (map.name);}

    foreach(string opt in m_DropOptions){
      Debug.Log (opt);
    }

    Debug.Log (m_Dropdown.value);

    BaseSaver.putNextMap (m_DropOptions[m_Dropdown.value]);
  }

  public void startGameButton(){
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
    nGame.reputation = 0;
    nGame.gold = 250;
    nGame.enemyRoster = new UnitInfo[armySize];
    for (int i = 0; i < armySize; i++) {
      UnitInfo info = new UnitInfo ();
      info.playerNo = 0;
      if (i < 2) {
        info.type = UnitInfo.unitType.Monster;
      } else {
        info.type = choiceArr[Random.Range(0, 3)];
      }
      info.human = true;
      nGame.enemyRoster [i] = info;
    }
    BaseSaver.putGame (nGame);

    BaseSaver.putLocation ("StartScreen");
    SceneManager.LoadScene ("LocationScene");
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
