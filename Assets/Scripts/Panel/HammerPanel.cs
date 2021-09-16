using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HammerPanel : PanelBase
{
    private Hammer hammer;
    [SerializeField]
    private GameObject lineObject;
    [SerializeField]
    private Image checkImage;
    //¹ÌÃÆ³Ä ¹Î¿µ¾Æ ¿Ö ÄÚµå¸£ ÀÌ·°¤¾°Ô Â®¾î ³Ê ÇÇ°ïÇß´Ï 
    public void OnClickHammer()
    {
        //if (!hammer.isSold) return;
        SetHammer();
    }

    public override void Init(int num, Sprite sprite)
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
        if (!hammer.GetIsSold())
        {
            itemImage.color = Color.black;
        }
    }

    public override void SetActiveCheck()
    {
        checkImage.gameObject.SetActive(false);
    }

    private void SetHammer()
    {
        GameManager.Instance.CurrentUser.UserHammer(hammer.name);
        GameManager.Instance.uiManager.CheckHammer();
        checkImage.gameObject.SetActive(true);
        GameManager.Instance.uiManager.ChangeHammerSprite(itemImage.sprite);
    }
}