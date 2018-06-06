using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LocationPanel : MonoBehaviour {

  public GameObject locationMain;

  private LocationInfo thisInfo;
  protected Stack<LocationInfo> tStack;
  private LocationMain locMeta;
  private Sprite locSprite;
  private GameObject[] infoBtns;
  private static int BTNLENGTH = 6;

  private int lClick = -1;

  void Awake(){
    Debug.Log ("Awake");

    tStack = new Stack<LocationInfo>();

    locMeta = locationMain.GetComponent<LocationMain> ();
    locSprite = locationMain.GetComponent<SpriteRenderer> ().sprite;

    thisInfo = locMeta.info;
  }

  void Start(){
    Debug.Log ("Start");

    infoBtns = new GameObject[BTNLENGTH];
    for (int i = 0; i < BTNLENGTH; i++) {
      infoBtns [i] = GameObject.Find ("Button_0" + (i + 1).ToString ());
      infoBtns [i].SetActive (false);
    }

    PopulateInfo(thisInfo);
  }

  void OnEnable(){
    Debug.Log ("OnEnable");
  }

  void PopulateInfo(LocationInfo info){
    GameObject.Find ("InfoHeader").GetComponent<Text> ().text = info.name;
    GameObject.Find ("InfoDescription").GetComponent<Text> ().text = info.description;
    GameObject.Find ("InfoImage").GetComponent<Image> ().sprite = locSprite;

    PopulateButtons (info);
  }

  void PopulateButtons(LocationInfo info){
    //    tStack.Push (info);
    for(int i = 0; i < BTNLENGTH; i++) {
      if (info.children != null && info.children.Length > i) {
        infoBtns [i].SetActive (true);
        infoBtns [i].GetComponentInChildren<Text> ().text = info.children [i].name;
      } else if (info.children == null || info.children.Length == i) {
        infoBtns [i].SetActive (true);
        string txt = "Leave";
        if (tStack.Count < 1) {
          txt = "Back";
        }
        infoBtns [i].GetComponentInChildren<Text> ().text = txt;
      } else {
        infoBtns [i].SetActive(false);
      }
    }

//    Debug.Log ("List");
//    Debug.Log (tList.Count);
  }

//  void Update(){
//    if (lClick != -1) {
//      Debug.Log ("Click!: " + lClick.ToString());
//      if (lClick >= thisInfo.children.Length) {
//        Debug.Log (thisInfo.name);
//        if (tList.Count > 0) {
//          Debug.Log ("Back");
//          thisInfo = removeItem();
//          PopulateInfo (thisInfo);
//        } else {
//          Debug.Log ("Leave");
//          SceneManager.LoadScene ("AdventureScene");
//        }
//      } else {
//        Debug.Log (thisInfo.children[lClick].name);
//
//        addItem (thisInfo);
//
//        thisInfo = thisInfo.children [lClick];
//        PopulateInfo (thisInfo);
//      }
//
//      lClick = -1;
//    }
//    Debug.Log (tList.Count);
//    Debug.Log ("Click: " + lClick.ToString());
//  }

  private LocationInfo removeItem(){
    Debug.Log ("removeItem");
    return tStack.Pop ();
//    return BaseSaver.getLocation ();
  }

  private void addItem(LocationInfo info){
    Debug.Log ("addItem");
    tStack.Push (info);
//    BaseSaver.putLocation (info);
  }

  public void ButtonClick(int sel){
    int clicked = sel - 1;

    Debug.Log ("Sel: " + sel.ToString());
    Debug.Log ("Debug.Log (tList.Count);");
    Debug.Log (tStack.Count);
    Debug.Log ("thisInfo.children.Length: " + thisInfo.children.Length.ToString());

    if (clicked >= thisInfo.children.Length) {
      Debug.Log (thisInfo.name);
      if (tStack.Count > 0) {
        Debug.Log ("Back");
        thisInfo = removeItem();
        PopulateInfo (thisInfo);
      } else {
        Debug.Log ("Leave");
        SceneManager.LoadScene ("AdventureScene");
      }
    } else {
      Debug.Log (thisInfo.children[clicked].name);

      addItem (thisInfo);

      thisInfo = thisInfo.children [clicked];
      PopulateInfo (thisInfo);
    }
  }
}
