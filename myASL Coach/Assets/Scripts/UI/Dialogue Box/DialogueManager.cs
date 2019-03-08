using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour {
	[SerializeField]
	private GameObject dialogueBoxPrefab;
	[SerializeField]
	Transform instantiateLocation;

	List<DialogueBox> dialogueBoxes = new List<DialogueBox> ();
	#region Show Messages
	/// <summary>
	/// Show message with Yes and No actions
	/// </summary>
	/// <param name="message">message you you want to show</param>
	/// <param name="yesAction">action performed when Yes is selected</param>
	/// <param name="noAction">action performed when No is selected</param>
	public void ShowMsgYesNo (
		string message,
		UnityAction yesAction = null,
		UnityAction noAction = null) {
		initializeMsgBox (message,
			DialogueType.YesNo,
			new DialogueResultAction (DialogueResult.Yes, yesAction),
			new DialogueResultAction (DialogueResult.No, noAction));
	}

	/// <summary>
	/// Show message with Yes,No, and Cancel actions
	/// </summary>
	/// <param name="message">message you you want to show</param>
	/// <param name="yesAction">action performed when Yes is selected</param>
	/// <param name="noAction">action performed when No is selected</param>
	/// <param name="cancelAction">action performed when Cancel is selected</param>
	public void ShowMsgYesNoCancel (
		string message,
		UnityAction yesAction = null,
		UnityAction noAction = null,
		UnityAction cancelAction = null) {
		initializeMsgBox (message,
			DialogueType.YesNo,
			new DialogueResultAction (DialogueResult.Yes, yesAction),
			new DialogueResultAction (DialogueResult.No, noAction),
			new DialogueResultAction (DialogueResult.Cancel, cancelAction));
	}
	public void ShowMsgOkay (
		string message,
		UnityAction okayAction = null) {
		initializeMsgBox (message,
			DialogueType.Okay,
			new DialogueResultAction (DialogueResult.Okay, okayAction));
	}
	#endregion
	void initializeMsgBox (string message, DialogueType type, params DialogueResultAction[] resultAction) {
		GameObject initMsgBox = Instantiate (dialogueBoxPrefab);
		initMsgBox.transform.SetParent(instantiateLocation,false);
		DialogueBoxUI dboxUI = initMsgBox.GetComponent<DialogueBoxUI> ();
		dboxUI.Initialize (message, type);

		dialogueBoxes.Add (new DialogueBox (message, initMsgBox, resultAction));
	}
	void removeMessageBox (GameObject go) {
		if (dialogueBoxes.Count > 0)
			for (int i = dialogueBoxes.Count - 1; i >= 0; i--) {
				if (dialogueBoxes[i].messageBox == go) {

					dialogueBoxes[i].messageBox.GetComponent<DialogueBoxUI> ().Remove ();
					dialogueBoxes.RemoveAt (i);
				}
			}
	}
	public void AcceptResponse (DialogueResult result, GameObject sender) {
		Debug.Log ("You clicked " + result.ToString ());
		DialogueBox dbox = searchDialogueBox (sender);
		if (dbox == null) { Debug.LogError ("Query was null"); return; }
		UnityAction action = dbox.GetResultAction (result);
		if (action != null) {
			action.Invoke ();
			removeMessageBox (sender);
		} else {
			//TODO Add default Action
			removeMessageBox (sender);
		}
	}

	DialogueBox searchDialogueBox (GameObject query) {
		for (int i = 0; i < dialogueBoxes.Count; i++) {
			if (query == dialogueBoxes[i].messageBox) {
				return dialogueBoxes[i];
			}
		}
		return null;
	}
}