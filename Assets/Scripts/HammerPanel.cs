using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerPanel : PartTimerPanel
{
    private Hammer hammer;

    public void OnClickHammer()
    {
    }
    public override void Init(int num)
    {
        hammer = GameManager.Instance.GetHammers()[num];
        nameText.text = hammer.name;
        levelText.text = hammer.level.ToString() + " Level";
        priceText.text = hammer.grade;
    }
}
