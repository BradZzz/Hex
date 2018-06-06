using System;

[Serializable]
public class OutcomeInfo {
  string name;
  int value;
  ResType outcome;

  public enum ResType {
    Resource, Object, Upgrade, Quest, Battle, None
  }
}
