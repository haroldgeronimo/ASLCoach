using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public CameraLocation[] locations;
	public float smoothTime = 5;
	//setup variables
	bool isChangePos = false;
	Vector3 target;
	void Start () {
		isChangePos = false;
		changeCameraPosition (0);
	}
	/* 
		void Update () {
			if (Input.GetKeyDown (KeyCode.Q)) changeCameraPosition (0);
			if (Input.GetKeyDown (KeyCode.W)) changeCameraPosition (1);
			if (Input.GetKeyDown (KeyCode.E)) changeCameraPosition (2);
			if (Input.GetKeyDown (KeyCode.R)) changeCameraPosition (3);
		} */
	private void LateUpdate () {
		if (isChangePos) {
			this.transform.position = Vector3.Lerp (this.transform.position, target, Time.deltaTime * smoothTime);
			if (target == this.transform.position)
				isChangePos = false;
		}
	}
	public void changeCameraPosition (int i) {
		Vector3 newPosition = getLocationByIndex (i);
		target = newPosition;
		isChangePos = true;
	}
	public void changeCameraPosition (string name) {
		Vector3 newPosition = getLocationByName (name);
		target = newPosition;
		isChangePos = true;
	}
	public void changeCameraPosition (Vector3 newPosition) {
		target = newPosition;
		isChangePos = true;
	}
	public void changeCameraPosition (Answer answer) {
		foreach (CameraLocation loc in locations) {
			if (answer.type == loc.answerType) {
				target = loc.location;
				isChangePos = true;
				break;
			}

		}
	}

	//getters
	public Vector3 getLocationByIndex (int i) {
		if (i >= locations.Length || i < 0) return Vector3.zero;
		return locations[i].location;
	}
	public Vector3 getLocationByName (string name) {
		foreach (CameraLocation loc in locations) {
			if (name == loc.locationName)
				return loc.location;
		}
		return Vector3.zero;
	}
}