using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable ()]
public class Question {
	[HideInInspector()]
	public int rightAnswerIndex;
	[HideInInspector()]
	public Answer[] choices;
	public Answer rightAnswer;
	public Answer[] rightAnswerPool;
	/* 
	populateChoices(choice pool, rightanswer, int choiceCount)
	pickRandomAnswer(choice pool)
	public initialize(choice pool){}
	public initialize(choice pool, answer){}


	 */
	void populateChoices (Answer[] answers, Answer rAnswer, int choiceCount) {
		choices = new Answer[choiceCount];
		List<Answer> answerLib = new List<Answer>(answers);
		Answer ansHolder;
		rightAnswerIndex = Random.Range (0, choiceCount);//
		answerLib.Remove(rAnswer);

		for (int i = 0; i < choiceCount; i++) {
			ansHolder = rAnswer;
			if (i != rightAnswerIndex) {
				//no repeating of choice
				int choiceIndex = Random.Range (0, answerLib.Count);//
					ansHolder = answerLib[choiceIndex];
					answerLib.RemoveAt(choiceIndex);
			} else {
				rightAnswer = rAnswer;
			}

			choices[i] = ansHolder;
		}

	}
	Answer pickRandomAnswer (Answer[] answers) {
		return answers[Random.Range (0, answers.Length)];//
	}
	public void initialize (Answer[] answers, int choiceCount = 4) {
		if (rightAnswer == null && rightAnswerPool.Length <= 0) {
			Answer ans = pickRandomAnswer (answers);
			//Debug.Log (ans);
			populateChoices (answers, ans, choiceCount);
		} else if(rightAnswer != null && rightAnswerPool.Length <= 0){
			
			populateChoices (answers, rightAnswer, choiceCount);
		}
		else if(rightAnswer == null && rightAnswerPool.Length > 0){
					Answer ans = pickRandomAnswer (rightAnswerPool);
			populateChoices (answers,ans , choiceCount);
		}
		else if(rightAnswer != null && rightAnswerPool.Length > 0){
			populateChoices (answers,rightAnswer , choiceCount);
		}
	}
}