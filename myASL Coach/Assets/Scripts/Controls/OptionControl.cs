using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionControl : MonoBehaviour {
	PlayerManager PM;
	GameManager GM;

	int developerModeCounter = 0;
	bool isDev = false;
	public int developerRequiredCount = 5;
	Coroutine devTimeRoutine;
	void Start () {
		PM = PlayerManager.singleton;
		GM = PM.gameManager;
	}

	public void ResetGame () {
		MsgBox.ShowMsgYesNo (
			"This will delete all your progress. Are you sure you want to continue?",
			delegate {
				GM.ResetGame ();
				isDev = false;
			}
		);

	}
	public void incrementCounter () {
		if (isDev) return;
		if (devTimeRoutine == null)
			devTimeRoutine = StartCoroutine (DeveloperTimer ());
		developerModeCounter++;
	}
	IEnumerator DeveloperTimer () {
		yield return new WaitForSeconds (2);
		if (developerModeCounter >= developerRequiredCount)
			EnableDevMode ();
		developerModeCounter = 0;
		devTimeRoutine = null;
	}

	public void EnableDevMode () {
		isDev = true;
		GM.currentDifficulty = 2;
		MsgBox.ShowMsgOkay (
			"Congratulations! You are now a developer. Sub2Pewds!"
		);

	}

	public void ShowCredits(){
			MsgBox.ShowMsgOkay (
			"Created and Developed by our team. \n \nThis game is for learning the American Sign Language."
		);
	}
}