using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAnswerList : MonoBehaviour {
    // Pool Variable
    public GameObject UiAnswerButton;
    public Transform answerContainer;
    public TextMeshProUGUI poolNameTxt;

    //Single Answers
    public TextMeshProUGUI answerTxt;
    public Image answerImg;
    public GameObject UiAnswerPanel;
    public void ShowAnswerList (AnswerPool ansPool) {
        poolNameTxt.text = ansPool.answerPoolName; 
        GameObject ansUI;
        foreach (Answer ans in ansPool.answers) {
            ansUI = Instantiate (UiAnswerButton, Vector3.zero, Quaternion.identity, answerContainer);
            ansUI.transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text = ans.text;
            ansUI.GetComponent<Button> ().onClick.AddListener (delegate { ShowAnswer (ans); });
        }
        gameObject.SetActive (true);
    }

    public void ShowAnswer (Answer ans) {

        UiAnswerPanel.SetActive (true);
        answerImg.sprite = ans.img;
        answerTxt.text = ans.text;
    }
}