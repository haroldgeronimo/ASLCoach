using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueBoxUI : MonoBehaviour {
	[SerializeField]
	private TextMeshProUGUI messageTxt;
	[SerializeField]
	private List<DialogueTypeUI> typeUI = new List<DialogueTypeUI> ();
	private DialogueManager DM;

	void Start () {
		DM = PlayerManager.singleton.gameManager.DM;
		if (DM == null)
			Debug.LogError ("No dialogue manager found!");
	}
	public void Initialize (string message, DialogueType type) {
		messageTxt.text = message;
		if (typeUI.Count > 0)
			foreach (DialogueTypeUI tUI in typeUI) {
				if (tUI.type == type) {
					tUI.UI.SetActive (true);
					break;
				}
			}
	}

	public void Remove () {
		StartCoroutine(AnimateClose());
	}
	IEnumerator AnimateClose () {
		GetComponent<Animator>().SetTrigger("Close");
		yield return new WaitForSeconds (.4f);
		Destroy (this.gameObject);
	}

	public void SendResponse (DialogueResult result) {
		DM.AcceptResponse (result, gameObject);
	}
}