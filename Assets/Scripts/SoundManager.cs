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

    [SerializeField] private AudioClip popButton;
    [SerializeField] private AudioClip bpButton;
    [SerializeField] private AudioClip paperSound;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        backgroundAudioSource = GetComponent<AudioSource>();
        effectAudioSource = transform.GetChild(0).GetComponent<AudioSource>();

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

    public void PopButton()
        => effectAudioSource.PlayOneShot(popButton);

    public void BpButton()
        => effectAudioSource.PlayOneShot(bpButton);

    public void PaperSound()
        => effectAudioSource.PlayOneShot(paperSound);
}
