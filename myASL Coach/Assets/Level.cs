using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Level", menuName = "myASL Coach/Level", order = 0)]
public class Level : ScriptableObject {
	public int levelNumber;
	public string levelName;
	public string levelDescription;

	public int pointsEach = 5;
	public Question[] questions;
	public AnswerPool answerPool;

	public Answer[] introductoryAnswers;
	
	public bool isCheckpoint = false;
	public void InitializeQuestions () {
		for (int i = 0; i < questions.Length; i++) {
			questions[i].initialize (answerPool.answers);
		}
	}
}