using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreMenuUI : MonoBehaviour {
	int score = 0;
	// Use this for initialization
	void Start () {
	score = 	PlayerManager.singleton.points;
	GetComponent<TextMeshProUGUI>().text = score.ToString();
	}
	
	
}
