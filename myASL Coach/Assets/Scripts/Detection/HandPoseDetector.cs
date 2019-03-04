using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity;
using OpenCVModule;
using UnityEngine;
using UnityEngine.UI;

public class HandPoseDetector : MonoBehaviour {
	public HandPoseAlphabetEstimation HPA;
	public Text debugText;

	public char CharacterDetection () {
		if (HPA != null) {
			if (HPA.letter != null)
				return HPA.letter[0];
			else
				return '~';
		} else {
			return '!';
		}
	}
	//WHAT THE FREAK AYOKO NA

	public void DebugWriteLn (string s) {
		debugText.text += "\n" + s;
	}
	public void DebugWrite (string s) {
		debugText.text += s;
	}
	public void DebugClear () {
		debugText.text = string.Empty;
	}
}