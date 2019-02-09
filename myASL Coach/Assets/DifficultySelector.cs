using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelector : MonoBehaviour {
	GameManager GM;
	Button btn;
	public int difficultyIndex;
	void Start () {
		GM = PlayerManager.singleton.gameManager;
		if (GM == null) {
			Debug.LogError ("Game Manager Not Found!");
			return;
		}
		btn = GetComponent<Button> ();

		if(difficultyIndex <= GM.currentDifficulty)
		btn.interactable = true;
		else btn.interactable = false;
	}

}