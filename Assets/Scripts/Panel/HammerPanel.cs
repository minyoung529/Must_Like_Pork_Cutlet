using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerPanel : PanelBase
{
    private Hammer hammer;

    public void OnClickHammer()
    {
    }
    public override void Init(int num, Sprite sprite)
    {
        hammer = GameManager.Instance.CurrentUser.hammerList[num];
        //nameText.text = hammer.name;
        //levelText.text = hammer.level.ToString() + " Level";
        //priceText.text = hammer.grade;
    }
}
