﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LocationPanel : MonoBehaviour {

//  public GameObject locationMain;

  public GameObject startScreen;

  public GameObject[] locations;

  protected Stack<LocationInfo> tStack;
  private LocationMain locMeta;
  private Sprite locSprite;
  private GameObject[] infoBtns;
  private static int BTNLENGTH = 6;

  private GameObject header;
  private GameObject description;
  private GameObject image;
  private GameObject character;

  private bool sScreen;
  private Stack<GameInfo> gameState;

  void getLocation(){
    locMeta = locations[0].GetComponent<LocationMain> ();
    locSprite = locations[0].GetComponent<SpriteRenderer> ().sprite;

    string locationName = BaseSaver.getLocation ();
    Debug.Log ("locationName: " + locationName);
    sScreen = false;

    if (locationName.Length > 0) {
      BaseSaver.resetLocation ();
      if (locationName.Equals ("StartScreen")) {
        locMeta = startScreen.GetComponent<LocationMain> ();
        locSprite = startScreen.GetComponent<SpriteRenderer> ().sprite;
        sScreen = true;
      } else {
        foreach (GameObject location in locations) {
          if (location.name.Equals(locationName)){
            locMeta = location.GetComponent<LocationMain> ();
            locSprite = location.GetComponent<SpriteRenderer> ().sprite;
          }
        }
      }
    }
  }

  void Awake(){
    Debug.Log ("Awake");

    gameState = new Stack<GameInfo> ();
    gameState.Push (BaseSaver.getGame());

    getLocation ();
//
//    locMeta = locations[0].GetComponent<LocationMain> ();
//    locSprite = locations[0].GetComponent<SpriteRenderer> ().sprite;
  }

  void Start(){
    Debug.Log ("Start");

    header = GameObject.Find ("InfoHeader");
    description = GameObject.Find ("InfoDescription");
    image = GameObject.Find ("InfoImage");
    character = GameObject.Find ("InfoCharacter");

    tStack = new Stack<LocationInfo>();

    infoBtns = new GameObject[BTNLENGTH];
    for (int i = 0; i < BTNLENGTH; i++) {
      infoBtns [i] = GameObject.Find ("Button_0" + (i + 1).ToString ());
      infoBtns [i].SetActive (false);
    }

    tStack.Push (locMeta.info);

    PopulateInfo(tStack.Peek());
  }

  private IEnumerator Jump(GameObject character)
  {
    Vector3 startPos = character.transform.position;
    float t = 0;
    Vector3 endPos = new Vector3(startPos.x * startPos.y + System.Math.Sign(5), startPos.z);
    float factor = 1f;
    float moveSpeed = 1f;

    while (t < 1f)
    {
      t += Time.deltaTime * moveSpeed;
      float y = 0;
      if (t < .5f) {
        y = character.transform.position.y + t;
      } else {
        y = character.transform.position.y + (t - 1);
      }
      Vector3 pos = new Vector3(character.transform.position.x, y, character.transform.position.z);
      character.transform.position = pos;

      yield return null;
    }

    while(t < .5f){
      t += Time.deltaTime * moveSpeed;
      float y = character.transform.position.y + (.5f - t);
      Vector3 pos = new Vector3(character.transform.position.x, y, character.transform.position.z);
      character.transform.position = pos;

      yield return null;
    }

    yield return 0;
  }

  void PopulateInfo(LocationInfo info){
    if (info.nxtScene.Length > 0) {
      BaseSaver.putGame (gameState.Pop ());
      SceneManager.LoadScene (info.nxtScene);
    }

    header.GetComponent<Text> ().text = info.header;
    description.GetComponent<Text> ().text = info.description;
    if (LocationInfo.LocType.None != info.img) {
      Sprite img = info.gameObject.GetComponent<Image> ().sprite;
      if (info.img == LocationInfo.LocType.Char) {
        character.SetActive (true);
        character.GetComponent<Image> ().sprite = img;

        StartCoroutine(Jump(character));
      } else {
        image.GetComponent<Image> ().sprite = img;
        character.SetActive (false);
      }
    } else {
      character.SetActive (false);
    }

    PopulateButtons (info);
  }

  void PopulateButtons(LocationInfo info){
    //    tStack.Push (info);
    for(int i = 0; i < BTNLENGTH; i++) {
      if (info.children != null && info.children.Length > i) {
        infoBtns [i].SetActive (true);
        infoBtns [i].GetComponentInChildren<Text> ().text = info.children [i].name;
      } else if ((info.children == null || info.children.Length == i) && (!sScreen || tStack.Count > 1)) {
        infoBtns [i].SetActive (true);
        string txt = "Leave";
        if (tStack.Count > 1) {
          txt = "Back";
        }
        infoBtns [i].GetComponentInChildren<Text> ().text = txt;
      } else {
        infoBtns [i].SetActive(false);
      }
    }
  }

  private LocationInfo removeItem(){
    Debug.Log ("removeItem");

    if (tStack.Peek().nxtRes.Length > 0) {
      gameState.Pop ();
    }

    return tStack.Pop ();
  }
    
  private void addItem(LocationInfo info){
    Debug.Log ("addItem");

    //Moving forward here, so populate the player as they need to be populated

    if (info.nxtRes.Length > 0) {
      GameInfo tGame = gameState.Peek();
      foreach(ResInfo inf in info.nxtRes){
        switch(inf.type){
        case ResInfo.ResType.Unit:
          composeSquad(inf, tGame);
          break;
        case ResInfo.ResType.Upgrade:
          addAttribute(inf, tGame);
          break;
        }
      }

      gameState.Push (tGame);
    }

    tStack.Push (info);
  }

  void composeSquad(ResInfo resI, GameInfo gameI){
    List<string> typeArr = new List<string>(new string[]{"K","S","L"});
    List<UnitInfo> roster = new List<UnitInfo> (gameI.playerRoster);

    for(int i = 0; i < resI.value; i++){
      UnitInfo unitI = new UnitInfo ();
      unitI.playerNo = 0;
      unitI.type = (UnitInfo.unitType)typeArr.IndexOf(resI.name);
      unitI.human = true;
      roster.Add (unitI);
      gameI.playerRoster = roster.ToArray ();
    }
  }

  void addAttribute(ResInfo resI, GameInfo gameI){
    List<string> attribs = new List<string> (gameI.attributes);
    attribs.Add (resI.name);
    gameI.attributes = attribs.ToArray ();
  }


  public void ButtonClick(int sel){
    int clicked = sel - 1;

    Debug.Log ("Sel: " + sel.ToString());
    Debug.Log ("Debug.Log (tList.Count);");
    Debug.Log (tStack.Count);
//    Debug.Log ("thisInfo.children.Length: " + thisInfo.children.Length.ToString());
//
    Debug.Log (tStack);

    if (clicked >= tStack.Peek().children.Length) {
      //Going back, so pop the gamestate if there is a resource we added
      Debug.Log (tStack.Peek().name);
      if (tStack.Count > 1) {
        Debug.Log ("Back");
        removeItem ();
        PopulateInfo (tStack.Peek());
      } else {
        Debug.Log ("Leave");
        SceneManager.LoadScene ("AdventureScene");
      }
    } else {
      Debug.Log (tStack.Peek().children[clicked].name);
      addItem (tStack.Peek().children [clicked]);
      PopulateInfo (tStack.Peek());
    }
  }
}
