using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	//Setup variables
	public float timeAllotment = 5;
	public float timeBumper = 1;
	public float cooldownTime = 0.2f;
	public LevelPool levelPool;
	public LearningManager lrnManager;

	//UI
	public Transform[] difficultyTransforms;
	List<Button> buttons = new List<Button> ();
	public Text answerTxt;
	public Image imgAnswer;
	public Image countdownUI;
	public GameObject levelComplete;
	public TextMeshProUGUI levelNoTxt;
	public TextMeshProUGUI levelNameTxt;
	public TextMeshProUGUI scoreTxt;

	//in game variables
	float currentTime = 0;
	float currentCooldown = 0;
	float timePercent = 1;
	Level currentLevel;
	int levelIndex;
	int currentScore = 0;
	bool isPlaying = true;

	int currentQuestion = -1;

	void Start () {
		lrnManager = GetComponent<LearningManager> ();
		// TODO gawing dynamic
		PrepareLevel (0);
	}
	// Update is called once per frame
	void Update () {
		if (isPlaying)
			updateTimes ();
	}
	public void PrepareLevel (int lvlIndex) {

		//TODO check here if level is valid
		levelIndex = lvlIndex;
		for (int i = 0; i < difficultyTransforms.Length; i++) {
			difficultyTransforms[0].gameObject.SetActive (false);
		}
		// TODO gawing dynamic
		switch (levelPool.difficultyType) {
			case Difficulty.Easy:
				difficultyTransforms[0].gameObject.SetActive (true);
				LoadButtons (difficultyTransforms[0]);
				break;
			case Difficulty.Average:
				difficultyTransforms[1].gameObject.SetActive (true);
				LoadButtons (difficultyTransforms[1]);
				break;
			case Difficulty.Difficult:
				difficultyTransforms[2].gameObject.SetActive (true);
				LoadButtons (difficultyTransforms[2]);
				break;
			default:
				break;
		}
		currentLevel = levelPool.levels[lvlIndex];

		currentScore = 0;
		scoreTxt.text = currentScore.ToString ();
		levelNoTxt.text = "Level " + levelPool.levels[lvlIndex].levelNumber;
		levelNameTxt.text = levelPool.levels[lvlIndex].levelName;
		currentLevel.InitializeQuestions ();
		NextQuestion ();
		if(currentLevel.introductoryAnswers.Length > 0){
			lrnManager.StartLearn(currentLevel.introductoryAnswers);
		}

	}
	public void acceptAnswer (int i) {
		if (currentCooldown > 0)
			return;

		if (i == currentLevel.questions[currentQuestion].rightAnswerIndex) {
			// TODO Addscore
			int addedScore = currentLevel.pointsEach * (int) currentTime;
			if (addedScore == 0) addedScore = currentLevel.pointsEach;
			currentScore += addedScore;
			scoreTxt.text = currentScore.ToString ();
			NextQuestion ();
		} else {
			currentCooldown = cooldownTime;
			toggleButtons ();
			StartCoroutine (coolDown ());
		}
	}
	void updateTimes () {
		currentTime -= Time.deltaTime;

		if (currentTime < 0)
			NextQuestion ();

		timePercent = currentTime / (timeAllotment + timeBumper);
		countdownUI.fillAmount = timePercent;
		countdownUI.color = Color.HSVToRGB (timePercent / 5, 1, 1);
	}

	IEnumerator coolDown () {
		while (currentCooldown > 0) {

			currentCooldown -= 1;
			yield return new WaitForSeconds (1);
		}
		toggleButtons ();
		yield return null;
	}

	void NextQuestion () {
		Debug.Log ("Next Question");
		StopAllCoroutines ();
		isPlaying = false;
		currentQuestion++;
		currentTime = timeAllotment + timeBumper;
		if (currentQuestion >= currentLevel.questions.Length) {
			EndLevel ();
			return;
		}
		isPlaying = true;

		//prepare for question
		answerTxt.text = currentLevel.questions[currentQuestion].rightAnswer.text;
		imgAnswer.sprite = currentLevel.questions[currentQuestion].rightAnswer.img;
		Answer[] choices = currentLevel.questions[currentQuestion].choices;
		for (int i = 0; i < choices.Length; i++) {
			TextMeshProUGUI choiceText = buttons[i].transform.GetChild (0).GetComponent<TextMeshProUGUI> ();

			choiceText.text = choices[i].text;
		}
	}
	void EndLevel () {
		PlayerManager.singleton.points += currentScore;
		levelComplete.SetActive (true);
		currentQuestion = -1;
	}
	public void NextLevel () {
		levelComplete.SetActive (false);

		levelIndex++;
		if (levelPool.levels.Length > levelIndex) {

			PrepareLevel (levelIndex);
		} else {
			Debug.Log ("End of Level Pool");
			//end of level pool;
		}
	}

	public void PauseGame(){
		isPlaying = false;
	}
	
	public void PlayGame(){
		isPlaying = true;
	}

	#region UI
	void LoadButtons (Transform difficultyTrans) {

		Button btn;
		foreach (Transform btnTrans in difficultyTrans) {
			btn = btnTrans.GetComponent<Button> ();
			if (btn != null) {
				buttons.Add (btn);
			}

		}

	}

	void toggleButtons () {
		foreach (Button btn in buttons) {
			btn.interactable = !btn.interactable;
		}
	}
	#endregion

}