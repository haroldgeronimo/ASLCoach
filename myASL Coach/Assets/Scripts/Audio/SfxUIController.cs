using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxUIController : MonoBehaviour
{
    public void buttonClick()
    {

        AudioManager.instance.SfxManager.PlaySfx(SfxType.click);
    }
        public void alert()
    {

        AudioManager.instance.SfxManager.PlaySfx(SfxType.alert);
    }

}
