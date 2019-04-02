using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxUIController : MonoBehaviour
{
    public void buttonClick()
    {

        AudioManager.instance.SfxManager.buttonClick();
    }
}
