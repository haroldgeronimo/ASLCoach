using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceQuestion : QuestionControl {
	//Setup variables
	public float timeAllotment = 5;
	public float timeBumper = 1;
	public float cooldownTime = 0.2f;
	public int failTreshold = 3;

	//In game variables	
	float currentTime = 100;
	float currentCooldown = 0;
	float timePercent = 1;

	public int wrongCounter = 0;

	//3d
	public CharacterAnimator animator;
	public CameraController cam;

	//UI
	public Image countdownUI;
	public Transform[] difficultyTransforms;
	List<Button> buttons = new List<Button> ();
	public Text answerTxt;
	public Image imgAnswer;

	public Sprite rightImg;
	public Sprite wrongImg;
	public Image resultImg;
	public Animator resultAnimator;

	public GameObject failPanel;
	void Update () {
		if (isPlaying)
			updateTimes ();
	}

	public override void StartQuestion (Question q) {

		toggleButtonsOn ();
		base.StartQuestion (q);
		UIPrepareForQuestion (q);

		animator.animateForced (q.rightAnswer);
		cam.changeCameraPosition (q.rightAnswer);
		currentTime = timeAllotment + timeBumper;
	}

	public void RefreshAnimation(){
		StartCoroutine(refreshAnimation());
	}
	IEnumerator refreshAnimation(){
		animator.animateForced ("Default");
		yield return new WaitForSeconds(.2f);
		animator.animateForced (question.rightAnswer);
		cam.changeCameraPosition (question.rightAnswer);
	}

	public void acceptAnswer (int i) {
		if (currentCooldown > 0)
			return;

		if (i == question.rightAnswerIndex) {
			RightAnswer ();
		} else {
			WrongAnswer ();
		}
	}

	void RightAnswer () {
		wrongCounter = 0;
		questionManager.PlayerCorrect (currentTime);
		animator.animateDefault ();
		toggleButtonsOff ();
		EndQuestion ();

		resultImg.sprite = rightImg;
		resultAnimator.SetTrigger ("Result");
	}
	void WrongAnswer () {

		if (questionManager.currentLevel.isCheckpoint)
			questionManager.PlayerWrong ();
		currentCooldown = cooldownTime;
		toggleButtonsOff ();
		StartCoroutine (coolDown ());

		resultImg.sprite = wrongImg;
		resultAnimator.SetTrigger ("Result");
		wrongCounter++;

	}

	void updateTimes () {
		currentTime -= Time.deltaTime;

		if (currentTime < 0) {
			if (questionManager.currentLevel.isCheckpoint)
				questionManager.PlayerWrong ();

			wrongCounter++;
			if (wrongCounter >= failTreshold) {
				failPanel.SetActive (true);
				animator.animateDefault ();
				for (int i = 0; i < difficultyTransforms.Length; i++) {
					difficultyTransforms[0].gameObject.SetActive (false);
				}
				isPlaying = false;
				return;
			}
			EndQuestion ();
		}

		timePercent = currentTime / (timeAllotment + timeBumper);
		ChangeTimerUI (timePercent);
	}

	IEnumerator coolDown () {
		while (currentCooldown > 0) {

			currentCooldown -= 1;
			yield return new WaitForSeconds (1);
		}
		toggleButtonsOn ();
		yield return null;
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

	public void ChangeTimerUI (float timerPercent) {
		countdownUI.fillAmount = timerPercent;
		countdownUI.color = Color.HSVToRGB (timerPercent / 5, 1, 1);
	}

	public void toggleButtonsOn () {
		foreach (Button btn in buttons) {
			btn.interactable = true;
		}
	}
	public void toggleButtonsOff () {
		foreach (Button btn in buttons) {
			btn.interactable = false;
		}
	}

	public void UIPrepareForQuestion (Question currentQuestion) {

		answerTxt.text = currentQuestion.rightAnswer.text;
		imgAnswer.sprite = currentQuestion.rightAnswer.img;
		Answer[] choices = currentQuestion.choices;

		for (int i = 0; i < difficultyTransforms.Length; i++) {
			difficultyTransforms[0].gameObject.SetActive (false);
		}
		// TODO gawing dynamic
		switch (levelManager.levelPool.difficultyType) {
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

		for (int i = 0; i < choices.Length; i++) {
			TextMeshProUGUI choiceText = buttons[i].transform.GetChild (0).GetComponent<TextMeshProUGUI> ();

			choiceText.text = choices[i].text;
		}
	}
	#endregion
}

/*
		} */