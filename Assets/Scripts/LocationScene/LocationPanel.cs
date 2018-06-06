using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationPanel : MonoBehaviour {

  public GameObject locationMain;

  private LocationInfo thisInfo;
  private LocationMain locMeta;
  private Sprite locSprite;
  private GameObject[] infoBtns;
  private static int BTNLENGTH = 6;

  void Awake(){
    locMeta = locationMain.GetComponent<LocationMain> ();
    locSprite = locationMain.GetComponent<SpriteRenderer> ().sprite;

    thisInfo = locMeta.info;
  }

  void Start(){
    infoBtns = new GameObject[BTNLENGTH];
    for (int i = 0; i < BTNLENGTH; i++) {
      infoBtns [i] = GameObject.Find ("Button_0" + (i + 1).ToString ());
      infoBtns [i].SetActive (false);
    }

    PopulateInfo(thisInfo);
  }

  void PopulateInfo(LocationInfo info){
    GameObject.Find ("InfoHeader").GetComponent<Text> ().text = info.name;
    GameObject.Find ("InfoDescription").GetComponent<Text> ().text = info.description;
    GameObject.Find ("InfoImage").GetComponent<Image> ().sprite = locSprite;

    PopulateButtons (info);
  }

  void PopulateButtons(LocationInfo info){
    for(int i = 0; i < BTNLENGTH; i++) {
      if (info.children.Length > i) {
        infoBtns [i].SetActive (true);
        infoBtns [i].GetComponentInChildren<Text> ().text = info.children [i].name;
      } else if (info.children.Length == i) {
        infoBtns [i].SetActive (true);
        string txt = "Leave";
        if (thisInfo.parent != null) {
          txt = "Back";
        }
        infoBtns [i].GetComponentInChildren<Text> ().text = txt;
      } else {
        infoBtns [i].SetActive (false);
      }
    }
  }

  public void ButtonClick(int sel){
    int clicked = sel - 1;

    Debug.Log ("Sel: " + clicked.ToString());
    Debug.Log ("thisInfo.children.Length: " + thisInfo.children.Length.ToString());

    if (clicked >= thisInfo.children.Length) {
      Debug.Log (thisInfo);
      if (thisInfo.parent != null) {
        Debug.Log ("Back");
        thisInfo = thisInfo.parent[0];
        PopulateInfo (thisInfo);
      } else {
        Debug.Log ("Leave");
      }
    } else {
      Debug.Log (thisInfo.children[clicked].name);

      LocationInfo iTemp = thisInfo;
      thisInfo = thisInfo.children [clicked];
      thisInfo.parent = new LocationInfo[]{iTemp};

      PopulateInfo (thisInfo);
    }
  }

//  public GameObject cGlossary;
//
//  private ChoiceGlossary glossy;
//  private GameObject thisPlayer;
//  private GameObject infoPnl;
//  private GameObject[] infoBtns;
//
//  private OptionInfo[] lstOptions;
//
//  //At the start we need to pull out the player 
//  //and attach it to the panel gameobject 
//  void Start () {
//    infoBtns = new GameObject[6];
//    for (int i = 0; i < 6; i++) {
//      infoBtns [i] = GameObject.Find ("Button_0" + (i + 1).ToString ());
//    }
//    infoPnl = GameObject.Find ("InfoPanel");
//
//    glossy = cGlossary.GetComponent<ChoiceGlossary> ();
//    populateInfoPanel (glossy);
//
//    if (BaseSaver.getPicked() > -1) {
//      Debug.Log ("Picked: " + BaseSaver.getPicked ().ToString ());
//      selectChoice(BaseSaver.getPicked(), true);
//    }
//  }
//
//  public void selectChoice(int btn, bool callback) {
//    ChoiceInfo choice = glossy.choices [0];
//    OptionInfo option = lstOptions [btn - 1];
//
//    if (callback) {
//      GameObject.Find ("InfoDescription").GetComponent<Text> ().text = BaseSaver.getBattle().won ? choice.winningGreeting : choice.losingGreeting;
//
//      OptionInfo final = new OptionInfo ();
//      final.TextOptions = new string[]{ "Continue" };
//      final.result = OptionInfo.resultType.None;
//      final.reaction = "<confirm/>";
//
//      populateInfoButtons (new OptionInfo[]{ final });
//      BaseSaver.resetChoice ();
//    } else {
//      if (!option.reaction.Equals ("<confirm/>")) {
//        GameObject.Find ("InfoDescription").GetComponent<Text> ().text = option.reaction;
//
//        OptionInfo final = new OptionInfo ();
//        final.TextOptions = new string[]{ "Continue" };
//        final.result = (option.result == OptionInfo.resultType.MiniGame || option.result == OptionInfo.resultType.Battle)
//          ? option.result : OptionInfo.resultType.None;
//        final.reaction = "<confirm/>";
//
//        populateInfoButtons (new OptionInfo[]{ final });
//      } else {
//        if (option.result == OptionInfo.resultType.MiniGame) {
//          BaseSaver.putChoice(choice, btn);
//          SceneManager.LoadScene ("MiniGameScene");
//        } else {
//          if (option.result == OptionInfo.resultType.Battle) {
//            BaseSaver.putChoice(choice, btn);
//            SceneManager.LoadScene ("BattleScene");
//          } else {
//            SceneManager.LoadScene ("AdventureScene");
//          }
//        }
//      }
//    }
//  }
//
//  void populateInfoPanel(ChoiceGlossary glossy){
//    ChoiceInfo choice = glossy.choices [0];
//    GameObject.Find ("InfoHeader").GetComponent<Text> ().text = choice.name;
//    GameObject.Find ("InfoDescription").GetComponent<Text> ().text = choice.openingGreeting;
//
//    populateInfoButtons(choice.options);
//  }
//
//  void populateInfoButtons(OptionInfo[] options){
//    for (int i = 0; i < 6; i++) {
//      if (i < options.Length) {
//        infoBtns [i].SetActive (true);
//        infoBtns [i].transform.transform.Find("Text").GetComponent<Text> ().text = options[i].TextOptions[0];
//      } else {
//        infoBtns [i].SetActive (false);
//      }
//    }
//
//    lstOptions = options;
//  }
}
