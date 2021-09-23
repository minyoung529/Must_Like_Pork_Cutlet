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
    [SerializeField] private AudioClip waterButton;
    [SerializeField] private AudioClip cancelButton;

    [SerializeField] private AudioClip moneySound;
    [SerializeField] private AudioClip nyamSound;
    [SerializeField] private AudioClip paperSound;
    [SerializeField] private AudioClip rewardSound;
    [SerializeField] private AudioClip levelUpSound;
    [SerializeField] private AudioClip ddiringSound;
    [SerializeField] private AudioClip tadaSound;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        backgroundAudioSource = GetComponent<AudioSource>();
        effectAudioSource = transform.GetChild(0).GetComponent<AudioSource>();

        LobbyBackground();
        StartCoroutine(EffectSound());
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

    private IEnumerator EffectSound()
    {
        effectAudioSource.volume = 0f;
        yield return new WaitForSeconds(0.5f);
        effectAudioSource.volume = 1f;
    }

    public void PopButton()
        => effectAudioSource.PlayOneShot(popButton);

    public void BpButton()
        => effectAudioSource.PlayOneShot(bpButton);

    public void PaperSound()
        => effectAudioSource.PlayOneShot(paperSound);

    public void WaterButton()
        => effectAudioSource.PlayOneShot(waterButton);

    public void CancelButton()
    => effectAudioSource.PlayOneShot(cancelButton);

    public void MoneySound()
        => effectAudioSource.PlayOneShot(moneySound);

    public void NyamSound()
        => effectAudioSource.PlayOneShot(nyamSound);

    public void RewardSound()
        => effectAudioSource.PlayOneShot(rewardSound);

    public void LevelUpSound()
        => effectAudioSource.PlayOneShot(levelUpSound);

    public void DdiringSound()
        => effectAudioSource.PlayOneShot(ddiringSound);

    public void TadaSound()
    => effectAudioSource.PlayOneShot(tadaSound);

}
