using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControlConnection : MonoBehaviour {
	SceneController sceneController;

	// Use this for initialization
	void Start () {
		if (PlayerManager.singleton == null) {

			Debug.LogError ("Player manager was not found!");
			return;
		}
		sceneController = PlayerManager.singleton.GetComponent<SceneController>();
			if (sceneController == null) {

			Debug.LogError ("Scene Controller on Player Manager was not found!");
			return;
		}
	}

	public void ChangeScene(int sceneIndex){
		sceneController.LoadScene(sceneIndex);
	}
	public void ChangeScene(string sceneName){
		sceneController.LoadScene(sceneName);
	}
	
	public void LoadLastScene(){
		sceneController.LoadLastScene();
	}

}