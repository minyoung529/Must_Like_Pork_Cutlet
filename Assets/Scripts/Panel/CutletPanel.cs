using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutletPanel : PanelBase
{
    private Cutlet cutlet;

    public override void Init(int num, Sprite sprite)
    {
        maxLevel = 10;
        cutlet = GameManager.Instance.CurrentUser.cutlets[num];
        base.Init(num, sprite);
        SetUp();
        SetSoldItem();
    }

    protected override void SetUp()
    {
        nameText.text = cutlet?.name;
        levelText.text = cutlet?.level + " Level";
        priceText.text = cutlet?.price + "¿ø";
    }

    public void OnClickCutlet()
    {
        if (GameManager.Instance.CurrentUser.money < cutlet.price)
            return;

        GameManager.Instance.AddMoney(cutlet.price, false);
        cutlet.LevelUp();
        GameManager.Instance.SetCutletPrice();
        GameManager.Instance.uiManager.UpdatePanel();
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