using System;

[Serializable]
public class ResInfo {
  string name;
  int value;
  ResType type;

  public enum ResType {
    Resource, Object, Upgrade, Quest, None
  }
}
