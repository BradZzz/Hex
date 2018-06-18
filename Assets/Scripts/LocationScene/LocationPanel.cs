using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LocationPanel : MonoBehaviour {

//  public GameObject locationMain;

  public GameObject startScreen;

  public GameObject[] locations;
  public GameObject[] quests;

  public HexCell HexCellPrefab; 

  protected List<QuestInfo> availableQuests;
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

  void Awake(){
    Debug.Log ("Awake");

    gameState = new Stack<GameInfo> ();
    availableQuests = new List<QuestInfo> ();
    foreach(GameObject quest in quests){
      availableQuests.Add (quest.GetComponent<QuestMain>().quest);
    }

    gameState.Push (BaseSaver.getGame());

    getLocation ();
//
//    locMeta = locations[0].GetComponent<LocationMain> ();
//    locSprite = locations[0].GetComponent<SpriteRenderer> ().sprite;
  }

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

  //Traverse all the nodes of the location and calculate events
  void TraverseMeta(LocationInfo node){
    node.visible = true;

    if(node.appearChance < 1){
      float pick = UnityEngine.Random.Range (0, 100);
      float chance = node.appearChance * 100;

      Debug.Log ("Name: " + node.name);
      Debug.Log ("Pick: " + pick.ToString());
      Debug.Log ("Chance: " + chance.ToString());

      if (pick > chance) {
        node.visible = false;
        Debug.Log ("Making invisible");
      }
    }

    /*
     * We need to disable the parent to the quest if the quest has already been accepted
     */

    foreach (LocationInfo child in node.children) {
      if (child.nxtRes.Length > 0) {
        foreach (ResInfo res in child.nxtRes) {
          if (res.type == ResInfo.ResType.Quest) {
            if (returnValidQ (res, BaseSaver.getGame()).Length < 1){
              node.visible = false;
            }
          }
        }
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
        case ResInfo.ResType.Quest:
          addQuest(inf, tGame);
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
    List<CharacterInfo.attributeType> attribs = new List<CharacterInfo.attributeType> (gameI.attributes);

    String[] names = Enum.GetNames (typeof(CharacterInfo.attributeType));
    //Find the enum that matches the name here
    for (int i = 0; i < names.Length; i++) {
      if (names[i].Equals(resI.name)){
        Debug.Log ("Adding Attribute: " + resI.name);
        attribs.Add ((CharacterInfo.attributeType)Enum.GetValues(typeof(CharacterInfo.attributeType)).GetValue(i));
      }
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

  QuestInfo fillQuestInfo(QuestInfo quest){
    QHexBoard qBoard = new QHexBoard(BaseSaver.getBoardInfo().width, BaseSaver.getBoardInfo().height, HexCellPrefab);
    HexCell[] cells = qBoard.getCells();

    TileInfo[] tiles = BaseSaver.getTiles ();
    UnitInfo[] units = BaseSaver.getUnits ();

    List<int> dests = new List<int> ();
    for (int i = 0; i < units.Length; i++) {
      if (units[i].human) {
        quest.startIdx = i;
      }
    }
    Debug.Log ("Starting idx: " + quest.startIdx.ToString());
    for (int i = 0; i < tiles.Length; i++) {
      if (tiles[i].type == quest.locType && !tiles[i].interaction) {
        HexCell[] path = HexAI.aStar (cells,cells[quest.startIdx],cells[i]);
        if(path != null && path.Length < 15){
          dests.Add (i);
        }
//        if(path != null && path.Length > 3 && path.Length < 8){
//          dests.Add (i);
//        }
      }
    }
      
    if (dests.Count > 0) {
      int[] theseDests = dests.ToArray ();
      HexUtilities.ShuffleArray (theseDests);

      tiles [theseDests [0]].interaction = true;
      quest.endIdx = theseDests [0];
      Debug.Log ("Destination Set: " + quest.endIdx.ToString ());
      BaseSaver.setTiles (tiles);
      Debug.Log ("Tiles saved");
    } else {
      Debug.Log ("No destinations! Quest invalid...");
    }

    return quest;
  }

  QuestInfo[] returnValidQ (ResInfo resI, GameInfo gameI) {
    
    List<string> playerQuestTitles = new List<String> ();
    QuestInfo.QuestGroup thisQ = QuestInfo.QuestGroup.None;
    String[] names = Enum.GetNames (typeof(QuestInfo.QuestGroup));
    List<QuestInfo> validQuests = new List<QuestInfo> ();

    foreach (QuestInfo quest in gameI.quests) {
      playerQuestTitles.Add (quest.title);
    }
    for (int i = 0; i < names.Length; i++) {
      if (names[i].Equals(resI.name)){
        thisQ = (QuestInfo.QuestGroup)Enum.GetValues (typeof(QuestInfo.QuestGroup)).GetValue (i);
        break;
      }
    }
    if (thisQ != QuestInfo.QuestGroup.None) {
      foreach (QuestInfo quest in availableQuests) {
        if (quest.questGroup == thisQ && !playerQuestTitles.Contains (quest.title)) {
          validQuests.Add (quest);
        }
      }
    }
    return validQuests.ToArray ();
  }

  void addQuest(ResInfo resI, GameInfo gameI){

    List<QuestInfo> playerQuests = new List<QuestInfo> (gameI.quests);
    QuestInfo[] validQs = returnValidQ(resI, gameI);

    if (validQs.Length > 0) {
      HexUtilities.ShuffleArray (validQs);

      QuestInfo thisQuest = JsonUtility.FromJson<QuestInfo> (JsonUtility.ToJson (validQs [0]));

      Debug.Log ("Adding Quest: " + thisQuest.title);

      playerQuests.Add (fillQuestInfo (thisQuest));
      gameI.quests = playerQuests.ToArray ();
    } else {
      Debug.Log ("No more valid quests to choose from");
    }


//    Debug.Log ("Quest: " + resI.name);
//    List<string> playerQuestTitles = new List<String> ();
//    foreach (QuestInfo quest in gameI.quests) {
//      playerQuestTitles.Add (quest.title);
//    }
//
//    List<QuestInfo> playerQuests = new List<QuestInfo> (gameI.quests);
//    List<QuestInfo> validQuests = new List<QuestInfo> ();
//    QuestInfo.QuestGroup thisQ = QuestInfo.QuestGroup.None;
//    String[] names = Enum.GetNames (typeof(QuestInfo.QuestGroup));
//
//    for (int i = 0; i < names.Length; i++) {
//      if (names[i].Equals(resI.name)){
//        thisQ = (QuestInfo.QuestGroup)Enum.GetValues (typeof(QuestInfo.QuestGroup)).GetValue (i);
//        Debug.Log ("Found Quest Type: " + names[i]);
//      }
//    }
//
//    if (thisQ != QuestInfo.QuestGroup.None) {
//      foreach(QuestInfo quest in availableQuests){
//        if (quest.questGroup == thisQ && !playerQuestTitles.Contains(quest.title)) {
//          validQuests.Add (quest);
//        }
//      }
//      if (validQuests.Count > 0) {
//        QuestInfo[] qArr = validQuests.ToArray ();
//        HexUtilities.ShuffleArray (qArr);
//
//        QuestInfo thisQuest = JsonUtility.FromJson<QuestInfo> (JsonUtility.ToJson (qArr [0]));
//
//        Debug.Log ("Adding Quest: " + thisQuest.title);
//
//        playerQuests.Add (fillQuestInfo (thisQuest));
//        gameI.quests = playerQuests.ToArray ();
//      } else {
//        Debug.Log ("No more valid quests to choose from");
//      }
//    }
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

  class QHexBoard{
    int height, width;
    HexCell prefab;
    HexCell[] cells;

    public QHexBoard(int height, int width, HexCell prefab){
      this.height = height;
      this.width = width;
      this.prefab = prefab;
      cells = new HexCell[height * width];
      for (int z = 0, i = 0; z < height; z++) {
        for (int x = 0; x < width; x++) {
          CreateCell(x, z, i++);
        }
      }
    }

    public HexCell[] getCells(){
      return cells;
    }

    private void CreateCell (int x, int z, int i) {
      Vector3 position;
      position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
      position.y = 0f;
      position.z = z * (HexMetrics.outerRadius * 1.5f);

      HexCell cell = cells[i] = Instantiate<HexCell>(prefab);
      cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
      if (x > 0) {
        cell.SetNeighbor(HexDirection.W, cells[i - 1]);
      }
      if (z > 0) {
        if ((z & 1) == 0) {
          cell.SetNeighbor(HexDirection.SE, cells[i - width]);
          if (x > 0) {
            cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
          }
        }
        else {
          cell.SetNeighbor(HexDirection.SW, cells[i - width]);
          if (x < width - 1) {
            cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
          }
        }
      }
    }
  }
}
