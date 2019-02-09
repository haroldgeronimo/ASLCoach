using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour {
	public Level currentLevel;
	LevelManager levelManager;
	MultipleChoiceQuestion multipleChoiceQuestion;
	DetectionQuestion detectionQuestion;

	int currentQuestion = -1;
	void Awake () {
		levelManager = GetComponent<LevelManager> ();
		multipleChoiceQuestion = GetComponent<MultipleChoiceQuestion> ();
		detectionQuestion = GetComponent<DetectionQuestion> ();
	}

	public void StartQuestions (Level lvl) {
		if (lvl == null) { Debug.LogError ("No level sent from Level Manager!"); return; }
		currentLevel = lvl;
		Debug.Log("Level " + lvl);
		currentLevel.InitializeQuestions ();
		
		Debug.Log("NEXT QUESTION");
		NextQuestion ();
	}

	public void PlayerCorrect (float pointsMultiplier) {
		int addedScore = currentLevel.pointsEach * (int) pointsMultiplier;
		if (addedScore == 0) addedScore = currentLevel.pointsEach;
		levelManager.AddToScore (addedScore);
	}
	public void PlayerWrong(){
		int addedScore = -currentLevel.pointsEach;
		levelManager.AddToScore (addedScore);
	}

	public void NextQuestion () {
		Debug.Log ("Next Question");
		if(currentLevel == null)
		{
			Debug.LogError("No set level");
			return;
		}
		currentQuestion++;
		Debug.Log("Current Question:" + currentQuestion);
		if (currentQuestion >= currentLevel.questions.Length) {
			currentQuestion = -1;
			levelManager.EndLevel ();
			return;
		}
		if (currentLevel.questions[currentQuestion].questionType == QuestionType.MultipleChoice)
			multipleChoiceQuestion.StartQuestion (currentLevel.questions[currentQuestion]);
		else if (currentLevel.questions[currentQuestion].questionType == QuestionType.Detection)
			detectionQuestion.StartQuestion (currentLevel.questions[currentQuestion]);

	}

}