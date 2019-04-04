using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SfxManager : MonoBehaviour {
  public AudioManager AM;
  public List<SoundFX> SfxLibrary = new List<SoundFX> ();


  public void PlaySfx (SfxType type) {
    SoundFX sfx = sfxQuery (null, type);
    AM.PlaySound2D (sfx.clip);
  }
  public void PlaySfx (string name) {
    SoundFX sfx = sfxQuery (name);
    AM.PlaySound2D (sfx.clip);
  }
  #region Queries
  SoundFX sfxQuery (string clipName = null, SfxType type = SfxType.none) {
    if (clipName == null && type == SfxType.none) return null;
    foreach (SoundFX sfx in SfxLibrary) {
      if (clipName != null && type != SfxType.none) {
        if (sfx.name == clipName && sfx.type == type)
          return sfx;
      } else if (clipName != null && type == SfxType.none) {
        if (sfx.name == clipName)
          return sfx;
      } else {
        if (sfx.type == type)
          return sfx;
      }
    }
    return null;
  }
  #endregion
  [System.Serializable ()]
  public class SoundFX {
    public string name;
    public SfxType type;
    public AudioClip clip;
  }

}

public enum SfxType { click, disabled,alert, warning,success,failed, negative, positive, correct, wrong, none };