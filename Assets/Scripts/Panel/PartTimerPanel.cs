using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartTimerPanel : PanelBase
{
    [SerializeField] private Text nextMPS;
    private PartTimer partTimer;
    private ParticleSystem particle;

    public override void Init(int num, Sprite sprite = null, int state = 0)
    {
        partTimer = GameManager.Instance.CurrentUser.partTimerList[num];
        particle = GetComponentInChildren<ParticleSystem>();
        base.Init(num, sprite);
        SetSoldItem();
        maxLevel = 10;
    }

    public override void SetUp()
    {
        nameText.text = partTimer?.name;
        priceText.text = GameManager.Instance.ConvertMoneyText(partTimer.price);
        levelText.text = string.Format("{0} Level",partTimer?.level);
        nextMPS.text = string.Format("+ √ ¥Á {0}", GameManager.Instance.ConvertMoneyText((ulong) partTimer.GetNextMPS()));
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
        particle.Play();
        SoundManager.Instance.LevelUpSound();

        GameManager.Instance.UIManager.UpdatePanel();
        GameManager.Instance.UIManager.ActivePartTimers();
        GameManager.Instance.CurrentUser.Quests[3].PlusCurValue(1);
        GameManager.Instance.QuestManager.UpdateQuest();

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