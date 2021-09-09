using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutletPanel : PartTimerPanel
{
    Cutlet cutlet;

    public override void Init(int num)
    {
        cutlet = GameManager.Instance.GetCutlets()[num];
        nameText.text = cutlet.name;
        levelText.text = cutlet.level.ToString() + " Level";
        priceText.text = cutlet.price.ToString();
    }
}
