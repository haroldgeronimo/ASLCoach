using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public int currentDifficulty = 0;
	public int currentLevel = 0;
	public LevelPool[] levelPool;
	public PlayerManager playerManager;
	public SaveManager saveManager;
	public DialogueManager DM;

	private void Awake () {
		playerManager = GetComponent<PlayerManager> ();
		saveManager = GetComponent<SaveManager> ();
		DM = GetComponent<DialogueManager> ();
		LoadGame ();
	}
	void ShowMessage () {
		MsgBox.ShowMsgOkay ("Subscribe to PewDiePie!");
	}
	void ShowQuestion () {
		MsgBox.ShowMsgYesNo ("Do you want to show message?",
			delegate { ShowMessage (); },
			delegate { ShowQuestion (); });
	}
	void Start () {
		ShowQuestion ();
	}
	// Update is called once per frame
	void Update () {

	}
	public void SaveGame () {

		PlayerData pd = new PlayerData ();
		pd.points = playerManager.points;
		pd.currentDifficulty = currentDifficulty;
		pd.currentLevel = currentLevel;

		PlayerData pdOrig = saveManager.LoadGame (saveManager.dataPath);
		if (pdOrig != null) {
			if (pdOrig.currentDifficulty > pd.currentDifficulty) {
				pd = pdOrig;
				pd.points = playerManager.points;
				saveManager.SaveGame (pd, saveManager.dataPath);
				return;
			}
		}
		saveManager.SaveGame (pd, saveManager.dataPath);

	}
	void LoadGame () {
		PlayerData pd = saveManager.LoadGame (saveManager.dataPath);
		if (pd == null) return;
		playerManager.points = pd.points;
		currentDifficulty = pd.currentDifficulty;
		currentLevel = pd.currentLevel;
	}

	public void SaveProgress (int crtLevel, bool isEndOfDifficulty) {
		if (!isEndOfDifficulty) {
			currentLevel = crtLevel;
		} else {
			currentLevel = 0;
			currentDifficulty++;
		}

		SaveGame ();
	}
	public void ResetGame () {
		currentLevel = 0;
		currentDifficulty = 0;
		playerManager.points = 0;
		SaveGame ();
	}

}