using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutletPanel : PanelBase
{
    private Cutlet cutlet;
    private ParticleSystem particle;

    public override void Init(int num, Sprite sprite = null, int state = 0)
    {
        maxLevel = 5;

        base.Init(num, sprite);
        this.num = num;
        cutlet = GameManager.Instance.CurrentUser.cutlets[num];

        SetUp();
        SetSoldItem();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    public override void SetUp()
    {
        nameText.text = cutlet?.name;
        levelText.text = cutlet?.level + " Level";
        priceText.text = cutlet?.price + "¿ø";
    }

    public void OnClickCutlet()
    {
        if (GameManager.Instance.CurrentUser.money < cutlet.price)
            return;

        if (num != 0)
            if (IsSecret()) return;

        GameManager.Instance.AddMoney(cutlet.price, false);
        cutlet = GameManager.Instance.CurrentUser.cutlets.Find(x => x.name == cutlet.name);
        cutlet.LevelUp();
        GameManager.Instance.SetCutletPrice();
        GameManager.Instance.uiManager.UpdatePanel();
        GameManager.Instance.CurrentUser.Quests[2].PlusCurValue(1);
        GameManager.Instance.questManager.UpdateQuest();
        particle.Play();

        SetUp();
        SetSoldItem();
    }

    public override void Inactive()
    {
        base.Inactive(cutlet.GetIsSold(), cutlet.price, cutlet.level);
    }

    protected override void SecretInfo()
    {
        base.SecretInfo(cutlet.GetIsSold(), cutlet.price, cutlet.name);
    }

    private void SetSoldItem()
    {
        if (cutlet.GetIsSold())
        {
            levelText.transform.SetSiblingIndex(levelText.transform.GetSiblingIndex() - 1);
            itemImage.color = Color.white;
        }
    }

    protected override bool IsSecret()
    {
        if (num == 0) return false;
        return GameManager.Instance.CurrentUser.cutlets[num - 1].level < maxLevel;
    }
}