using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class QuestInfo {
  [HideInInspector]
  public HexCoordinates startIdx;
  [HideInInspector]
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
  public ChoiceInfo confrontation;
  [HideInInspector]
  public bool completed;
  [HideInInspector]
  public bool placed;
  [HideInInspector]
  public bool rewardAtNext;
  public ResInfo[] rewards;
  public ResInfo[] challenges;
  [HideInInspector]
  public DistanceType distance;

  public enum DistanceType {
    Cold, Warmer, Burning, None
  }

  public enum QuestType {
   Battle, Assist, Travel, None
  }

  public enum QuestGroup {
    Basic, Village, Castle, Game, Special, None
  }
}
