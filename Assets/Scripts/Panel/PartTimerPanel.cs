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

    protected bool isSecret;
    protected int num;


    public void Update()
    {
        Inactive();
    }
    public virtual void Init(int num)
    {
        this.num = num;
        partTimer = GameManager.Instance.CurrentUser.partTimerList[num];
        SetUp();
        QuestionMark();

    }

    protected virtual void SetUp()
    {
        nameText.text = partTimer.name;
        priceText.text = partTimer.price.ToString() + "¿ø";
        levelText.text = partTimer.level.ToString() + " Level";
    }

    protected virtual void SecretInfo()
    {
        nameText.text = "???? ???";
        priceText.text = partTimer.price.ToString() + "¿ø";
        levelText.text = "";
    }

    private string QuestionMark()
    {
        char[] nameToChar = new char[partTimer.name.Length];
        string questionMarkName;

        for (int i = 0; i < partTimer.name.Length; i++)
        {
            if (partTimer.name[i] == ' ')
                nameToChar[i] = ' ';
            else
                nameToChar[i] = '?';

        }

        questionMarkName = new string(nameToChar);
        return questionMarkName;
    }

    public void OnPointerUp()
    {
        if (GameManager.Instance.CurrentUser.money < partTimer.price) return;
        GameManager.Instance.CurrentUser.money -= partTimer.price;
        partTimer.LevelUp();
        SetUp();
        GameManager.Instance.uiManager.UpdatePanel();
    }

    public virtual void Inactive()
    {
        if (GameManager.Instance.CurrentUser?.money < partTimer?.price)
        {
            if (partTimer.level < 1)
            {
                if (num > 0)
                {
                    if (GameManager.Instance.CurrentUser?.money + 10 < partTimer?.price)
                    {
                        gameObject.SetActive(true);
                        nameText.text = QuestionMark();
                        isSecret = true;
                    }
                }


                if (num > 1)
                {
                    if (GameManager.Instance.CurrentUser?.money + 100 < partTimer?.price)
                        gameObject.SetActive(false);
                }

            }

            else
            {
                gameObject.SetActive(true);
                isSecret = false;
            }

            buttonImage.color = new Color32(0, 0, 0, 121);

            if (isSecret)
            {
                SecretInfo();
            }
        }

        else
        {
            buttonImage.color = Color.clear;
        }
    }
}
