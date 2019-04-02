using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public AudioManager AM;
    public string buttonClickSoundName;
    public string notificationSoundName;

    public void buttonClick()
    {
		AM.PlaySound2D(buttonClickSoundName);
    }
}
