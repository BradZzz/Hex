using System.Collections;
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
      
    TraverseMeta(locMeta.info);
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

  //Traverse all the nodes of the location and calculate events
  void TraverseMeta(LocationInfo node){
    node.visible = true;

    if(node.appearChance < 1){
      float pick = Random.Range (0, 100);
      float chance = node.appearChance * 100;

      Debug.Log ("Name: " + node.name);
      Debug.Log ("Pick: " + pick.ToString());
      Debug.Log ("Chance: " + chance.ToString());

      if (pick > chance) {
        node.visible = false;
        Debug.Log ("Making invisible");
      }
    }
    foreach (LocationInfo child in node.children) {
      TraverseMeta (child);
    }
  }

  LocationInfo[] GetChildNodes(LocationInfo parent){
    if (parent == null){
      return null;
    }
    List<LocationInfo> children = new List<LocationInfo> ();
    foreach (LocationInfo child in parent.children) {
      if (child.visible) {
        children.Add (child);
      }
    }
    return children.ToArray ();
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
//    Vector3 endPos = new Vector3(startPos.x * startPos.y + System.Math.Sign(5), startPos.z);
//    float factor = 1f;
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

    character.transform.position = startPos;

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
      if (GetChildNodes(info) != null && GetChildNodes(info).Length > i) {
        infoBtns [i].SetActive (true);
        infoBtns [i].GetComponentInChildren<Text> ().text = GetChildNodes(info) [i].name;
      } else if ((GetChildNodes(info) == null || GetChildNodes(info).Length == i) && (!sScreen || tStack.Count > 1)) {
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

    //
    if (tStack.Peek().nxtRes.Length > 0 && tStack.Peek().needRes.Length == 0) {
      Debug.Log ("Pop");
      gameState.Pop ();
    }

    return tStack.Pop ();
  }

  private GameInfo getDeref(){
    return JsonUtility.FromJson<GameInfo> (JsonUtility.ToJson (gameState.Peek()));
  }
    
  private void addItem(LocationInfo info){
    Debug.Log ("addItem");

    if (info.nxtRes.Length > 0) {
      GameInfo tGame = getDeref();
      foreach(ResInfo inf in info.nxtRes){
        switch(inf.type){
        case ResInfo.ResType.Unit:
          composeSquad(inf, tGame);
          break;
        case ResInfo.ResType.Upgrade:
          addAttribute(inf, tGame);
          break;
        case ResInfo.ResType.Resource:
          addResource(inf, tGame);
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
    if (!attribs.Contains(resI.name)) {
      attribs.Add (resI.name);
    }
    gameI.attributes = attribs.ToArray ();
  }

  void addResource(ResInfo resI, GameInfo gameI){
    Debug.Log ("Adding Resource: " + resI.name);
    if (resI.name.Equals("Ration")) {
      gameI.rations += resI.value;
      Debug.Log ("Bought Ration");
    }
  }

  private bool canAfford(LocationInfo nxtLoc){
    if (nxtLoc.needRes.Length > 0){
      foreach(ResInfo res in nxtLoc.needRes){
        switch(res.type){
        case ResInfo.ResType.Resource:
          GameInfo gameI = gameState.Pop ();
          if (res.name.Equals("Gold")) {
            if (gameI.gold >= res.value) {
              gameI.gold-=res.value;
            } else {
              gameState.Push(gameI);
              return false;
            }
          }
          gameState.Push(gameI);
          break;
        }
      }
    }
    return true;
  }

  public void ButtonClick(int sel){
    int clicked = sel - 1;

    Debug.Log ("Sel: " + sel.ToString());
    Debug.Log ("Debug.Log (tList.Count);");
    Debug.Log (tStack.Count);
//    Debug.Log ("thisInfo.children.Length: " + thisInfo.children.Length.ToString());
//
    Debug.Log (tStack);

    //We need to check in this if statement to check for resources required for option
    if (clicked >= GetChildNodes(tStack.Peek()).Length) {
      //Going back, so pop the gamestate if there is a resource we added
      Debug.Log (tStack.Peek().name);
      if (tStack.Count > 1) {
        Debug.Log ("Back");
        removeItem ();
        PopulateInfo (tStack.Peek());
      } else {
        Debug.Log ("Leave");
        BaseSaver.putGame (gameState.Pop());
        SceneManager.LoadScene ("AdventureScene");
      }
    } else {
      if (canAfford (GetChildNodes(tStack.Peek ()) [clicked])) {
        Debug.Log (GetChildNodes(tStack.Peek ())[clicked].name);
        addItem (GetChildNodes(tStack.Peek ())[clicked]);
        PopulateInfo (tStack.Peek ());
      } else {
        Debug.Log ("Cannot afford");
      }
    }
  }
}
