using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerPanel : PanelBase
{
    private Hammer hammer;
    [SerializeField]
    private GameObject lineObject;

    public void OnClickHammer()
    {
    }

    public override void Init(int num, Sprite sprite)
    {
        base.Init(num, sprite);
        hammer = GameManager.Instance.CurrentUser.hammerList[num];
        SetColor();
        ChangeLine();
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
        if (hammer.grade == "common" && GameManager.Instance.CurrentUser.hammerList[num + 1].grade == "rare")
        {
            if (transformNumber % 3 != 1)
            {
                if (transformNumber % 3 == 0)
                {
                    obj = Instantiate(lineObject, transform.parent);
                    obj.transform.SetSiblingIndex(transformNumber + 1);
                }

                else
                {
                    obj = Instantiate(lineObject, transform.parent);
                    obj.transform.SetSiblingIndex(transformNumber + 1);
                    obj.SetActive(true);
                    obj = Instantiate(lineObject, transform.parent);
                    obj.transform.SetSiblingIndex(transformNumber + 1);
                }
                obj.SetActive(true);

            }
        }

        else if (hammer.grade == "rare" && GameManager.Instance.CurrentUser.hammerList[num + 1].grade == "legendary")
        {
            if (transformNumber % 3 != 1)
            {
                if (transformNumber % 3 == 0)
                {
                    obj = Instantiate(lineObject, transform.parent);
                    obj.transform.SetSiblingIndex(transformNumber + 1);
                }

                else
                {
                    obj = Instantiate(lineObject, transform.parent);
                    obj.transform.SetSiblingIndex(transformNumber + 1);
                    obj = Instantiate(lineObject, transform.parent);
                    obj.transform.SetSiblingIndex(transformNumber + 1);
                }
                obj.SetActive(true);
            }
        }

    }
}