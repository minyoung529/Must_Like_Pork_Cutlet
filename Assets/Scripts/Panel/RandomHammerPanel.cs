using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHammerPanel : PanelBase
{
    Hammer hammer;

    public override void Init(int num, Sprite sprite = null, int state = 0)
    {
        hammer = GameManager.Instance.CurrentUser.hammerList[num];
        base.Init(num, sprite);
    }
}
