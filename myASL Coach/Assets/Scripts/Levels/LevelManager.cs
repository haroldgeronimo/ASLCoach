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

	public LevelPool[] difficultyPools;

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

	public Sprite thumbsUp;
	public Sprite confetti;

	public Image emojiImg;

	public GameObject failedPanel;

	public GameObject constructPanel;

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
		levelPool = difficultyPools[PlayerManager.singleton.gameManager.currentDifficulty];
		PrepareLevel (PlayerManager.singleton.gameManager.currentLevel);
	}
	// Update is called once per frame

	public void PrepareLevel (int lvlIndex) {
		if(levelPool == null){
			constructPanel.SetActive(true);return;
		}
		if(levelPool.levels.Length == 0){
			constructPanel.SetActive(true);return;

		}
		//TODO check here if level is valid
		if (lvlIndex > levelPool.levels.Length - 1 || lvlIndex < 0) {
			Debug.LogError ("Level index out of range!");
			return;
		}

		levelIndex = lvlIndex;

		currentLevel = levelPool.levels[lvlIndex];

		currentScore = 0;
		scoreTxt.text = currentScore.ToString ();

		levelNoTxt.text = currentLevel.isCheckpoint? "Checkpoint ": "Level " + currentLevel.levelNumber;

		levelNameTxt.text = levelPool.levels[lvlIndex].levelName;

		levelPanel.SetActive (true);
		if (currentLevel.introductoryAnswers.Length > 0) {
			levelPanel.SetActive (false);
			
			lrnManager.StartLearn (currentLevel.introductoryAnswers);
			return;
		}
		

		questionManager.StartQuestions (currentLevel);

	}
	public void EndLevel () {
		PlayerManager.singleton.AddPoints (currentScore);
		if(currentLevel.isCheckpoint)
		PlayerManager.singleton.gameManager.SaveProgress(levelIndex,levelPool.isLast(levelIndex));
		levelComplete.SetActive (true);
		emojiImg.sprite = thumbsUp;
		levelCompleteDescription.text = "You have passed this level";
		
        AudioManager.instance.SfxManager.PlaySfx(SfxType.success);

	}
	public void NextLevel () {
		levelComplete.SetActive (false);

		levelIndex++;
		if (levelIndex < levelPool.levels.Length) {

			PrepareLevel (levelIndex);

		} else if (levelIndex == levelPool.levels.Length) {
			Debug.Log ("Congratulation stage");
			emojiImg.sprite = confetti;
			levelComplete.SetActive (true);
			levelCompleteDescription.text = "You have finished " + levelPool.difficultyType.ToString () + " stage.";

        AudioManager.instance.SfxManager.PlaySfx(SfxType.success);
		} else {
			Debug.Log ("End of Level Pool");
			sceneControl.ChangeScene (0);
		}
	}

	public void RestartLevel(){
		failedPanel.SetActive(false);
		GetComponent<MultipleChoiceQuestion>().wrongCounter = 0;
	
		PrepareLevel (levelIndex);
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
		levelPanel.SetActive (true);
		questionManager.StartQuestions (currentLevel);
	}

}