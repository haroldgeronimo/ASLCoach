using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionControl : MonoBehaviour {
	[HideInInspector]
	public QuestionManager questionManager;
	[HideInInspector]
	public Question question;
	[HideInInspector]
	public LevelManager levelManager;
	public GameObject[] UiQuestion;

	[HideInInspector]
	public bool isPlaying = false;

	void Awake () {
		questionManager = GetComponent<QuestionManager> ();
		levelManager = GetComponent<LevelManager> ();
	}
	public virtual void StartQuestion (Question q) {

		EnableUI ();
		question = q;
		isPlaying = true;
	}

	public virtual void EndQuestion () {
		StopAllCoroutines ();
		isPlaying = false;
		StartCoroutine (NextLevelBuffer ());
	}

	public void ResetUI(){
		StopAllCoroutines ();
		isPlaying = false;
		DisableUI ();
	}

	void EnableUI () {
		for (int i = 0; i < UiQuestion.Length; i++) {
			UiQuestion[i].SetActive (true);
		}
	}
	void DisableUI () {
		for (int i = 0; i < UiQuestion.Length; i++) {
			UiQuestion[i].SetActive (false);
		}
	}

	IEnumerator NextLevelBuffer () {
		yield return new WaitForSeconds (2.2f);
		DisableUI ();
		Debug.Log ("NEXT QUESTION");
		questionManager.NextQuestion ();

	}

}