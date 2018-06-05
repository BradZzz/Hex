using System;
using UnityEngine;

[Serializable]
public class LocationInfo {
  public LocationInfo[] parent;
  public LocationInfo[] children;
  public string name;
  [TextArea(3, 3)]
  public string description;
  public LocType img;
  public string nxtScene;
  public ResInfo[] nxtRes;
  public ResInfo[] needRes;

  public enum LocType {
    Market, Inn, Castle, Shop, Arena, None
  }
}
