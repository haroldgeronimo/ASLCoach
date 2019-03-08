using System;
using UnityEngine;
using UnityEngine.Events;
public enum DialogueType {
	YesNo,
	YesNoCancel,
	Okay
}

public enum DialogueResult {
	Yes,
	No,
	Cancel,
	Okay
}

public struct DialogueResultAction {
	public DialogueResult response;
	public UnityAction action;
	/// <summary>
	/// Initialize Dialogue Result Action item
	/// </summary>
	/// <param name="res">response</param>
	/// <param name="act">action</param>
	public DialogueResultAction (DialogueResult res, UnityAction act) {
		response = res;
		action = act;
	}
}

public class DialogueBox {
	public string message;
	public GameObject messageBox;

	public DialogueResultAction[] resultActions;

	public DialogueBox (string msg, GameObject msgBox, DialogueResultAction[] resultActionArr) {
		message = msg;
		messageBox = msgBox;
		resultActions = resultActionArr;
	}

	public UnityAction GetResultAction (DialogueResult result) {
		if (resultActions.Length > 0)
			for (int i = 0; i < resultActions.Length; i++) {
				if (resultActions[i].response == result) {
					return resultActions[i].action;
				}
			}
		return null;
	}

}

[Serializable]
public struct DialogueTypeUI {
	public DialogueType type;
	public GameObject UI;
}

public static class MsgBox {
	#region Show Messages
	/// <summary>
	/// Show message with Yes and No actions
	/// </summary>
	/// <param name="message">message you you want to show</param>
	/// <param name="yesAction">action performed when Yes is selected</param>
	/// <param name="noAction">action performed when No is selected</param>
	public static void ShowMsgYesNo (
		string message,
		UnityAction yesAction = null,
		UnityAction noAction = null) {
		DialogueManager DM = PlayerManager.singleton.gameManager.DM;
		if (DM != null)
			DM.ShowMsgYesNo (message, yesAction, noAction);
	}

	/// <summary>
	/// Show message with Yes,No, and Cancel actions
	/// </summary>
	/// <param name="message">message you you want to show</param>
	/// <param name="yesAction">action performed when Yes is selected</param>
	/// <param name="noAction">action performed when No is selected</param>
	/// <param name="cancelAction">action performed when Cancel is selected</param>
	public static void ShowMsgYesNoCancel (
		string message,
		UnityAction yesAction = null,
		UnityAction noAction = null,
		UnityAction cancelAction = null) {
		DialogueManager DM = PlayerManager.singleton.gameManager.DM;
		if (DM != null)
			DM.ShowMsgYesNoCancel (message, yesAction, noAction, cancelAction);
	}
	public static void ShowMsgOkay (
		string message,
		UnityAction okayAction = null) {
		DialogueManager DM = PlayerManager.singleton.gameManager.DM;
		if (DM != null)
			DM.ShowMsgOkay (message, okayAction);
	}
	#endregion
}