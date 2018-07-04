using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationMain : MonoBehaviour {
  public LocationInfo info;
  public AreaType area;

  public enum AreaType {
    Castle, Village, Bazaar, None
  }
}
