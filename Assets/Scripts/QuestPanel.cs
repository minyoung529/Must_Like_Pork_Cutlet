using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : PanelBase
{
    [SerializeField] Slider slider;
    private Quest quest;
    private int index;
    private bool isReward = false;
    public bool IsReward { get { return isReward; } }

    public override void Init(int num, Sprite sprite = null, int state = 0)
    {
        index = num;
        quest = GameManager.Instance.CurrentUser.Quests[index];

        nameText.text = quest.questName;
        levelText.text = quest.GetReward().ToString();
        priceText.text = string.Format("{0} / {1}", quest.GetCurValue(), quest.GetMaxValue());
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
            isReward = false;
            buttonImage.color = new Color32(164, 164, 164, 255);
        }

        else
        {
            isReward = true;
            buttonImage.color = new Color32(255, 217, 0, 255);
        }
    }

    public void OnClickReward()
    {
        if(quest.GetCurValue() >= quest.GetMaxValue())
        {
            GameManager.Instance.AddDiamond(quest.GetReward());
            quest.SetCurValue(quest.GetCurValue()- quest.GetMaxValue());
            SetUp();
            GameManager.Instance.UIManager.UpdatePanel();

            if (quest.GetCurValue() >= quest.GetMaxValue())
            {
                isReward = true;
            }

            else
            {
                isReward = false;
            }
        }
    }

}
