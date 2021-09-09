using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartTimerPanel : MonoBehaviour
{
    [SerializeField]
    protected Text nameText;
    [SerializeField]
    protected Text priceText;
    [SerializeField]
    protected Text levelText;
    [SerializeField]
    protected Image itemImage;
    [SerializeField]
    protected Image buttonImage;

    private PartTimer partTimer;

    public void Update()
    {
        Inactive();
    }
    public virtual void Init(int num)
    {
        partTimer = GameManager.Instance.CurrentUser.partTimerList[num];
        Debug.Log(partTimer.name);
        SetUp();
    }

    public void SetUp()
    {
        nameText.text = partTimer.name;
        priceText.text = partTimer.price.ToString() + "¿ø";
        levelText.text = partTimer.level.ToString() + " Level";
    }

    public void OnPointerUp()
    {
        if (GameManager.Instance.CurrentUser.money < partTimer.price) return;
        GameManager.Instance.CurrentUser.money -= partTimer.price;
        partTimer.level++;
        partTimer.PlusMPS();
        SetUp();
        GameManager.Instance.uiManager.UpdatePanel();
    }

    protected virtual void Inactive()
    {
        if (GameManager.Instance.CurrentUser?.money < partTimer?.price)
        {
            buttonImage.color = new Color32(0, 0, 0, 121);
        }

        else
        {
            buttonImage.color = Color.clear;
        }
    }
}
