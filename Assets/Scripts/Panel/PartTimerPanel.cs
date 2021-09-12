using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartTimerPanel : PanelBase
{
    private PartTimer partTimer;

    public override void Init(int num, Sprite sprite)
    {
        partTimer = GameManager.Instance.CurrentUser.partTimerList[num];
        base.Init(num, sprite);
        SetSoldItem();
        maxLevel = 10;
    }

    protected override void SetUp()
    {
        nameText.text = partTimer?.name;
        priceText.text = partTimer?.price + "��";
        levelText.text = partTimer?.level + " Level";
    }

    protected override void SecretInfo()
    {
        base.SecretInfo(partTimer.GetIsSold(), partTimer.price, partTimer.name);
    }

    public void OnClickUp()
    {
        if (GameManager.Instance.CurrentUser.money < partTimer.price) return;
        if (num != 0)
        {
            if (GameManager.Instance.CurrentUser.partTimerList[num - 1].level < maxLevel - 1) return;
        }

        GameManager.Instance.AddMoney(partTimer.price, false);
        partTimer.LevelUp();

        GameManager.Instance.uiManager.UpdatePanel();
        GameManager.Instance.uiManager.ActivePartTimers();

        SetSoldItem();
        SetUp();
    }

    public override void Inactive()
    {
        base.Inactive(partTimer.GetIsSold(), partTimer.price, partTimer.level);
    }

    private void SetSoldItem()
    {
        if (partTimer.GetIsSold())
        {
            levelText.transform.SetSiblingIndex(levelText.transform.GetSiblingIndex() - 1);
            itemImage.color = Color.white;
        }
    }

    protected override bool IsSecret()
    {
        if (num == 0) return false;
        return GameManager.Instance.CurrentUser.partTimerList[num - 1].level < maxLevel;
    }
}