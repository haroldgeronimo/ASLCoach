using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	public int lastScene = 0;
	public void LoadScene(int sceneBuildIndex){
		lastScene =  SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(sceneBuildIndex);
	
	}

	public void LoadScene(string sceneName){
		SceneManager.LoadScene(sceneName);
		
	}

	public void LoadLastScene(){
		SceneManager.LoadScene(lastScene);
	}
}
