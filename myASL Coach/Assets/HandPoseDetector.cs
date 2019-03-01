using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity;
using UnityEngine;
using UnityEngine.UI;

public class HandPoseDetector : MonoBehaviour {
	public PhoneCamera camera;
	public Text debugText;
	ColorBlobDetector detector;
	// Use this for initialization
	void Awake () {
		detector = new ColorBlobDetector ();
	}
	void Start () {

	}

	public char CharacterDetection () {
		debugText.text = "Start Detection \n";
		Debug.Log ("Detecting");
		if (camera.backCam == null) {
			
		debugText.text = "No cam \n";
			Debug.Log ("No cam");
			return ' ';
		}
		int width, height;
		width = camera.backCam.width;
		height = camera.backCam.height;
		Mat rgbMat = new Mat (height, width, CvType.CV_8UC1);

		if (rgbMat.cols () != width || rgbMat.rows () != height) {

		debugText.text = "Not same parameters for Mat \n";

			Debug.Log ("Not same parameters for Mat");
			return ' ';
		}
		debugText.text += "Mat created \n";

		Utils.webCamTextureToMat (camera.backCam, rgbMat);
		debugText.text += "Texture to Mat \n";

		Imgproc.GaussianBlur (rgbMat, rgbMat, new Size (1, 1), 1, 1);
		debugText.text += "Guassian Blur \n";

		List<MatOfPoint> contours = detector.getContours ();
		detector.process (rgbMat);
		debugText.text += "Contours \n";

		//DetectionDebug (rgbMat);
		//debugText.text += "Debug \n";

		string alphabet = "abcdefghijklmnopqrstuvwxyz";
		return alphabet[Random.Range (0, alphabet.Length)];
	}

	public void DetectionDebug (Mat rgbMat) {
		Texture texture = new Texture2D (rgbMat.cols (), rgbMat.rows ());
		debugText.text += "New Texture \n";

		Utils.matToTexture (rgbMat, texture);
		debugText.text += "Mat to Texture \n";

		camera.background.texture = texture;
		debugText.text += "Cam set to new texure \n";


	}
}