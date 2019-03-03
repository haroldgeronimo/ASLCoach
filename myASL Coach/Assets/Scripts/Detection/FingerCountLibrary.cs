using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName = "FingerCountLibrary", menuName = "Hand Detection Test/FingerLibrary", order = 0)]
public class FingerCountLibrary : ScriptableObject {
    string alphabet = "QWERTYUIOPASDFGHJKLZXCVBNM";
    public List<LetterCandidate> candidates = new List<LetterCandidate> ();

    public char GuessLetter(int numberOfFingers){
        char guessedChar = alphabet[Random.Range(0,alphabet.Length)];
        LetterCandidate letterCandidate = null;
        foreach(LetterCandidate c in candidates){
            if(numberOfFingers == c.numberOfFingers)
            letterCandidate = c;
        }
        if(letterCandidate != null){
            Candidate[] letters = letterCandidate.letters;
            guessedChar = letters[Random.Range(0,letters.Length)].letter;
        }

        return guessedChar;
    }
}
