using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Answer Pool", menuName = "myASL Coach/AnswerPool", order = 0)]
public class AnswerPool : ScriptableObject {
	public string answerPoolName;
	public Answer[] answers;

}
