using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHammerPanel : PanelBase
{
    private ParticleSystem particle;
    private ParticleSystem.MainModule mainModule;

    private Hammer hammer;

    private void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        mainModule = particle.main;

        particle.Pause();
    }

    public override void Init(int num, Sprite sprite = null, int state = 0)
    {
        hammer = GameManager.Instance.CurrentUser.hammerList.Find(x => x.code == num);
        hammer.amount++;
        hammer.SetIsSold(true);
        SetParticle();
        SetColor(hammer.grade);
        GameManager.Instance.UIManager.UpdatePanel();
        base.Init(num, sprite);
    }

    private void SetColor(string grade)
    {
        switch (grade)
        {
            case "common":
                buttonImage.color = new Color32(166, 166, 166, 255);
                break;
            case "rare":
                buttonImage.color = new Color32(200, 138, 255, 255);
                break;
            case "legendary":
                buttonImage.color = new Color32(255, 200, 61, 255);
                break;
        }
    }

    public override Hammer GetHammer()
    {
        return hammer;
    }

    private void SetParticle()
    {
        if (hammer.grade == "common")
        {
            Inactive();
        }

        else if (hammer.grade == "rare")
        {
            particle.gameObject.SetActive(true);
            particle.Play();
            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.white, new Color32(200, 138, 255, 255));
        }

        else if (hammer.grade == "legendary")
        {
            particle.gameObject.SetActive(true);
            particle.Play();
            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.white, new Color32(255, 200, 61, 255));
        }
    }

    public override void Inactive()
    {
        particle.gameObject.SetActive(false);
        particle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}