using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    private AudioSource backgroundAudioSource;
    private AudioSource effectAudioSource;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        backgroundAudioSource = GetComponent<AudioSource>();
        effectAudioSource = GetComponentInChildren<AudioSource>();

        Debug.Log(backgroundAudioSource.gameObject.name);
        Debug.Log(effectAudioSource.gameObject.name);
    }

}
