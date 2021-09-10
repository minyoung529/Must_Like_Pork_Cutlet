using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelBase : MonoBehaviour
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

    protected bool isSecret;
    protected int num;

    public virtual void Init(int num, Sprite sprite)
    {
        this.num = num;
        itemImage.sprite = sprite;
        SetUp();
    }

    protected virtual void SetUp() { }

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

    public virtual void Inactive() { }

    protected virtual void SecretInfo() { }
}