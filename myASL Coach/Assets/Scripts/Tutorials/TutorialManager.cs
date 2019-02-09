using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
	//Setup Variables
	[HideInInspector]
	public Tutorial loadedTutorial;
	[HideInInspector]
	public int currentIndex = -1;

	//UI Variables
	public TextMeshProUGUI tutorialTitle;
	public GameObject tutorialContainer;

	//UI with Image
	public TextMeshProUGUI tutTopic;
	public TextMeshProUGUI tutDescription;
	public Image tutImage;
	public GameObject tutContainer;

	//UI with No Image
	public TextMeshProUGUI tutTopicNoImg;
	public TextMeshProUGUI tutDescriptionNoImg;
	public GameObject tutContainerNoImg;

	public void StartTutorial (Tutorial tutorialToLoad) {
		loadedTutorial = null;
		loadedTutorial = tutorialToLoad;
		if (loadedTutorial == null) {
			Debug.LogError ("No Tutorial found!");
			return;
		}
		tutorialContainer.SetActive (true);
		tutorialTitle.text = loadedTutorial.tutorialName;
		NextTutorial ();
	}
	public void EndTutorial () {
		ClearTutorialUI ();
		tutorialContainer.SetActive (false);
		currentIndex = -1;
		loadedTutorial = null;
	}
	public void NextTutorial () {
		currentIndex++;
		if (currentIndex >= loadedTutorial.sheets.Count) {
			EndTutorial ();
			return;
		}
		TutorialSheet currentTut = loadedTutorial.sheets[currentIndex];
		LoadCurrentTutorialSheet (currentTut.topic, currentTut.description, currentTut.img);

	}
	#region UI
	void ClearTutorialUI () {
		tutContainer.SetActive (false);
		tutContainerNoImg.SetActive (false);
	}
	//Tutorial with image
	void LoadCurrentTutorialSheet (string topic, string description, Sprite img) {
		ClearTutorialUI ();
		if (img == null) {
			LoadCurrentTutorialSheet (topic, description);
			return;
		}

		tutContainer.SetActive (true);
		tutTopic.text = topic;
		tutDescription.text = description;
		tutImage.sprite = img;
	}
	//Tutorial with no image
	void LoadCurrentTutorialSheet (string topic, string description) {

		tutContainerNoImg.SetActive (true);
		tutTopicNoImg.text = topic;
		tutDescriptionNoImg.text = description;
	}
	#endregion
}