using System;
using UnityEngine;

[Serializable]
public class ChoiceInfo {
	public string name;
	public string image;
	[TextArea(3, 3)]
	public string openingGreeting;
	[TextArea(3, 3)]
	public string winningGreeting;
	[TextArea(3, 3)]
	public string losingGreeting;
	public OptionInfo[] options;
}
