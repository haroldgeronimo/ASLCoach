using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	public static PlayerManager singleton;
	public GameManager gameManager;
	//Player data
	public int points = 0;

	void Awake () {
		if(singleton != null){
		Destroy(this.gameObject);
		}
		else
		{
		singleton = this;
		DontDestroyOnLoad(this.gameObject);
		}

		gameManager = GetComponent<GameManager>();
	}

	public void AddPoints(int pointsToAdd){
		points += pointsToAdd;
		gameManager.SaveGame();
	}
}