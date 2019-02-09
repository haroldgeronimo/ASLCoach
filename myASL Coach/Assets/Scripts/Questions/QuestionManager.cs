using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionManager : MonoBehaviour {
	public Level currentLevel;
	LevelManager levelManager;
	MultipleChoiceQuestion multipleChoiceQuestion;
	DetectionQuestion detectionQuestion;
	//UI
	public TextMeshProUGUI scoreNotif;
	public Animator scoreAnimator;
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
		//UI
		scoreNotif.color = new Color(0,1,0,1);
		scoreNotif.text = "+" + addedScore.ToString();
		scoreAnimator.SetTrigger("Score");
	}
	public void PlayerWrong(){
		int addedScore = -currentLevel.pointsEach;
		levelManager.AddToScore (addedScore);
		//UI
		scoreNotif.color = new Color(1,0,0,1);
		scoreNotif.text = addedScore.ToString();
		scoreAnimator.SetTrigger("Score");
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