using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "myASL Coach/Tutorial", order = 0)]
public class Tutorial : ScriptableObject {
	public string tutorialName;
	public List<TutorialSheet> sheets = new List<TutorialSheet>();
}