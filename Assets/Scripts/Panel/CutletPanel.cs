using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutletPanel : PartTimerPanel
{
    Cutlet cutlet;

    public override void Init(int num)
    {
        this.num = num;
        cutlet = GameManager.Instance.GetCutlets()[num];
        SetUp();
    }

    protected override void SetUp()
    {
        nameText.text = cutlet.name;
        levelText.text = cutlet.level.ToString() + " Level";
        priceText.text = cutlet.price.ToString() + "¿ø";
    }

    public void OnClickCutlet()
    {
        if (GameManager.Instance.CurrentUser.money < cutlet.price)
            return;

        GameManager.Instance.CurrentUser.money -= cutlet.price;
        cutlet.LevelUp();
        SetUp();
        GameManager.Instance.SetCutletPrice();
        GameManager.Instance.uiManager.UpdatePanel();
    }

    public override void Inactive()
    {
        if (GameManager.Instance.CurrentUser?.money < cutlet?.price)
        {
            if (cutlet.level < 1)
            {
                if (num > 0)
                {
                    if (GameManager.Instance.CurrentUser?.money + 10 < cutlet?.price)
                    {
                        gameObject.SetActive(true);
                        isSecret = true;
                    }
                }


                if (num > 1)
                {
                    if (GameManager.Instance.CurrentUser?.money + 100 < cutlet?.price)
                        gameObject.SetActive(false);
                }

            }

            else
            {
                gameObject.SetActive(true);
                isSecret = false;
            }

            buttonImage.color = new Color32(0, 0, 0, 121);

            if (isSecret)
            {
                SecretInfo();
            }
        }

        else
        {
            buttonImage.color = Color.clear;
            isSecret = false;
            SetUp();
        }
    }

    protected override void SecretInfo()
    {
        nameText.text = "???? ???";
        priceText.text = cutlet.price.ToString() + "¿ø";
        levelText.text = "";
    }
}