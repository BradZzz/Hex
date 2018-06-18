using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class QuestInfo {

  public HexCoordinates startIdx;
  public HexCoordinates endIdx;

  [TextArea(1, 3)]
  public string title;
  [TextArea(3, 3)]
  public string startMsg;
  [TextArea(3, 3)]
  public string endMsg;

  public QuestType type;
  public QuestGroup questGroup;
  public TileInfo.tileType locType;
  public bool completed;
  public ResInfo[] rewards;
  public ResInfo[] challenges;

  public enum QuestType {
   Battle, Assist, Travel, None
  }

  public enum QuestGroup {
    Basic, Village, Castle, Game, Special, None
  }
}
