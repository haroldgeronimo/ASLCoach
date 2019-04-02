using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAudioPlayer : MonoBehaviour
{
    public AudioClip audio;
    AudioManager AM;
    void Start()
    {
		AM = AudioManager.instance;
		if(AM != null)
		AM.PlayMusic(audio);
    }
}
