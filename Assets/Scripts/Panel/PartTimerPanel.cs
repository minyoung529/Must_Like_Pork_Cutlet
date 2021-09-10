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
        base.Init(num, sprite); ;
    }

    protected override void SetUp()
    {
        nameText.text = partTimer?.name;
        priceText.text = partTimer?.price + "원";
        levelText.text = partTimer?.level + " Level";
    }

    protected override void SecretInfo()
    {
        if (!partTimer.GetIsSold())
            nameText.text = QuestionMark(partTimer.name);

        priceText.text = partTimer.price + "원";

        if (num > 0)
        {
            if (GameManager.Instance.CurrentUser.partTimerList[num - 1].level < 9)
                levelText.text = "조건: " + GameManager.Instance.CurrentUser.partTimerList[num - 1].name + " 레벨 10 이상";
        }
        isSecret = false;
    }

    public void OnClickUp()
    {
        if (GameManager.Instance.CurrentUser.money < partTimer.price) return;
        if (num != 0)
            if (GameManager.Instance.CurrentUser.partTimerList[num - 1].level < 9) return;
        GameManager.Instance.CurrentUser.money -= partTimer.price;
        partTimer.LevelUp();
        SetUp();
        GameManager.Instance.uiManager.UpdatePanel();
    }

    public override void Inactive()
    {

        if (num != 0)
        {
            Debug.Log(GameManager.Instance.CurrentUser.partTimerList[num - 1].level);

            if (GameManager.Instance.CurrentUser.partTimerList[num - 1].level > 9)
            {
                levelText.text = "";
            }
        }

        if (GameManager.Instance.CurrentUser?.money < partTimer?.price)
        {
            if (partTimer.level < 1)
            {
                //물음표 보임
                if (GameManager.Instance.CurrentUser?.money + partTimer.price * 0.2f < partTimer?.price)
                {
                    gameObject.SetActive(true);
                    SecretInfo();
                }
            }

            buttonImage.color = new Color32(0, 0, 0, 121);
            SecretInfo();
        }

        else
        {
            if (num != 0)
            {
                if (GameManager.Instance.CurrentUser.partTimerList[num - 1].level < 10)
                {
                    return;
                }
            }

            buttonImage.color = Color.clear;
            SetUp();
        }

    }
}
