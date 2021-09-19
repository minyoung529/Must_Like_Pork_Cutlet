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
        quest = GameManager.Instance.questManager.GetQuests()[index];

        nameText.text = quest.questName;
        levelText.text = quest.GetReward().ToString();
        priceText.text = string.Format("{0} / {1}", quest.GetCurValue(), quest.GetMaxValue());
        slider.maxValue = quest.GetMaxValue();
    }

    protected override void SetUp()
    {
        slider.value = quest.GetCurValue();
    }
}
