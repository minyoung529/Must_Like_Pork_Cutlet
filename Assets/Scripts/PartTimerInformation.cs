using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartTimerInformation : HammerInformation
{
    [SerializeField]
    private Text detailText;
    PartTimer partTimer;

    protected override void SetUp()
    {
        itemName.text = partTimer.name;
        levelText.text = partTimer.level.ToString();
        information.text = partTimer.story;
        detailText.text = partTimer.story;
        ability.text = string.Format("ÃÊ´ç {0}¿ø", partTimer.mps);

        itemImage.sprite = GameManager.Instance.UIManager.partTimerSprites[CheckIndex(partTimer)];
    }

    private int CheckIndex(PartTimer partTimer)
    {
        List<PartTimer> partTimers = GameManager.Instance.CurrentUser.partTimerList;

        for (int i = 0; i < partTimers.Count; i++)
        {
            if (partTimer == partTimers[i])
                return i;
        }
        return 0;
    }

    public override void Next()
    {
        if (index == GameManager.Instance.CurrentUser.partTimerList.Count - 1) index = -1;
        index++;
        partTimer = GameManager.Instance.CurrentUser.partTimerList.Find(x => x.code == index);
        SetUp();
    }

    public override void Previous()
    {
        if (index == 0) index = GameManager.Instance.CurrentUser.partTimerList.Count;
        index--;
        partTimer = GameManager.Instance.CurrentUser.partTimerList.Find(x => x.code == index);
        SetUp();
    }

    public override void SetIndex(int index)
    {
        gameObject.SetActive(true);
        this.index = index;
        partTimer = GameManager.Instance.CurrentUser.partTimerList.Find(x => x.code == this.index);
        SetUp();
    }
}
