using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoSingleton<SoundManager>
{
    private AudioSource backgroundAudioSource;
    private AudioSource effectAudioSource;

    [SerializeField] private AudioClip lobbyBGM;
    [SerializeField] private AudioClip gameBGM;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        backgroundAudioSource = GetComponent<AudioSource>();
        effectAudioSource = GetComponentInChildren<AudioSource>();

        LobbyBackground();
    }

    public void LobbyBackground()
    {
        backgroundAudioSource.clip = lobbyBGM;
        backgroundAudioSource.Play();
    }

    public void OnClickStart()
    {
        StartCoroutine(FadeSound(gameBGM));
    }

    private IEnumerator FadeSound(AudioClip clip)
    {
        backgroundAudioSource.DOFade(0f, 2f);
        yield return new WaitForSeconds(1.5f);
        backgroundAudioSource.clip = clip;
        backgroundAudioSource.Play();
        backgroundAudioSource.DOFade(1f, 2f);
    }
}
