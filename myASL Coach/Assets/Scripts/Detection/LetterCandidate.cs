using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LetterCandidate {
	public string name;
	public int numberOfFingers;
	public Candidate[] letters;

}

[System.Serializable]
public class Candidate {
	public char letter;
	[Range (.1f, 1)]
	public float probability = 1;
}