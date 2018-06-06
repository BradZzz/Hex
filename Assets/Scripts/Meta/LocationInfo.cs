using System;
using UnityEngine;

[Serializable]
public class LocationInfo {
  public LocationInfo(LocationInfo loc){
    name = loc.name;
    description = loc.description;
    img = loc.img;
    nxtScene = loc.nxtScene;
    nxtRes = loc.nxtRes;
    needRes = loc.needRes;
  }

  public LocationInfo[] children;
  public string name;
  [TextArea(3, 3)]
  public string description;
  public LocType img;
  public string nxtScene;
  public ResInfo[] nxtRes;
  public ResInfo[] needRes;

  public enum LocType {
    Market, Inn, Castle, Arena, None
  }
}
