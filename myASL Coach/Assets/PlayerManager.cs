using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	public static PlayerManager singleton;
	//Player data
	public int points = 0;

	void Awake () {
		if(singleton != null){
		Destroy(this.gameObject);
		}
		else
		{
		singleton = this;
		DontDestroyOnLoad(this.gameObject);
		}
	}
}