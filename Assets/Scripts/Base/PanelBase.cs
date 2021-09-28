using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelBase : MonoBehaviour
{
    [SerializeField] protected Text nameText;
    [SerializeField] protected Text priceText;
    [SerializeField] protected Text levelText;
    [SerializeField] protected Image itemImage;
    [SerializeField] protected Image buttonImage;

    protected int num;
    protected int maxLevel;

    public virtual void Init(int num, Sprite sprite = null, int state = 0)
    {
        this.num = num;
        itemImage.sprite = sprite;
        SetUp();
    }

    public virtual void SetUp() { }

    protected virtual string QuestionMark(string name)
    {
        char[] nameToChar = new char[name.Length];
        string questionMarkName;

        for (int i = 0; i < name.Length; i++)
        {
            if (name[i] == ' ')
                nameToChar[i] = ' ';
            else
                nameToChar[i] = '?';

        }

        questionMarkName = new string(nameToChar);
        return questionMarkName;
    }

    protected virtual void SecretInfo() { }
    protected virtual void SecretInfo(bool isSold, ulong price, string name)
    {
        if (!isSold && IsSecret())
        {
            nameText.text = QuestionMark(name);
            if (num > 0)
            {
                itemImage.color = Color.black;
            }
        }

        else
        {
            nameText.text = name;
            itemImage.color = Color.white;
        }

        priceText.text = GameManager.Instance.ConvertMoneyText(price);
        buttonImage.color = new Color32(0, 0, 0, 121);

        if (num > 0)
        {
            if (IsSecret())
                levelText.text = "조건: 앞 상품 레벨 10 이상";
        }
    }

    public virtual void Inactive()
    {
    }

    public virtual void Inactive(bool isSold, ulong price, int level)
    {
        //잠금해제 후 팔린 거, 0 제외
        if (num != 0)
        {
            if (!IsSecret() && isSold)
            {
                SetUp();
            }
        }

        //현재 돈이 그 가격보다 적을 때
        if (GameManager.Instance.CurrentUser?.money < price)
        {
            buttonImage.color = new Color32(0, 0, 0, 121);
        }

        //팔리지 않았으면 감춘다
        if (!isSold && num != 0)
        {
            SecretInfo();
        }

        if (!IsSecret() && GameManager.Instance.CurrentUser?.money >= price)
        {
            buttonImage.color = Color.clear;
            SetUp();
        }
    }

    protected virtual bool IsSecret() { return false; }
    public virtual void SetActiveCheck() { }
    public virtual void Mounting() { }
    public virtual Hammer GetHammer()
    {
        return null;
    }
}