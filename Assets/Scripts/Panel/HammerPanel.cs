using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HammerPanel : PanelBase
{
    private Hammer hammer;

    [SerializeField] private GameObject lineObject;
    [SerializeField] private Image checkImage;

    public void OnClickHammer()
    {
        //여기계속오류
        GameManager.Instance.UIManager.ActiveHammerInfo(hammer.code);
    }

    public override void Init(int num, Sprite sprite = null, int state = 0)
    {
        base.Init(num, sprite);
        hammer = GameManager.Instance.CurrentUser.hammerList[num];
        SetColor();
        ChangeLine();
        SetSoldItem();
    }

    private void SetColor()
    {
        if (hammer.grade == "common")
            buttonImage.color = new Color32(207, 207, 207, 255);

        else if (hammer.grade == "rare")
            buttonImage.color = new Color32(190, 150, 250, 255);

        else if (hammer.grade == "legendary")
            buttonImage.color = new Color32(255, 216, 76, 255);
    }

    private void ChangeLine()
    {
        if (num == GameManager.Instance.CurrentUser.hammerList.Count - 1) return;

        GameObject obj;
        int transformNumber = transform.GetSiblingIndex();
        string nextGrade = hammer.grade == "common" ? "rare" : "legendary";

        if (hammer.grade != nextGrade && GameManager.Instance.CurrentUser.hammerList[num + 1].grade == nextGrade)
        {
            if (transformNumber % 3 != 1)
            {
                if (transformNumber % 3 == 0)
                {
                    obj = Instantiate(lineObject, transform.parent);
                    obj.transform.SetSiblingIndex(transformNumber + 1);
                    obj.SetActive(true);
                }

                else
                {
                    obj = Instantiate(lineObject, transform.parent);
                    obj.transform.SetSiblingIndex(transformNumber + 1);
                    obj.SetActive(true);
                    obj = Instantiate(lineObject, transform.parent);
                    obj.transform.SetSiblingIndex(transformNumber + 1);
                    obj.SetActive(true);
                }
            }
        }
    }

    private void SetSoldItem()
    {
        if (hammer.amount == 0)
        {
            itemImage.color = Color.black;
        }
        else
        {
            itemImage.color = Color.white;
        }
    }

    public override void SetActiveCheck()
    {
        checkImage.gameObject.SetActive(false);
    }

    public override void Mounting()
    {
        if (hammer.amount == 0) return;

        GameManager.Instance.CurrentUser.UserHammer(hammer.name);
        GameManager.Instance.UIManager.CheckHammer();
        checkImage.gameObject.SetActive(true);
        GameManager.Instance.UIManager.ChangeHammerSprite(itemImage.sprite);
    }

    public override void Inactive()
    {
        if (hammer.amount == 0)
        {
            buttonImage.color = Color.gray;

            if (hammer.isSold)
            {
                itemImage.color = Color.white;
                return;
            }

            itemImage.color = Color.black;
        }

        else
        {
            buttonImage.color = Color.white;
            itemImage.color = Color.white;
        }
    }
}