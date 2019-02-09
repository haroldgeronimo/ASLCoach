using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LearningManager : MonoBehaviour {
	public Answer[] answersToLearn;
	public int currentLearnIndex = 0;
	[HideInInspector]
	public LevelManager lvlManager;

	//UI

	public GameObject lrnPanel;
	public GameObject firstBtn, nthBtn, lastBtn;

	public TextMeshProUGUI txtType;
	public TextMeshProUGUI txtAnswerText;
	public Image imgAnswer;

	//3d
	public CharacterAnimator animator;
	public CameraController cam;
	void Awake () {
		lvlManager = GetComponent<LevelManager> ();
	}
	public void StartLearn (Answer[] ansToLrn) {
		
		animator.animateForced ("Default");
		lvlManager.PauseGame ();
		PrepareLearn (ansToLrn);
	}
	void PrepareLearn (Answer[] ansToLrn) {

		lrnPanel.SetActive (true);
		currentLearnIndex = 0;
		answersToLearn = ansToLrn;
		InitializeView (answersToLearn[currentLearnIndex]);
	}
	void EndLearn () {
		animator.animateForced ("Default");
		lrnPanel.SetActive (false);
		currentLearnIndex = 0;
		lvlManager.PlayGame ();
		//Playgame
	}
	public void NextLearn () {
		currentLearnIndex++;
		if (currentLearnIndex >= answersToLearn.Length) {
			Debug.Log ("Play Level");
			EndLearn ();
			return;
		}
		InitializeView (answersToLearn[currentLearnIndex]);

	}
	public void PrevLearn () {
		currentLearnIndex--;
		if (currentLearnIndex < 0) {
			Debug.LogError ("Learn out of index");
			return;
		}
		InitializeView (answersToLearn[currentLearnIndex]);

	}

	void InitializeView (Answer ans) {
		LoadUIBtns ();
		LoadUIInformation (ans);
		Load3DView (ans);
	}

	void Load3DView (Answer answer) {
		animator.animateForced (answer);
		cam.changeCameraPosition (answer);
	}
	#region UI 
	void LoadUIBtns () {
		firstBtn.SetActive (false);
		nthBtn.SetActive (false);
		lastBtn.SetActive (false);
		Debug.Log (currentLearnIndex);
		if (currentLearnIndex == 0) {

			firstBtn.SetActive (true);
			Debug.Log ("First to be learnt");
		} else if (currentLearnIndex == answersToLearn.Length - 1) {

			lastBtn.SetActive (true);
			Debug.Log ("Last to be learnt");
		} else {

			nthBtn.SetActive (true);
			Debug.Log ((currentLearnIndex + 1) + " to be learnt");
		}
	}

	void LoadUIInformation (Answer ans) {
		txtType.text = "New " + ans.type.ToString () + "!";
		txtAnswerText.text = ans.text.ToString ();
		imgAnswer.sprite = ans.img;
	}
	#endregion
}