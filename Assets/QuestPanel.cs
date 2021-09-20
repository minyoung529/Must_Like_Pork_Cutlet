using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : PanelBase
{
    [SerializeField] Slider slider;
    private Quest quest;
    private int index;

    public override void Init(int num, Sprite sprite = null, int state = 0)
    {
        index = num;
        quest = GameManager.Instance.CurrentUser.Quests[index];

        nameText.text = quest.questName;
        levelText.text = quest.GetReward().ToString();
        priceText.text = string.Format("{0} / {1}", Mathf.Clamp(quest.GetCurValue(),0, quest.GetMaxValue()), quest.GetMaxValue());
        slider.maxValue = quest.GetMaxValue();
    }

    public override void SetUp()
    {
        int cur = quest.GetCurValue();
        int max = quest.GetMaxValue();

        slider.value = cur;
        priceText.text = string.Format("{0} / {1}", cur, max);

        if (cur < max)
        {
            buttonImage.color = new Color32(164, 164, 164, 255);
        }

        else
        {
            buttonImage.color = new Color32(255, 217, 0, 255);
        }
    }

    public void OnClickReward()
    {
        if(quest.GetCurValue() >= quest.GetMaxValue())
        {
            GameManager.Instance.AddDiamond(quest.GetReward());
            quest.SetCurValueZero();
            SetUp();
            GameManager.Instance.uiManager.UpdatePanel();
        }
    }
}
