using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionControl : MonoBehaviour {
	PlayerManager PM;
	GameManager GM;
	void Start(){
		PM = PlayerManager.singleton;
		GM = PM.gameManager;
	}

	public void ResetGame(){
		GM.ResetGame();
	}
}
