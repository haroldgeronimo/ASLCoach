using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class DetectionQuestion : QuestionControl {
	char[] answerChars;
	public int detectionPerSecond = 1;
	int currentIndex = 0;
	string alphabet = "abcdefghijklmnopqrstuvwxyz";
	char detectedCharacter;
	HandPoseDetector HPD;
	Coroutine detectionCoroutine;

	//UI
	public TextMeshProUGUI rightAnswerTxt;
	public TextMeshProUGUI userAnswerTxt;
	public TextMeshProUGUI userCurrentAnswerTxt;

	private void Start () {
		HPD = GetComponent<HandPoseDetector> ();
		if (HPD == null)
			Debug.LogError ("Hand Pose detector cannot be found");
	}

	public override void StartQuestion (Question q) {
		base.StartQuestion (q);

		currentIndex = 0;
		if (q.rightAnswer.text.Length == 0) {
			Debug.LogError ("Answer not set!");
		}

		rightAnswerTxt.text = q.rightAnswer.text;
		userCurrentAnswerTxt.text = string.Empty;
		userAnswerTxt.text = string.Empty;
		answerChars = q.rightAnswer.text.ToUpper ().Replace (" ", "").ToCharArray ();
		detectionCoroutine = StartCoroutine (Detect ());

		HPD.HPA.webCamTextureToMatHelper.Init ();

	}
	public void SkipQuestion () {
		EndQuestion ();
	}

	void acceptAnswer (char c) {

		if (c == answerChars[currentIndex]) {
			//right answer
			userCurrentAnswerTxt.text = c.ToString ();
			userAnswerTxt.text = userAnswerTxt.text + c.ToString ();
			currentIndex++;

			AudioManager.instance.SfxManager.PlaySfx (SfxType.alert);
		}

		if (currentIndex > answerChars.Length - 1) {

			questionManager.PlayerCorrect (answerChars.Length);
			EndQuestion ();

			AudioManager.instance.SfxManager.PlaySfx (SfxType.correct);
		}
	}

	public override void EndQuestion () {
		base.EndQuestion ();

		if (detectionCoroutine != null)
			StopCoroutine (detectionCoroutine);
		HPD.HPA.webCamTextureToMatHelper.Dispose ();
	}

	IEnumerator Detect () {

		while (true) {
			Debug.Log ("Detection init");
			if (HPD == null) {

				yield return new WaitForSeconds (1 / detectionPerSecond);
				continue;
			}
			char detectedCharacter = HPD.CharacterDetection ();

			//userCurrentAnswerTxt.text = detectedCharacter.ToString ();
			acceptAnswer (detectedCharacter);
			yield return new WaitForSeconds (1 / detectionPerSecond);
		}
		yield return null;
	}

}