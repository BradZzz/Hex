using System;

[Serializable]
public class ResInfo {
  public string name;
  public int value;
  public ResType type;

  public enum ResType {
    Resource, Object, Upgrade, Quest, Battle, Unit, None
  }
}
