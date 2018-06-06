using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LocationPanel : MonoBehaviour {

  /*
   * 
   * 
   * 
   */ 

  public GameObject locationMain;

  private LocationInfo thisInfo;
  private Stack<LocationInfo> tStack = new Stack<LocationInfo>();
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
    tStack = new Stack<LocationInfo>();

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
    //    tStack.Push (info);
    for(int i = 0; i < BTNLENGTH; i++) {
      if (info.children != null && info.children.Length > i) {
        infoBtns [i].SetActive (true);
        infoBtns [i].GetComponentInChildren<Text> ().text = info.children [i].name;
      } else if (info.children == null || info.children.Length == i) {
        infoBtns [i].SetActive (true);
        string txt = "Leave";
        if (tStack.Count > 0) {
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
      if (tStack.Count > 0) {
        Debug.Log ("Back");
        thisInfo = tStack.Pop ();
        PopulateInfo (thisInfo);
      } else {
        Debug.Log ("Leave");
        SceneManager.LoadScene ("AdventureScene");
      }
    } else {
      Debug.Log (thisInfo.children[clicked].name);

      tStack.Push (thisInfo);
      thisInfo = thisInfo.children [clicked];
      PopulateInfo (thisInfo);
    }
  }
}
