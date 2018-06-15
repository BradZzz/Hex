using System;
using UnityEngine;

[Serializable]
public class OptionInfo {
  [TextArea(2, 3)]
	public String[] TextOptions;
	[TextArea(3, 3)]
	public String reaction;
	public resultType result;

	public enum resultType {
		Avoid, Battle, MiniGame, Treasure, None
	}
}
