using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    public AudioClip ClickButtonClip;

    private void Awake()
    {
        instance = this;
    }

    public void PlayClickButtonClip(AudioSource asr)
    {
        asr.volume = StaticData.AudioVolume;
        asr.PlayOneShot(ClickButtonClip);
    }

    public static void PlayClip(AudioSource asr, AudioClip clip)
    {
        //asr.volume = StaticData.AudioVolume;
        asr.PlayOneShot(clip);
    }
}
