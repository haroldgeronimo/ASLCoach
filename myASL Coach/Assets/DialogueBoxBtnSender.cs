using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class DialogueBoxBtnSender : MonoBehaviour {
	[SerializeField]
	private DialogueBoxUI dialogueBoxUI;
	[SerializeField]
	private DialogueResult result;

	 private void Start() {
		Button btn = GetComponent<Button>();
		btn.onClick.RemoveAllListeners();
		btn.onClick.AddListener(delegate{dialogueBoxUI.SendResponse(result);});
	}

}