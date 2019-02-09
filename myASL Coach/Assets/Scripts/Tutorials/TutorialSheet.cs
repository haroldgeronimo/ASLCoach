using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable ()]
public class TutorialSheet {
	public string topic;
	[TextArea(4,5)]
	public string description;
	public Sprite img;
}