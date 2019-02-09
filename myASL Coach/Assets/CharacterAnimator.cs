using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour {
	public Animator animator;
	public string lastAnimation;
	public void animateByText (string animationName) {
		animator.SetTrigger (animationName);
		lastAnimation = animationName;
	}
	public void animateByAnswer (Answer answer) {
		animator.SetTrigger (answer.text);
		lastAnimation = answer.text;
	}
	public void animateDefault () {
		animator.SetTrigger ("Default");
	}

	public void animateForced (Answer answer) {
		animator.Play (answer.text.ToUpper ());

	}
	public void animateForced (string animName) {
		animator.Play (animName);

	}
}