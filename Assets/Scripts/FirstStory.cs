using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FirstStory : MonoBehaviour
{
    [SerializeField] private Image[] storyImages;
    [SerializeField] private GameObject tutorialPanel;

    private ParticleSystem particle;

    void Start()
    {
        if (GameManager.Instance.CurrentUser.isTutorial)
        {
            gameObject.SetActive(false);
            return;
        }

        particle = GetComponentInChildren<ParticleSystem>();
        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particle.gameObject.SetActive(false);

        if (!GameManager.Instance.CurrentUser.isTutorial)
        {
            transform.parent.GetChild(1).gameObject.SetActive(false);
            StartCoroutine(StoryTelling());
        }
    }

    private IEnumerator StoryTelling()
    {
        yield return new WaitForSeconds(4f);

        for (int i = 0; i < storyImages.Length; i++)
        {
            if (i == 2)
            {
                yield return new WaitForSeconds(1f);
                particle.Play();
                particle.gameObject.SetActive(true);
            }

            else
            {
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                particle.gameObject.SetActive(false);
            }

            storyImages[i].DOFade(0f, 1f);
            yield return new WaitForSeconds(4f);
        }

        gameObject.SetActive(false);
        Camera.main.transform.position = Vector3.zero;
        tutorialPanel.SetActive(true);
    }
}