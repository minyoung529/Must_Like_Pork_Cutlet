using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FirstStory : MonoBehaviour
{
    [SerializeField] private Image[] storyImages;
    ParticleSystem particle;
    private Image image;

    void Start()
    {
        if(GameManager.Instance.CurrentUser.isTutorial)
        {
            gameObject.SetActive(false);
        }

        image = GetComponent<Image>();
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
        yield return new WaitForSeconds(6f);

        for (int i = 0; i < storyImages.Length; i++)
        {
            if (i == 2)
            {
                yield return new WaitForSeconds(2.7f);
                particle.Play();
                particle.gameObject.SetActive(true);
            }

            else
            {
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                particle.gameObject.SetActive(false);
            }

            storyImages[i].DOFade(0f, 1f);
            yield return new WaitForSeconds(6.8f);
        }

        gameObject.SetActive(false);
        transform.parent.GetChild(1).gameObject.SetActive(true);
        GameManager.Instance.CurrentUser.isTutorial = true;
        Camera.main.transform.position = new Vector3(0f, 3f, 0f);
    }
}