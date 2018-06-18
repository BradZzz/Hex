using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestPanel : MonoBehaviour {

  protected List<QuestInfo> availableQuests;
  private Sprite locSprite;
  private GameObject[] infoBtns;
  private string[] mainButtons;
  private static int BTNLENGTH = 6;

  private GameObject header;
  private GameObject description;
  private GameObject image;
  private GameObject character;

  private bool sScreen;
  private Stack<GameInfo> gameState;

  private QLocType thisLoc;
  private int thisSel;

  public enum QLocType {
    Main, Quests, Attribs, Stats, Enemies, None
  }

  void Start(){
    Debug.Log ("Start");

    header = GameObject.Find ("InfoHeader");
    description = GameObject.Find ("InfoDescription");
    image = GameObject.Find ("InfoImage");
    character = GameObject.Find ("InfoCharacter");

    infoBtns = new GameObject[BTNLENGTH];
    for (int i = 0; i < BTNLENGTH; i++) {
      infoBtns [i] = GameObject.Find ("Button_0" + (i + 1).ToString ());
      infoBtns [i].SetActive (false);
    }

    thisLoc = QLocType.Main;
    mainButtons = new string[]{ "Quests" };
    thisSel = -1;

    PopulateInfo(thisLoc, thisSel);
  }

  void PopulateInfo(QLocType screen, int selection){
    Debug.Log ("thisLoc");
    Debug.Log (thisLoc);
    GameInfo game = BaseSaver.getGame ();
    switch(screen){
    case QLocType.Main:
      Debug.Log ("Main");
      header.GetComponent<Text> ().text = "Player Menu";
      description.GetComponent<Text> ().text = "Click on the buttons on the side to see the current information about your character.";
      PopulateButtons (mainButtons);
      break;
    case QLocType.Quests:
      Debug.Log ("Quests");
      if (selection > -1){
        header.GetComponent<Text> ().text = game.quests[selection].title;
        description.GetComponent<Text> ().text = game.quests[selection].startMsg;
        PopulateButtons (new string[]{});
      } else {
        header.GetComponent<Text> ().text = "Quests";
        description.GetComponent<Text> ().text = "Here are your character's active quests. Click on one to see more information and make it active.";
        string[] qsts = new string[game.quests.Length];
        for(int i = 0; i < game.quests.Length; i++){
          qsts [i] = game.quests [i].title;
        }
        PopulateButtons (qsts);
      }
      break;
    case QLocType.Attribs:
      Debug.Log ("Attribs");
      break;
    case QLocType.Stats:
      Debug.Log ("Stats");
      break;
    case QLocType.Enemies:
      Debug.Log ("Enemies");
      break;
    }
  }

  void PopulateButtons(string[] titles){
    //    tStack.Push (info);
    for(int i = 0; i < BTNLENGTH; i++) {
      if (i < titles.Length) {
        infoBtns [i].SetActive (true);
        infoBtns [i].GetComponentInChildren<Text> ().text = titles[i];
      } else if (i == titles.Length) {
        infoBtns [i].SetActive (true);
        string txt = "Leave";
        if (QLocType.Main != thisLoc) {
          txt = "Back";
        }
        infoBtns [i].GetComponentInChildren<Text> ().text = txt;
      } else {
        infoBtns [i].SetActive(false);
      }
    }
  }

  public void ButtonClick(int sel){
    int clicked = sel - 1;
    Debug.Log ("Sel: " + sel.ToString());

    GameInfo game = BaseSaver.getGame ();


    if (clicked < mainButtons.Length && thisLoc == QLocType.Main) {
      thisLoc = (QLocType)System.Enum.Parse(typeof(QLocType), mainButtons[clicked]);
    } else if (thisLoc == QLocType.Quests && thisSel == -1 && clicked < game.quests.Length) {
      thisSel = clicked;
    } else {
      if (thisSel > -1) {
        thisSel = -1;
      } else if (thisLoc != QLocType.Main) {
        thisLoc = QLocType.Main;
      } else {
        SceneManager.LoadScene ("AdventureScene");
      }
    }
    PopulateInfo(thisLoc, thisSel);
  }
}
