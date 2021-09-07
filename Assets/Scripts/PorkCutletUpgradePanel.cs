using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PorkCutletUpgradePanel : MonoBehaviour
{
    private int abilityNum;

    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Image cutletImage;

    private PorkCutlet porkCutlet;

    public void Init(int abilityNum)
    {
        this.abilityNum = abilityNum;
        porkCutlet = GameManager.Instance.GetUser().cutletList[this.abilityNum];
    }

    public void SetUp()
    {
        nameText.text = porkCutlet.name;
        priceText.text = porkCutlet.price.ToString();
        levelText.text = porkCutlet.level.ToString();
    }
}
