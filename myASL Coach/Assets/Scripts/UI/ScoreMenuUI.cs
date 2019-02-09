using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreMenuUI : MonoBehaviour {
	int score = 0;
	TextMeshProUGUI scoreTxt;
	// Use this for initialization
	void Start () {
		scoreTxt = GetComponent<TextMeshProUGUI> ();
	}

	void Update () {

		score = PlayerManager.singleton.points;
		scoreTxt.text = score.ToString ();
	}

}