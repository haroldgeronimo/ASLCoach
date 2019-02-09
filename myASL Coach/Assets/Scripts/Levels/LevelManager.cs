using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	//Setup variables
	[HideInInspector]
	public QuestionManager questionManager;
	public SceneControlConnection sceneControl;


	public LevelPool levelPool;
	[HideInInspector]
	public LearningManager lrnManager;

	//UI
	public GameObject levelPanel;
	public GameObject levelComplete;
	public TextMeshProUGUI levelCompleteDescription;
	public TextMeshProUGUI levelNoTxt;
	public TextMeshProUGUI levelNameTxt;
	public TextMeshProUGUI scoreTxt;

	//in game variables
	Level currentLevel;
	int levelIndex;
	int currentScore = 0;
	void Awake () {

		lrnManager = GetComponent<LearningManager> ();
		questionManager = GetComponent<QuestionManager> ();
	}
	void Start () {
		// TODO gawing dynamic
		PrepareLevel (0);
	}
	// Update is called once per frame

	public void PrepareLevel (int lvlIndex) {

		//TODO check here if level is valid
		if (lvlIndex > levelPool.levels.Length - 1 || lvlIndex < 0) {
			Debug.LogError ("Level index out of range!");
			return;
		}


		levelIndex = lvlIndex;

		currentLevel = levelPool.levels[lvlIndex];

		currentScore = 0;
		scoreTxt.text = currentScore.ToString ();
		
		levelNoTxt.text = currentLevel.isCheckpoint?"Checkpoint ":"Level " + currentLevel.levelNumber;

		levelNameTxt.text = levelPool.levels[lvlIndex].levelName;

		levelPanel.SetActive(true);
		if (currentLevel.introductoryAnswers.Length > 0) {
		levelPanel.SetActive(false);

			lrnManager.StartLearn (currentLevel.introductoryAnswers);
			return;
		}

		questionManager.StartQuestions (currentLevel);

	}
	public void EndLevel () {
		PlayerManager.singleton.AddPoints (currentScore);
		levelComplete.SetActive (true);
		levelCompleteDescription.text = "You have passed this level";

	}
	public void NextLevel () {
		levelComplete.SetActive (false);

		levelIndex++;
		if (levelIndex < levelPool.levels.Length) {

			PrepareLevel (levelIndex);

		} else if ( levelIndex == levelPool.levels.Length) {
			Debug.Log ("Congratulation stage");

			levelComplete.SetActive (true);
			levelCompleteDescription.text = "You have finished " + levelPool.difficultyType.ToString() + " stage.";

		} else {
			Debug.Log ("End of Level Pool");
			sceneControl.ChangeScene(0);
		}
	}

	public void AddToScore (int scoreAdded) {
		currentScore += scoreAdded;
		scoreTxt.text = currentScore.ToString ();
	}
	public void PauseGame () {
		// TODO
	}

	public void PlayGame () {
		// TODO
		levelPanel.SetActive(true);
		questionManager.StartQuestions (currentLevel);
	}

}