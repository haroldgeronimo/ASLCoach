using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Level", menuName = "myASL Coach/Level", order = 0)]
public class Level : ScriptableObject {
	public int levelNumber;
	public string levelName;
	public string levelDescription;

	public int pointsEach = 5;
	public Question[] questionsPrimitive;

	[HideInInspector]
	public Question[] questions;
	public AnswerPool answerPool;

	public Answer[] introductoryAnswers;

	public bool isCheckpoint = false;
	public void InitializeQuestions () {
		questions = cloneQuestion(questionsPrimitive);
		for (int i = 0; i < questions.Length; i++) {
			questions[i].initialize (answerPool.answers);
		}
	}

	public Question[] cloneQuestion (Question[] questionToClone) {
		Question[] newQuestion = new Question[questionToClone.Length];
		for (int i = 0; i < questionToClone.Length; i++) {
			Question q = new Question ();
			q.rightAnswerIndex = questionToClone[i].rightAnswerIndex;
			q.choices = questionToClone[i].choices;
			q.rightAnswer = questionToClone[i].rightAnswer;
			q.rightAnswerPool = questionToClone[i].rightAnswerPool;
			q.questionType = questionToClone[i].questionType;
			newQuestion[i] = q;
		}
		return newQuestion;
	}
}