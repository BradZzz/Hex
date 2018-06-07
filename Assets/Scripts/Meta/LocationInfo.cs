using System;
using UnityEngine;

/*
 * This can't be serializable bc it contains recursive references. C# does not like that
 */
public class LocationInfo : MonoBehaviour {
  public LocationInfo[] children;
  public string name;
  public string header;
  [TextArea(3, 3)]
  public string description;
  public LocType img;
  public string nxtScene;
  public ResInfo[] nxtRes;
  public ResInfo[] needRes;

  public enum LocType {
    Market, Inn, Castle, Estate, Arena, Char, Main, None
  }
}
