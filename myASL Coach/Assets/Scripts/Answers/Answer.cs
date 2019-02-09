using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Answer", menuName = "myASL Coach/Answer")]
public class Answer : ScriptableObject {
	public string text;
	public Sprite img;
	public AnswerType type = AnswerType.Letter;

	//array of animations
}
